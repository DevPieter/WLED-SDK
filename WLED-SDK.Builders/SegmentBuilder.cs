using System.Drawing;
using DevPieter.WLED_SDK.Core;
using DevPieter.WLED_SDK.Core.Extensions;
using DevPieter.WLED_SDK.Core.Models.WledState;
using DevPieter.WLED_SDK.Extensions.State;
using Newtonsoft.Json.Linq;

namespace DevPieter.WLED_SDK.Builders;

public class SegmentBuilder(IWledClient client)
{
    private readonly List<JObject> _segments = [];

    /// <summary>
    /// The segments that have been added to the builder.
    /// </summary>
    public IReadOnlyList<JObject> Segments => _segments.AsReadOnly();

    /// <remarks>Requires <see cref="IWledClient"/> to be provided.</remarks>
    public SegmentBuilder AddSegment(int start, int stop, string? name = null, int? id = null)
    {
        var info = client.GetInfoOrThrow();
        var ledCount = info.LedInfo.Count;
        var segmentCount = info.LedInfo.MaxSegments;

        if (_segments.Count >= segmentCount) throw new InvalidOperationException($"Cannot add more than {segmentCount} segments.");

        var segment = new JObject
        {
            ["id"] = Math.Clamp(id ?? _segments.Count, 0, segmentCount - 1),
            ["start"] = Math.Clamp(start, 0, ledCount - 1),
            ["stop"] = Math.Clamp(stop, start, ledCount)
        };

        if (!string.IsNullOrWhiteSpace(name)) segment["n"] = name;
        _segments.Add(segment);
        return this;
    }

    /// <summary>
    /// Sets all segment settings to their default values.
    /// </summary>
    public SegmentBuilder SetDefaults(bool setColor = false)
    {
        Group(0);
        Spacing(0);
        Offset(0);
        Effect(0);
        Speed(128);
        Intensity(128);
        Palette(0);
        Selected(true);
        Reverse(false);
        On(true);
        Brightness(255);
        Mirror(false);
        ColorTemperature(255);
        Freeze(false);
        Set(0);

        EffectCustomSlider1(0);
        EffectCustomSlider2(0);
        EffectCustomSlider3(0);
        EffectOption1(false);
        SetEffectOption2(false);
        EffectOption3(false);

        if (!setColor) return this;
        var color = System.Drawing.Color.FromArgb(255, 160, 0);
        Color(color, color, color);

        return this;
    }

    /// <inheritdoc cref="Segment.Group"/>
    public SegmentBuilder Group(int group)
    {
        GetLastSegmentOrThrow()["grp"] = Math.Clamp(group, 0, 255);
        return this;
    }

    /// <inheritdoc cref="Segment.Spacing"/>
    public SegmentBuilder Spacing(int spacing)
    {
        GetLastSegmentOrThrow()["spc"] = Math.Clamp(spacing, 0, 255);
        return this;
    }

    /// <inheritdoc cref="Segment.Offset"/>
    public SegmentBuilder Offset(int offset)
    {
        GetLastSegmentOrThrow()["of"] = offset; // TODO - Clamp offset (-len+1 to len)
        return this;
    }

    /// <inheritdoc cref="Segment.Colors"/>
    public SegmentBuilder Color(Color primary, Color secondary = default, Color tertiary = default)
    {
        var lastSegment = GetLastSegmentOrThrow();

        primary = primary == System.Drawing.Color.Empty ? System.Drawing.Color.Black : primary;
        var colors = new JArray { primary.ToHexString(false) };

        if (secondary != System.Drawing.Color.Empty) colors.Add(secondary.ToHexString(false));
        if (tertiary != System.Drawing.Color.Empty) colors.Add(tertiary.ToHexString(false));

        lastSegment["col"] = colors;
        return this;
    }

    /// <remarks>Requires <see cref="IWledClient"/> to be provided.</remarks>
    /// <inheritdoc cref="Segment.EffectId"/>
    public SegmentBuilder Effect(int effectId)
    {
        GetLastSegmentOrThrow()["fx"] = client.ClampEffectId(effectId);
        return this;
    }

    /// <inheritdoc cref="Segment.Speed"/>
    public SegmentBuilder Speed(int speed)
    {
        GetLastSegmentOrThrow()["sx"] = Math.Clamp(speed, 0, 255);
        return this;
    }

    /// <inheritdoc cref="Segment.Intensity"/>
    public SegmentBuilder Intensity(int intensity)
    {
        GetLastSegmentOrThrow()["ix"] = Math.Clamp(intensity, 0, 255);
        return this;
    }

