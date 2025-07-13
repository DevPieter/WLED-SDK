using System.Net.WebSockets;
using System.Text.Json;
using DevPieter.WLED_SDK.Core;
using DevPieter.WLED_SDK.Core.Models.WledInfo;
using DevPieter.WLED_SDK.Core.Models.WledState;
using DevPieter.WLED_SDK.Core.WledEventArgs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace DevPieter.WLED_SDK.Client.WebSocket;

public class WledWebsocketClient : IWledClient, IDisposable
{
    private readonly WebsocketClient _client;
    private readonly ILogger<WledWebsocketClient>? _logger;
    private readonly TimeSpan _noResponseTimeout = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The URI of the WLED device.
    /// </summary>
    public Uri Uri { get; }

    public Info? Info { get; private set; }
    public State? State { get; private set; }

    /// <summary>
    /// Similar to <see cref="Core.Models.WledState.State.TransitionDuration"/>, but only applies to the requests from this client.
    /// <br/><br/>
    /// Setting this to -1 will use <see cref="Core.Models.WledState.State.TransitionDuration"/> instead.
    /// </summary>
    public int ClientTransitionTime { get; set; } = -1;

    /// <summary>
    /// Invoked when the info of the WLED device changes.
    /// </summary>
    public event EventHandler<InfoChangedEventArgs>? OnInfoChanged;

    /// <summary>
    /// Invoked when the state of the WLED device changes.
    /// </summary>
    public event EventHandler<StateChangedEventArgs>? OnStateChanged;

    /// <summary>
    /// Invoked when the client disconnects from the WLED device.
    /// </summary>
    public event EventHandler<DisconnectionInfo>? OnDisconnected;

    public WledWebsocketClient(Uri uri, ILogger<WledWebsocketClient>? logger = null)
    {
        Uri = uri;
        _logger = logger;

        _client = new WebsocketClient(Uri);
        _client.IsReconnectionEnabled = false;

        _client.MessageReceived.Subscribe(OnMessageReceived);
        _client.DisconnectionHappened.Subscribe(OnDisconnectionHappened);
    }

    public WledWebsocketClient(string url, ILogger<WledWebsocketClient>? logger = null) : this(GetUri(url), logger)
    {
    }

    private static Uri GetUri(string url)
    {
        if (!url.StartsWith("ws://") && !url.StartsWith("wss://")) url = $"ws://{url}";
        if (!url.EndsWith("/ws")) url = $"{url}/ws";
        return new Uri(url);
    }

    /// <summary>
    /// Returns whether the client is connected to the WLED device.
    /// </summary>
    public bool IsConnected => _client is { IsRunning: true, IsStarted: true };

    /// <summary>
    /// Connects the client to the WLED device.
    /// </summary>
    /// <param name="waitUntilReady">Whether to wait until the client receives the info and state of the WLED device. (Recommended)</param>
    /// <param name="cancellationToken">The cancellation token to cancel the connection attempt.</param>
    /// <exception cref="InvalidOperationException">Thrown when the client is already connected.</exception>
    public async Task ConnectAsync(bool waitUntilReady = true, CancellationToken cancellationToken = default)
    {
        if (IsConnected) throw new InvalidOperationException("Client is already connected.");
        _logger?.LogInformation("Connecting to {Uri}...", Uri);

        try
        {
            await _client.StartOrFail();
            _logger?.LogInformation("Connected to {Uri}.", Uri);

            if (!waitUntilReady) return;
            var readyTask = BlockUntilReadyAsync();
            var timeoutTask = Task.Delay(_noResponseTimeout, cancellationToken);

            await Task.WhenAny(readyTask, timeoutTask);
            if (!readyTask.IsCompleted) throw new TimeoutException("The client did not receive the info and state of the WLED device in time.");
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Failed to connect to {Uri}.", Uri);
            throw;
        }
    }

    /// <summary>
    /// Disconnects the client from the WLED device.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the client is not connected.</exception>
    public async Task StopAsync()
    {
        if (!_client.IsRunning) throw new InvalidOperationException("Client is not running.");
        if (!_client.IsStarted) throw new InvalidOperationException("Client is not started.");
        _logger?.LogInformation("Disconnecting from {Uri}...", Uri);

        try
        {
            await _client.StopOrFail(WebSocketCloseStatus.NormalClosure, "Client stopped.");
        }
        catch
        {
            // Ignored
        }
    }

