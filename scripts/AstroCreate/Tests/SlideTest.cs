using System.Collections.Generic;
using System.Linq;
using AstroCreate.Utilities;
using Godot;
using SimaiSharp;

namespace AstroCreate.Tests;

public partial class SlideTest : Node
{
    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        var squarePrefab = ResourceLoader.Load<PackedScene>("res://prefabs/note/slide.tscn")
            .Instantiate() as Node2D;

        var chartData = FileAccess.Open("res://charts/超熊猫的周遊記（ワンダーパンダートラベラー)/maidata.txt",
            FileAccess.ModeFlags.Read).GetAsText();

        var chartDataStream = TextUtility.GenerateStreamFromString(chartData);

        var file = new SimaiFile(chartDataStream);
        var chart = SimaiConvert.Deserialize(file.GetValue("inote_5"));

        foreach (var noteCollection in chart.NoteCollections)
        foreach (var note in noteCollection.ToArray())
        {
            if (note.slidePaths.Count <= 0) continue;

            var slideList = new List<Node2D>();

            GD.Print(note.location.group, note.location.index);

            foreach (var generator in note.slidePaths
                         .Select(slidePath => SlideUtility.MakeSlideGenerator(note, slidePath))
                         .SelectMany(slideGenerators => slideGenerators))
                for (var i = 0f; i < 1; i += .05f)
                {
                    Vector2 location;
                    float rotation;

                    generator.GetPoint(i, out location, out rotation);

                    var square = squarePrefab.Duplicate() as Node2D;
                    square.Position = location * 50 + GetViewport().GetVisibleRect().Size / 2;
                    square.Rotation = rotation;
                    AddChild(square);

                    slideList.Add(square);
                }

            await ToSignal(GetTree().CreateTimer(2.5), "timeout");

            foreach (var slide in slideList) slide.QueueFree();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}