using System.Threading.Tasks;
using AstroCreate.Gameplay;
using AstroCreate.Utilities;
using AstroDX.Contexts.Gameplay.PlayerScope;
using Godot;
using SimaiSharp;
using SimaiSharp.Structures;

namespace AstroCreate.Tests;

public partial class GameplayTest : Node
{
    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        var touchPrefab = ResourceLoader.Load<PackedScene>("res://prefabs/note/touch.tscn")
            .Instantiate() as Node2D;

        var chartData = FileAccess.Open("res://charts/超熊猫的周遊記（ワンダーパンダートラベラー)/maidata.txt",
            FileAccess.ModeFlags.Read).GetAsText();

        var chartDataStream = TextUtility.GenerateStreamFromString(chartData);

        var file = new SimaiFile(chartDataStream);
        var chart = SimaiConvert.Deserialize(file.GetValue("inote_5"));

        foreach (var noteCollection in chart.NoteCollections)
        {
            foreach (var note in noteCollection.ToArray())
                if (note.slidePaths.Count > 0)
                {
                    var slide = new Slide(this, note, note.slidePaths);

                    foreach (var node in slide.SlideNodeList) AddChild(node);

                    slide.SetVisible(.5f);

                    async Task Func()
                    {
                        for (var i = RenderManager.SlideSpacing; i < slide.length; i += RenderManager.SlideSpacing)
                        {
                            slide.SetVisible(i / slide.length);
                            await ToSignal(GetTree().CreateTimer(.05), "timeout");
                        }

                        slide.QueueFree();
                    }

                    Func();
                }
                else if (note.type is NoteType.Tap or NoteType.Break && note.length == null)
                {
                    GD.Print($"{note.type} | {note.location.group}{note.location.index}");

                    async Task Func()
                    {
                        var gridPosition = NoteUtility.GetPosition(note.location) * 50;
                        var modifiedPosition = GetViewport().GetVisibleRect().Size / 2;
                        modifiedPosition.X += gridPosition.X;
                        modifiedPosition.Y += -gridPosition.Y;

                        var touch = touchPrefab.Duplicate() as Node2D;
                        touch.Position = GetViewport().GetVisibleRect().Size / 2;
                        touch.SetMeta("IsBreak", Variant.From(note.type == NoteType.Break));
                        AddChild(touch);

                        var tween = CreateTween();
                        tween.TweenProperty(touch, "position", modifiedPosition, .25);
                        tween.Finished += async () =>
                        {
                            await ToSignal(GetTree().CreateTimer(note.length.GetValueOrDefault(.5f)), "timeout");
                            touch.QueueFree();
                        };
                        tween.Play();
                    }

                    Func();
                }

            await ToSignal(GetTree().CreateTimer(.2f), "timeout");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}