using DevPieter.WLED_SDK.Core.Enums;
using Newtonsoft.Json;

namespace DevPieter.WLED_SDK.Core.Models.WledState;

public class Segment
{
    [JsonProperty("id")] public int Id { get; private set; }

    [JsonProperty("start")] public int Start { get; private set; }

    [JsonProperty("stop")] public int Stop { get; private set; }

    [JsonProperty("len")] public int Length { get; private set; }
    
    [JsonProperty("grp")] public int Group { get; private set; }

    [JsonProperty("spc")] public int Spacing { get; private set; }

    [JsonProperty("of")] public int Offset { get; private set; }

    [JsonProperty("on")] public bool On { get; private set; }

    [JsonProperty("frz")] public bool Freeze { get; private set; }

    [JsonProperty("bri")] public int Brightness { get; private set; }

    [JsonProperty("cct")] public int ColorTemperature { get; private set; }

    [JsonProperty("set")] public int Set { get; private set; }

    [JsonProperty("n")] public string? Name { get; private set; }

    [JsonProperty("col")] public int[][] Colors { get; private set; } = null!;

    [JsonProperty("fx")] public int EffectId { get; private set; }

    [JsonProperty("sx")] public int Speed { get; private set; }

    [JsonProperty("ix")] public int Intensity { get; private set; }

    [JsonProperty("pal")] public int PaletteId { get; private set; }

    [JsonProperty("c1")] public int EffectCustomSlider1 { get; private set; }

    [JsonProperty("c2")] public int EffectCustomSlider2 { get; private set; }

    [JsonProperty("c3")] public int EffectCustomSlider3 { get; private set; }

    [JsonProperty("sel")] public bool Selected { get; private set; }

    [JsonProperty("rev")] public bool Reverse { get; private set; }

    [JsonProperty("mi")] public bool Mirror { get; private set; }

    [JsonProperty("o1")] public bool EffectOption1 { get; private set; }

    [JsonProperty("o2")] public bool EffectOption2 { get; private set; }

    [JsonProperty("o3")] public bool EffectOption3 { get; private set; }

    [JsonProperty("si")] public SoundSimulationType SoundSimulation { get; private set; }

    [JsonProperty("m12")] public Expand1Dfx Expand1Dfx { get; private set; }
}