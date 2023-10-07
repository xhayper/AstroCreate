using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

#nullable enable

namespace AstroCreate.Gameplay
{
    public class Timeline : MonoBehaviour
    {
        public float _time;
        
        [SerializeField] public AudioSource? audioStreamPlayer;

        public readonly List<ObjectBehaviour.ObjectBehaviour> Behaviours = new();

        private bool _paused;

        [NonSerialized] public float firstNote;

        public static Timeline Instance => _instance;
        private static Timeline _instance;

        public bool Paused
        {
            get => !audioStreamPlayer?.isPlaying ?? _paused;
            set
            {
                if (audioStreamPlayer != null)
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
            get => audioStreamPlayer?.time ?? _time;
            set
            {
                if (audioStreamPlayer != null)
                    audioStreamPlayer.time = value;
                else
                    _time = value;
            }
        }

        public void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (_instance != null) return;

            _instance = this;
            DontDestroyOnLoad(this);
        }

        public void Update()
        {
            if (Paused) return;

            var delta = UnityEngine.Time.deltaTime;

            foreach (var behaviour in Behaviours) behaviour.Update(Time - firstNote);

            if (audioStreamPlayer == null) _time += delta;
            else _time = audioStreamPlayer.time;
        }

        public void AddBehaviour(ObjectBehaviour.ObjectBehaviour behaviour)
        {
            Behaviours.Add(behaviour);
        }
    }
}