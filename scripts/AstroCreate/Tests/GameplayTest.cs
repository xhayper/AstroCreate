using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstroCreate.Utilities;
using Godot;
using SimaiSharp;
using SimaiSharp.Structures;

namespace AstroCreate.Tests;

public partial class GameplayTest : Node
{
    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        var slidePrefab = ResourceLoader.Load<PackedScene>("res://prefabs/note/slide.tscn")
            .Instantiate() as Node2D;
        var touchPrefab = ResourceLoader.Load<PackedScene>("res://prefabs/note/touch.tscn")
            .Instantiate() as Node2D;

        var chartData = FileAccess.Open("res://charts/超熊猫的周遊記（ワンダーパンダートラベラー)/maidata.txt",
            FileAccess.ModeFlags.Read).GetAsText();

        var chartDataStream = TextUtility.GenerateStreamFromString(chartData);

        var file = new SimaiFile(chartDataStream);
        var chart = SimaiConvert.Deserialize(file.GetValue("inote_5"));

        foreach (var noteCollection in chart.NoteCollections)
        foreach (var note in noteCollection.ToArray())
        {
            if (note.slidePaths.Count > 0)
            {
                var slideList = new List<Node2D>();

                foreach (var generator in note.slidePaths
                             .Select(slidePath => SlideUtility.MakeSlideGenerator(note, slidePath))
                             .SelectMany(slideGenerators => slideGenerators))
                    for (var i = 0f; i < 1; i += .05f)
                    {
                        Vector2 location;
                        float rotation;

                        generator.GetPoint(i, out location, out rotation);

                        var gridPosition = location * 50;
                        var modifiedPosition = GetViewport().GetVisibleRect().Size / 2;
                        modifiedPosition.X += gridPosition.X;
                        modifiedPosition.Y += -gridPosition.Y;

                        var slide = slidePrefab.Duplicate() as Node2D;
                        slide.Position = modifiedPosition;
                        slide.Rotation = -rotation;

                        AddChild(slide);

                        slideList.Add(slide);
                    }

                async Task Func()
                {
                    await ToSignal(GetTree().CreateTimer(2.5), "timeout");
                    foreach (var slide in slideList) slide.QueueFree();
                }

                Func();
            }
            else if (note.type is NoteType.Tap or NoteType.Break)
            {
                GD.Print($"{note.type} | {note.location.group}{note.location.index}");

                async Task Func()
                {
                    var gridPosition = NoteUtility.GetPosition(note.location) * 50;
                    var modifiedPosition = GetViewport().GetVisibleRect().Size / 2;
                    modifiedPosition.X += gridPosition.X;
                    modifiedPosition.Y += -gridPosition.Y;

                    var touch = touchPrefab.Duplicate() as Node2D;
                    touch.Position = modifiedPosition;
                    touch.SetMeta("IsBreak", Variant.From(note.type == NoteType.Break));
                    AddChild(touch);

                    await ToSignal(GetTree().CreateTimer(note.length.GetValueOrDefault(.5f) + .5f), "timeout");
                    touch.QueueFree();
                }

                Func();
            }

            await ToSignal(GetTree().CreateTimer(note.length.GetValueOrDefault(.5f)), "timeout");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}