using System.Collections.Generic;
using Godot;
using SimaiSharp.Structures;

namespace AstroDX.Contexts.Gameplay.SlideGenerators;

public sealed class StraightGenerator : SlideGenerator
{
    private readonly Vector2 _endPoint;
    private readonly float _length;
    private readonly float _rotation;
    private readonly Vector2 _startPoint;

    public StraightGenerator(IReadOnlyList<Location> vertices)
    {
        _startPoint = GetPosition(vertices[0]);
        _endPoint = GetPosition(vertices[1]);

        var segment = _endPoint - _startPoint;
        _length = segment.Length();
        _rotation = Mathf.Atan2(segment.Y, segment.X);
    }

    public override float GetLength()
    {
        return _length;
    }

    public override void GetPoint(float t, out Vector2 position, out float rotation)
    {
        // Vector2.Lerp(_startPoint, _endPoint, t);
        position = _startPoint.Lerp(_endPoint, t);
        rotation = _rotation;
    }
}