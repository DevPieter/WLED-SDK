using Newtonsoft.Json.Linq;

namespace DevPieter.WLED_SDK.Builders;

public class PresetBuilder
{
    private readonly JObject _preset = new();

    public PresetBuilder(int presetId, string? name = null)
    {
        _preset["psave"] = Math.Clamp(presetId, 1, 250);
        if (!string.IsNullOrWhiteSpace(name)) _preset["n"] = name;
    }

    public PresetBuilder(int presetId, string? name, JObject apiCommand) : this(presetId, name)
    {
        _preset.Merge(apiCommand);
    }

    public PresetBuilder IncludeBrightness(bool include = true)
    {
        _preset["ib"] = include;
        return this;
    }

    public PresetBuilder IncludeSegments(bool save = true)
    {
        _preset["sb"] = save;
        return this;
    }

    public PresetBuilder CheckedSegmentsOnly(bool segmentsOnly = true)
    {
        _preset["sc"] = segmentsOnly;
        return this;
    }

    public PresetBuilder QuickLoadLabel(string? label)
    {
        _preset["ql"] = label;
        return this;
    }

    public JObject Build() => _preset;
}