using System.Collections.Generic;
using AstroDX.Contexts.Gameplay.SlideGenerators;
using SimaiSharp.Structures;

namespace AstroCreate.Utilities;

public class SlideUtility
{
    public static readonly IReadOnlyList<Location> TAP_LIST = new[]
    {
        new Location(1, NoteGroup.ASensor), new Location(2, NoteGroup.ASensor), new Location(3, NoteGroup.ASensor),
        new Location(4, NoteGroup.ASensor), new Location(5, NoteGroup.ASensor), new Location(6, NoteGroup.ASensor),
        new Location(7, NoteGroup.ASensor), new Location(8, NoteGroup.ASensor)
    };

    public static SlideGenerator? MakeSlideGenerator(SlideSegment segment)
    {
        if (segment.vertices.Count < 2)
            return null;

        return segment.slideType switch
        {
            SlideType.CurveCcw => new CurveCcwGenerator(segment.vertices),
            SlideType.CurveCw => new CurveCwGenerator(segment.vertices),
            SlideType.EdgeCurveCcw => new EdgeCurveCcwGenerator(segment.vertices),
            SlideType.EdgeCurveCw => new EdgeCurveCwGenerator(segment.vertices),
            SlideType.EdgeFold => new EdgeFoldGenerator(segment.vertices),
            SlideType.Fold => new FoldGenerator(segment.vertices),
            SlideType.RingCcw => new RingCcwGenerator(segment.vertices),
            SlideType.RingCw => new RingCwGenerator(segment.vertices),
            SlideType.ZigZagS => new ZigZagSGenerator(segment.vertices),
            SlideType.ZigZagZ => new ZigZagSGenerator(segment.vertices),
            SlideType.StraightLine => new StraightGenerator(segment.vertices),
            _ => null
        };
    }
}