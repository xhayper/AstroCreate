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

    public readonly float length;

    public readonly List<List<Node2D>> SlideNodeList = new();
    public readonly List<SlidePath> SlidePaths;

    public Slide(Node parentNode, Note note, List<SlidePath> slidePaths)
    {
        SlidePaths = slidePaths;

        foreach (var generator in note.slidePaths
                     .Select(slidePath => SlideUtility.MakeSlideGenerator(note, slidePath))
                     .SelectMany(slideGenerators => slideGenerators))
        {
            List<Node2D> nodeList = new();

            var slideLength = generator.GetLength();
            var viewportSize = parentNode.GetViewport();
            length += slideLength;

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

                nodeList.Add(slide);
            }

            SlideNodeList.Add(nodeList);
        }
    }

    public void SetVisible(bool visible)
    {
        foreach (var node in SlideNodeList.SelectMany(pathNodeList => pathNodeList)) node.Visible = visible;
    }

    /**
     * @param t 0-1
     */
    public void SetVisible(float t)
    {
        var currentLength = 0f;

        foreach (var node in SlideNodeList.SelectMany(pathNodeList => pathNodeList))
        {
            node.Visible = currentLength / length >= Mathf.Clamp(t, 0, 1);
            currentLength += RenderManager.SlideSpacing;
        }
    }

    public void QueueFree()
    {
        foreach (var node in SlideNodeList.SelectMany(pathNodeList => pathNodeList)) node.QueueFree();
    }
}