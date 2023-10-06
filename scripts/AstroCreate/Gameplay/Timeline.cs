using System.Collections.Generic;
using Godot;

namespace AstroCreate.Gameplay;

public partial class Timeline : Node
{
    public readonly List<ObjectBehaviour.ObjectBehaviour> Behaviours = new();

    private bool _paused;

    public float _time;

    public AudioStreamPlayer? audioStreamPlayer;

    public float FirstNote;

    public bool Paused
    {
        get => !audioStreamPlayer?.Playing ?? _paused;
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
        get => audioStreamPlayer?.GetPlaybackPosition() ?? _time;
        set
        {
            if (audioStreamPlayer != null)
                audioStreamPlayer.Seek(value);
            else
                _time = value;
        }
    }

    public void AddBehaviour(ObjectBehaviour.ObjectBehaviour behaviour)
    {
        Behaviours.Add(behaviour);
    }

    public override void _Process(double delta)
    {
        if (Paused) return;

        foreach (var behaviour in Behaviours) behaviour.Update(Time - FirstNote);

        if (audioStreamPlayer == null) _time += (float)delta;
        else _time = audioStreamPlayer.GetPlaybackPosition();
    }
}