    public async Task SendJsonAsync<T>(T message)
    {
        if (message is null) throw new ArgumentNullException(nameof(message));
        if (!IsConnected) throw new InvalidOperationException("Client is not connected.");

        await Task.Run(() =>
        {
            var jsonObject = JObject.FromObject(message);

            // Inject the custom transition time if it's not -1
            if (ClientTransitionTime is not -1) jsonObject.Add("tt", Math.Clamp(ClientTransitionTime, 0, 65535));

            var json = jsonObject.ToString(Formatting.None);

            _logger?.LogDebug("Sending JSON message to {Uri}: {Message}", Uri, json);
            _client.Send(json);
        });
    }

    /// <summary>
    /// Requests the current info and state of the WLED device. If the client responds, the <see cref="OnStateChanged"/> and <see cref="OnInfoChanged"/> events will be invoked.
    /// <remarks>Normally you don't need to call this method because the WLED device sends its info and state when changes occur.</remarks>
    /// </summary>
    public async Task RequestUpdateAsync() => await SendJsonAsync(new { v = true });

    /// <summary>
    /// Disconnects from the WLED device and disposes the <see cref="WebsocketClient"/>.
    /// </summary>
    public async void Dispose()
    {
        try
        {
            await StopAsync();
        }
        catch
        {
            // Ignored
        }

        _client.Dispose();
    }

    private async Task BlockUntilReadyAsync()
    {
        var infoReceivedTask = new TaskCompletionSource<bool>();
        var stateReceivedTask = new TaskCompletionSource<bool>();

        if (Info is null) OnInfoChanged += InfoChangedInternal;
        else infoReceivedTask.SetResult(true);

        if (State is null) OnStateChanged += StateChangedInternal;
        else stateReceivedTask.SetResult(true);


        await Task.WhenAll(infoReceivedTask.Task, stateReceivedTask.Task);
        return;

        void InfoChangedInternal(object? sender, InfoChangedEventArgs e)
        {
            infoReceivedTask.SetResult(true);
            OnInfoChanged -= InfoChangedInternal;
        }

        void StateChangedInternal(object? sender, StateChangedEventArgs e)
        {
            stateReceivedTask.SetResult(true);
            OnStateChanged -= StateChangedInternal;
        }
    }

    private void OnMessageReceived(ResponseMessage msg)
    {
        if (msg.MessageType is not WebSocketMessageType.Text || msg.Text is null) return;

        try
        {
            /* Try to parse the JSON message into a Info object. */
            var jsonInfo = JsonDocument.Parse(msg.Text).RootElement.GetProperty("info");
            var info = JsonConvert.DeserializeObject<Info>(jsonInfo.ToString());
            if (info is not null) UpdateInfoFromMessage(info);
        }
        catch
        {
            // Ignored
        }

        try
        {
            /* Try to parse the JSON message into a State object. */
            var jsonState = JsonDocument.Parse(msg.Text).RootElement.GetProperty("state");
            var state = JsonConvert.DeserializeObject<State>(jsonState.ToString());
            if (state is not null) UpdateStateFromMessage(state);
        }
        catch
        {
            // Ignored
        }
    }

    private void OnDisconnectionHappened(DisconnectionInfo info)
    {
        UpdateInfoFromMessage(null);
        UpdateStateFromMessage(null);

        if (info.Type is DisconnectionType.ByUser) _logger?.LogInformation("{Uri} successfully disconnected from WLED.", Uri);
        else _logger?.LogError("{Uri} disconnected from WLED: {Type}", Uri, info.Type);

        OnDisconnected?.Invoke(this, info);
    }

    private void UpdateInfoFromMessage(Info? info)
    {
        var previousInfo = Info;
        Info = info;

        OnInfoChanged?.Invoke(this, new InfoChangedEventArgs(previousInfo, info));
    }

    private void UpdateStateFromMessage(State? state)
    {
        var previousState = State;
        State = state;

        OnStateChanged?.Invoke(this, new StateChangedEventArgs(previousState, state));
    }
}