    /// <inheritdoc cref="Segment.EffectCustomSlider1"/>
    public SegmentBuilder EffectCustomSlider1(int slider1)
    {
        GetLastSegmentOrThrow()["c1"] = Math.Clamp(slider1, 0, 255);
        return this;
    }

    /// <inheritdoc cref="Segment.EffectCustomSlider2"/>
    public SegmentBuilder EffectCustomSlider2(int slider2)
    {
        GetLastSegmentOrThrow()["c2"] = Math.Clamp(slider2, 0, 255);
        return this;
    }

    /// <inheritdoc cref="Segment.EffectCustomSlider3"/>
    public SegmentBuilder EffectCustomSlider3(int slider3)
    {
        GetLastSegmentOrThrow()["c3"] = Math.Clamp(slider3, 0, 255);
        return this;
    }

    /// <inheritdoc cref="Segment.EffectOption1"/>
    public SegmentBuilder EffectOption1(bool option1 = true)
    {
        GetLastSegmentOrThrow()["o1"] = option1;
        return this;
    }

    /// <inheritdoc cref="Segment.EffectOption2"/>
    public SegmentBuilder SetEffectOption2(bool option2 = true)
    {
        GetLastSegmentOrThrow()["o2"] = option2;
        return this;
    }

    /// <inheritdoc cref="Segment.EffectOption3"/>
    public SegmentBuilder EffectOption3(bool option3 = true)
    {
        GetLastSegmentOrThrow()["o3"] = option3;
        return this;
    }

    /// <remarks>Requires <see cref="IWledClient"/> to be provided.</remarks>
    /// <inheritdoc cref="Segment.PaletteId"/>
    public SegmentBuilder Palette(int paletteId)
    {
        GetLastSegmentOrThrow()["pal"] = client.ClampPaletteId(paletteId);
        return this;
    }

    /// <inheritdoc cref="Segment.Selected"/>
    public SegmentBuilder Selected(bool selected = true)
    {
        GetLastSegmentOrThrow()["sel"] = selected;
        return this;
    }

    /// <inheritdoc cref="Segment.Reverse"/>
    public SegmentBuilder Reverse(bool reverse = true)
    {
        GetLastSegmentOrThrow()["rev"] = reverse;
        return this;
    }

    /// <inheritdoc cref="Segment.On"/>
    public SegmentBuilder On(bool on = true)
    {
        GetLastSegmentOrThrow()["on"] = on;
        return this;
    }

    /// <inheritdoc cref="Segment.Brightness"/>
    public SegmentBuilder Brightness(int brightness)
    {
        GetLastSegmentOrThrow()["bri"] = Math.Clamp(brightness, 0, 255);
        return this;
    }

    /// <inheritdoc cref="Segment.Mirror"/>
    public SegmentBuilder Mirror(bool mirror = true)
    {
        GetLastSegmentOrThrow()["mi"] = mirror;
        return this;
    }

    /// <inheritdoc cref="Segment.ColorTemperature"/>
    public SegmentBuilder ColorTemperature(int colorTemperature)
    {
        if (colorTemperature is (< 0 or > 255) and (< 1900 or > 10091)) throw new ArgumentOutOfRangeException(nameof(colorTemperature), "Color temperature must be between 0 and 255 or 1900 and 10091.");
        GetLastSegmentOrThrow()["cct"] = colorTemperature;
        return this;
    }

    // TODO - Individual LED control

    /// <inheritdoc cref="Segment.Freeze"/>
    public SegmentBuilder Freeze(bool freeze = true)
    {
        GetLastSegmentOrThrow()["frz"] = freeze;
        return this;
    }

    // TODO - Sound reactive settings (SoundSimulationType, Expand1Dfx)

    /// <inheritdoc cref="Segment.Set"/>
    public SegmentBuilder Set(int set)
    {
        GetLastSegmentOrThrow()["set"] = Math.Clamp(set, 0, 3);
        return this;
    }

    // TODO - Check "rpt" (Flag to repeat current segment settings)
    public JObject Build() => new() { ["seg"] = new JArray(_segments) };

    public List<JObject> BuildSplit() => _segments.Select(segment => new JObject { ["seg"] = new JArray(segment) }).ToList();

    private JObject GetLastSegmentOrThrow() =>
        _segments.LastOrDefault() ?? throw new InvalidOperationException("No segments have been added.");
}