#nullable enable

using System.Linq;
using AstroCreate.Gameplay;
using AstroCreate.Gameplay.Object;
using AstroCreate.Gameplay.ObjectBehaviour;
using AstroCreate.Utilities;
using SimaiSharp;
using SimaiSharp.Structures;
using UnityEngine;

namespace AstroCreate.Tests
{
    public class GameplayTest : MonoBehaviour
    {
        public MaiChart chart;
        public float firstNoteTime;
        public NoteCollection[] noteCollections;

        // private AudioStream? slideNoise = ResourceLoader.Exists("res://sounds/slide.wav")
        //     ? ResourceLoader.Load<AudioStream>("res://sounds/slide.wav")
        //     : null;
        //
        // private AudioStream? tapNoise = ResourceLoader.Exists("res://sounds/slide.wav")
        //     ? ResourceLoader.Load<AudioStream>("res://sounds/tap.wav")
        //     : null;

        public GameObject tapNotePrefab;
        public GameObject? noteFolder;

        private int noteIndex;
        public static GameplayTest Instance { get; private set; }

        public void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            var CHART_NAME = "超熊猫的周遊記（ワンタ\u3099ーハ\u309aンタ\u3099ートラヘ\u3099ラー）";
            var CHART_DIFF = "5";

            var chartFile = Resources.Load<TextAsset>($"Charts/{CHART_NAME}/maidata");

            LoadChart(chartFile.text, CHART_DIFF);
        }

        public void Update()
        {
            if (noteIndex >= noteCollections.Length) return;

            var noteCollection = noteCollections[noteIndex];

            if (Timeline.Instance.Time >= noteCollection.time + firstNoteTime)
                noteIndex++;
            else
                return;

            foreach (var note in noteCollection.ToArray())
                if (note.slidePaths.Count > 0)
                    foreach (var path in note.slidePaths)
                    foreach (var segment in path.segments)
                    foreach (var verticies in segment.vertices)
                        Debug.Log(
                            $"Slide {segment.slideType} | {path.startLocation.group}{path.startLocation.index} => {verticies.group}{verticies.index}");
                else if (note.type is NoteType.Tap or NoteType.Break && note.length == null)
                    Debug.Log($"{note.type} | {note.location.group}{note.location.index}");
        }

        public void LoadChart(string chartText, string chartDiff)
        {
            noteIndex = 0;

            var timeline = Timeline.Instance;

            timeline.Time = 0f;
            timeline.DestroyBehaviours();

            var chartDataStream = TextUtility.GenerateStreamFromString(chartText);

            var file = new SimaiFile(chartDataStream);
            var firstTime = file.GetValue("first");

            chart = SimaiConvert.Deserialize(file.GetValue($"inote_{chartDiff}"));
            noteCollections = chart.NoteCollections.ToArray();

            if (!float.TryParse(firstTime, out firstNoteTime)) firstNoteTime = noteCollections[0].time;
            timeline.firstNote = firstNoteTime;

            // if (
            //     ResourceLoader.Exists($"res://charts/{CHART_NAME}/track.mp3"))
            // {
            //     var musicSoundPlayer = CreateStreamPlayer($"res://charts/{CHART_NAME}/track.mp3");
            //     AddChild(musicSoundPlayer);
            //     musicSoundPlayer.Play();
            //     timeline.audioStreamPlayer = musicSoundPlayer;
            //     bgMusicPlayer = musicSoundPlayer;
            // }

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
                    var slide = new Slide(note, noteFolder);

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
                    var tapNote = Instantiate(tapNotePrefab, noteFolder?.transform);
                    tapNote.name = $"Touch (T{note.location.index + 1})";
                    tapNote.transform.position = Vector2.zero;
                    tapNote.transform.Rotate(new Vector3(0, 0, NoteUtility.GetRotation(note.location)));
                    tapNote.SetActive(false);

                    var noteObject = new TapNoteObject
                    {
                        collection = noteCollection,
                        obj = tapNote,
                        note = note
                    };

                    var noteBehaviour = new TapNoteBehaviour(noteObject);
                    timeline.AddBehaviour(noteBehaviour);

                    break;
                }
        }
    }
}