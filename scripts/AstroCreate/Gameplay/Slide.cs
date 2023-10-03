using System.Collections.Generic;
using System.Linq;
using AstroCreate.Utilities;
using AstroDX.Contexts.Gameplay.PlayerScope;
using Godot;
using SimaiSharp.Structures;

namespace AstroCreate.Gameplay;

public class Slide
{
    public static readonly Node2D SlidePrefab = ResourceLoader.Load<PackedScene>("res://prefabs/note/slide.tscn")
        .Instantiate() as Node2D;

    public readonly List<Node2D> SlideNodeList = new();
    public readonly List<SlidePath> SlidePaths;

    public Slide(Node parentNode, Note note, List<SlidePath> slidePaths)
    {
        SlidePaths = slidePaths;

        foreach (var generator in note.slidePaths
                     .Select(slidePath => SlideUtility.MakeSlideGenerator(note, slidePath))
                     .SelectMany(slideGenerators => slideGenerators))
        {
            var slideLength = generator.GetLength();
            var viewportSize = parentNode.GetViewport();

            for (var i = RenderManager.SlideSpacing; i < slideLength; i += RenderManager.SlideSpacing)
            {
                generator.GetPoint(Mathf.Clamp(i / slideLength, 0, 1), out var location, out var rotation);

                var gridPosition = location * 50;
                var modifiedPosition = viewportSize.GetVisibleRect().Size / 2;
                modifiedPosition.X += gridPosition.X;
                modifiedPosition.Y += -gridPosition.Y;

                var slide = SlidePrefab.Duplicate() as Node2D;
                slide.Position = modifiedPosition;
                slide.Rotation = -rotation;

                SlideNodeList.Add(slide);
            }
        }
    }

    public void QueueFree()
    {
        foreach (var node in SlideNodeList) node.QueueFree();
    }
}