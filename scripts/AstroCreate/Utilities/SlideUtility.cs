using System.Collections.Generic;
using System.Linq;
using AstroDX.Contexts.Gameplay.SlideGenerators;
using Godot;
using SimaiSharp.Structures;

namespace AstroCreate.Utilities;

public static class SlideUtility
{
    public static readonly IReadOnlyList<Location> TAP_LIST = new[]
    {
        new Location(0, NoteGroup.Tap), new Location(1, NoteGroup.Tap), new Location(2, NoteGroup.Tap),
        new Location(3, NoteGroup.Tap), new Location(4, NoteGroup.Tap), new Location(5, NoteGroup.Tap),
        new Location(6, NoteGroup.Tap), new Location(7, NoteGroup.Tap)
    };

    public static SlideGenerator? MakeSlideGenerator(Location startLocation, SlideSegment segment)
    {
        var vertices = segment.vertices.ToList();
        vertices.Insert(0, startLocation);

        return segment.slideType switch
        {
            SlideType.CurveCcw => new CurveCcwGenerator(vertices),
            SlideType.CurveCw => new CurveCwGenerator(vertices),
            SlideType.EdgeCurveCcw => new EdgeCurveCcwGenerator(vertices),
            SlideType.EdgeCurveCw => new EdgeCurveCwGenerator(vertices),
            SlideType.EdgeFold => new EdgeFoldGenerator(vertices),
            SlideType.Fold => new FoldGenerator(vertices),
            SlideType.RingCcw => new RingCcwGenerator(vertices),
            SlideType.RingCw => new RingCwGenerator(vertices),
            SlideType.ZigZagS => new ZigZagSGenerator(vertices),
            SlideType.ZigZagZ => new ZigZagZGenerator(vertices),
            SlideType.StraightLine => new StraightGenerator(vertices),
            _ => null
        };
    }

    public static IReadOnlyList<SlideGenerator> MakeSlideGenerator(Note note, SlidePath path)
    {
        var generators = new List<SlideGenerator>();
        var startPosition = note.location;

        foreach (var segment in path.segments)
        {
            foreach (var verticies in segment.vertices)
                GD.Print(segment.slideType, " | ",
                    startPosition.group, startPosition.index, "=>", verticies.group, verticies.index);

            var generator = MakeSlideGenerator(startPosition, segment);
            startPosition = segment.vertices.Last();

            if (generator != null) generators.Add(generator);
        }

        return generators.AsReadOnly();
    }
}