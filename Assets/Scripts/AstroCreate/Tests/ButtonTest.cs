#nullable enable

using System.IO;
using AstroCreate.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace AstroCreate.Tests
{
    public class ButtonTest : MonoBehaviour
    {
        public void Awake()
        {
            var button = GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                Timeline.Instance.Paused = true;
                NativeFilePicker.PickFile(FileSelected, ".txt");
            });
        }

        private void FileSelected(string? path)
        {
            Timeline.Instance.Paused = false;

            if (path == null)
                return;

            Timeline.Instance.audioStreamPlayer.clip = null;
            Timeline.Instance.Paused = false;
            Timeline.Instance.Time = 0;

            GameplayTest.Instance.LoadChart(File.ReadAllText(path), "6");
        }
    }
}