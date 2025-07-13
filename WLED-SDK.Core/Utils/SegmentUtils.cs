using DevPieter.WLED_SDK.Core.Models.WledState;
using Newtonsoft.Json.Linq;

namespace DevPieter.WLED_SDK.Core.Utils;

public static class SegmentUtils
{
    /// <summary>
    /// TODO - Move to SegmentExtensions?
    /// Gets the ids of the segments in the given state.
    /// </summary>
    /// <param name="state">The state to get the segment ids from.</param>
    /// <param name="selectedOnly">Whether to only get the ids of the selected segments.</param>
    /// <returns>The ids of the segments in the given state.</returns>
    public static int[] GetIds(State state, bool selectedOnly = false)
        => selectedOnly ? state.Segments.Where(segment => segment.Selected).Select(segment => segment.Id).ToArray() : state.Segments.Select(segment => segment.Id).ToArray();

    /// <summary>
    /// Clamps the given segments to the existing segments in the state.
    /// <br/><br/>
    /// If the given segments are null or empty, all existing segments will be returned.
    /// Replace -1 with the main segment id.
    /// Removes non-existent and duplicate segments.
    /// </summary>
    /// <param name="state">The state to clamp to.</param>
    /// <param name="segmentsIds">The segments to clamp.</param>
    /// <returns>The clamped existing non-duplicate segments.</returns>
    public static int[] ClampIds(State state, params int[]? segmentsIds)
    {
        if (segmentsIds is null || !segmentsIds.Any()) return GetIds(state);

        var result = new List<int>();
        var existing = GetIds(state);

        foreach (var segment in segmentsIds)
        {
            if (segment is -1) result.Add(state.MainSegmentId);
            if (!existing.Contains(segment)) continue;

            result.Add(segment);
        }

        return result.Distinct().ToArray();
    }

    /// <summary>
    /// TODO - Document
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="segmentsIds"></param>
    /// <returns></returns>
    /// 
    /// <example>
    /// In: new { fx = "r" }, [SEGMENTS]
    /// <br/>
    /// Out: { "seg": [ { "fx": "r", "id": [SEGMENT] }, { "fx": "r", "id": [SEGMENT] }, ... ] }
    /// </example>
    public static JObject CreateSegmentsWith(object obj, params int[] segmentsIds)
    {
        var array = new JArray();

        foreach (var segment in segmentsIds)
        {
            var clone = JObject.FromObject(obj);
            clone.Add("id", segment);

            array.Add(clone);
        }

        return new JObject { { "seg", array } };
    }
}