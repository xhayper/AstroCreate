using System.Linq;
using AstroCreate.Gameplay;
using AstroCreate.Gameplay.Object;
using AstroCreate.Gameplay.ObjectBehaviour;
using AstroCreate.Utilities;
using Godot;
using SimaiSharp;
using SimaiSharp.Structures;

namespace AstroCreate.Tests;

public partial class GameplayTest : Node
{
    public AudioStreamPlayer? bgMusicPlayer;

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

    private Node2D tapNotePrefab = ResourceLoader.Load<PackedScene>("res://prefabs/note/tap.tscn")
        .Instantiate() as Node2D;

    public double timeElapsed;

    public Timeline timeline;

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
        timeline = GetNode<Timeline>("/root/Timeline");
        // bgVideoPlayer = GetNode<VideoStreamPlayer>("Node2D/VideoStreamPlayer");

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
        timeline.FirstNote = firstNoteTime;

        if (
            ResourceLoader.Exists($"res://charts/{CHART_NAME}/track.mp3"))
        {
            var musicSoundPlayer = CreateStreamPlayer($"res://charts/{CHART_NAME}/track.mp3");
            AddChild(musicSoundPlayer);
            musicSoundPlayer.Play();
            timeline.audioStreamPlayer = musicSoundPlayer;
            bgMusicPlayer = musicSoundPlayer;
        }

        // if (ResourceLoader.Exists($"res://charts/{CHART_NAME}/pv.ogv"))
        // {
        //     var videoStreamPlayer = ResourceLoader.Load<VideoStream>($"res://charts/{CHART_NAME}/pv.ogv");
        //     bgVideoPlayer.Stream = videoStreamPlayer;
        //     bgVideoPlayer.Play();
        // }

        // TODO: Properly create objects when needed, not creating em all ato nce

        foreach (var noteCollection in chart.NoteCollections)
        foreach (var note in noteCollection)
            if (note.slidePaths.Count > 0)
            {
                var slide = new Slide(this, note, note.slidePaths);

                foreach (var node in slide.SlideNodeList.SelectMany(pathNodeList => pathNodeList)) AddChild(node);

                var slideObject = new SlideObject
                {
                    collection = noteCollection,
                    slide = slide
                };

                var slideBehaviour = new SlideBehaviour(slideObject);
                timeline.AddBehaviour(slideBehaviour);
            }
            else if (note.type is NoteType.Tap or NoteType.Break && note.length == null)
            {
                var tapNote = tapNotePrefab.Duplicate() as Node2D;
                tapNote.Position = GetViewport().GetVisibleRect().Size / 2;
                tapNote.Rotation = -NoteUtility.GetRotation(note.location);
                tapNote.Visible = false;
                tapNote.SetMeta("IsBreak", Variant.From(note.type == NoteType.Break));
                AddChild(tapNote);

                var noteObject = new TapNoteObject
                {
                    collection = noteCollection,
                    node = tapNote,
                    note = note
                };

                var noteBehaviour = new TapNoteBehaviour(noteObject);
                timeline.AddBehaviour(noteBehaviour);

                break;
            }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (noteIndex >= noteCollections.Length) return;

        var noteCollection = noteCollections[noteIndex];

        if (timeline.Time >= noteCollection.time + firstNoteTime) noteIndex++;

        foreach (var note in noteCollection.ToArray())
            if (note.type is NoteType.Tap or NoteType.Break && note.length == null)
                GD.Print($"{note.type} | {note.location.group}{note.location.index}");

        if (tapNoise == null) return;

        var touchSoundPlayer = CreateStreamPlayer(tapNoise);
        touchSoundPlayer.Finished += () => touchSoundPlayer.QueueFree();
        AddChild(touchSoundPlayer);
        touchSoundPlayer.Play();
    }
}