using System.Linq;
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
    public AudioStreamPlayer? bgMusicPlayer;
    public VideoStreamPlayer? bgVideoPlayer;

    public MaiChart chart;
    public float firstNoteTime;
    public NoteCollection[] noteCollections;

    public int noteIndex;

    private AudioStream? slideNoise = ResourceLoader.Exists("res://sounds/slide.wav")
        ? ResourceLoader.Load<AudioStream>("res://sounds/slide.wav")
        : null;

    private AudioStream? tapNoise = ResourceLoader.Exists("res://sounds/slide.wav")
        ? ResourceLoader.Load<AudioStream>("res://sounds/tap.wav")
        : null;

    public double timeElapsed;

    private Node2D touchPrefab = ResourceLoader.Load<PackedScene>("res://prefabs/note/touch.tscn")
        .Instantiate() as Node2D;

    public AudioStreamPlayer CreateStreamPlayer(string path)
    {
        return CreateStreamPlayer(ResourceLoader.Load<AudioStream>(path));
    }

    public AudioStreamPlayer CreateStreamPlayer(AudioStream audioStream)
    {
        var audioStreamPlayer = new AudioStreamPlayer();
        audioStreamPlayer.Stream = audioStream;
        return audioStreamPlayer;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bgVideoPlayer = GetNode<VideoStreamPlayer>("Node2D/VideoStreamPlayer");

        var CHART_NAME = "超熊猫的周遊記（ワンダーパンダートラベラー）";
        var CHART_DIFF = "5";

        var chartData = FileAccess.Open($"res://charts/{CHART_NAME}/maidata.txt",
            FileAccess.ModeFlags.Read).GetAsText();

        var chartDataStream = TextUtility.GenerateStreamFromString(chartData);

        var file = new SimaiFile(chartDataStream);
        var firstTime = file.GetValue("first");

        chart = SimaiConvert.Deserialize(file.GetValue($"inote_{CHART_DIFF}"));
        noteCollections = chart.NoteCollections.ToArray();

        if (!float.TryParse(firstTime, out firstNoteTime)) firstNoteTime = noteCollections[0].time;

        if (
            ResourceLoader.Exists($"res://charts/{CHART_NAME}/track.mp3"))
        {
            var musicSoundPlayer = CreateStreamPlayer($"res://charts/{CHART_NAME}/track.mp3");
            musicSoundPlayer.Finished += () => musicSoundPlayer.QueueFree();
            AddChild(musicSoundPlayer);
            musicSoundPlayer.Play();
            bgMusicPlayer = musicSoundPlayer;
        }

        if (ResourceLoader.Exists($"res://charts/{CHART_NAME}/pv.ogv"))
        {
            var videoStreamPlayer = ResourceLoader.Load<VideoStream>($"res://charts/{CHART_NAME}/pv.ogv");
            bgVideoPlayer.Stream = videoStreamPlayer;
            bgVideoPlayer.Play();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (bgMusicPlayer != null)
        {
            if (!bgMusicPlayer.Playing)
                return;

            if (bgMusicPlayer.Stream.GetLength() >= timeElapsed &&
                bgMusicPlayer.GetPlaybackPosition() - timeElapsed >= .1f)
                bgMusicPlayer.Seek((float)timeElapsed);

            if (noteIndex >= noteCollections.Length &&
                bgMusicPlayer.Stream.GetLength() >= timeElapsed)
                timeElapsed += delta;
        }

        if (noteIndex >= noteCollections.Length) return;

        var noteCollection = noteCollections[noteIndex];

        if (timeElapsed >= noteCollection.time + firstNoteTime)
        {
            noteIndex++;
            timeElapsed += delta;
        }
        else
        {
            timeElapsed += delta;
            return;
        }

        foreach (var note in noteCollection.ToArray())
            if (note.slidePaths.Count > 0)
            {
                var slide = new Slide(this, note, note.slidePaths);

                foreach (var node in slide.SlideNodeList.SelectMany(pathNodeList => pathNodeList)) AddChild(node);

                async Task Func()
                {
                    await ToSignal(GetTree().CreateTimer(.5), "timeout");

                    if (slideNoise != null)
                    {
                        var slideSoundPlayer = CreateStreamPlayer(slideNoise);
                        slideSoundPlayer.Finished += () => slideSoundPlayer.QueueFree();
                        AddChild(slideSoundPlayer);
                        slideSoundPlayer.Play();
                    }

                    for (var i = RenderManager.SlideSpacing; i < slide.length; i += RenderManager.SlideSpacing)
                    {
                        slide.SetVisible(i / slide.length);
                        await ToSignal(GetTree().CreateTimer(.01), "timeout");
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
                    tween.Finished += () => touch.QueueFree();
                    tween.Play();
                }

                Func();
            }

        if (tapNoise != null)
        {
            var touchSoundPlayer = CreateStreamPlayer(tapNoise);
            touchSoundPlayer.Finished += () => touchSoundPlayer.QueueFree();
            AddChild(touchSoundPlayer);
            touchSoundPlayer.Play();
        }
    }
}