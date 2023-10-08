#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AstroCreate.Gameplay
{
    public class Timeline : MonoBehaviour
    {
        public float _time;

        [SerializeField] public AudioSource? audioStreamPlayer;

        public readonly List<ObjectBehaviour.ObjectBehaviour> Behaviours = new();

        private bool _paused;

        [NonSerialized] public float firstNote;

        public static Timeline Instance { get; private set; }

        public bool Paused
        {
            get => audioStreamPlayer != null && audioStreamPlayer.clip != null ? !audioStreamPlayer.isPlaying : _paused;
            set
            {
                if (audioStreamPlayer != null && audioStreamPlayer.clip != null)
                {
                    if (value)
                        audioStreamPlayer.Stop();
                    else
                        audioStreamPlayer.Play();
                }
                else
                {
                    _paused = value;
                }
            }
        }

        public float Time
        {
            get => audioStreamPlayer != null && audioStreamPlayer.clip != null ? audioStreamPlayer.time : _time;
            set
            {
                if (audioStreamPlayer != null && audioStreamPlayer.clip != null)
                    audioStreamPlayer.time = value;
                else
                    _time = value;
            }
        }

        public void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (Instance != null) return;

            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void Update()
        {
            if (Paused) return;

            foreach (var behaviour in Behaviours) behaviour.Update(Time - firstNote);

            if (audioStreamPlayer == null || audioStreamPlayer.clip == null) _time += UnityEngine.Time.deltaTime;
            else _time = audioStreamPlayer.time;
        }

        public void AddBehaviour(ObjectBehaviour.ObjectBehaviour behaviour)
        {
            Behaviours.Add(behaviour);
        }

        public void DestroyBehaviours()
        {
            foreach (var behaviour in Behaviours) behaviour.Destroy();
            Behaviours.Clear();
        }
    }
}