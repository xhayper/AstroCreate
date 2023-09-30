using System.Collections.Generic;
using Godot;
using SimaiSharp.Structures;

namespace AstroDX.Contexts.Gameplay.SlideGenerators;

public sealed class EdgeFoldGenerator : SlideGenerator
{
    private readonly Vector2 _endPoint;
    private readonly float _endRotation;
    private readonly Vector2 _midPoint;
    private readonly Vector2 _startPoint;

    private readonly float _startRotation;

    private readonly float _startSegmentLength;
    private readonly float _totalLength;

    public EdgeFoldGenerator(IReadOnlyList<Location> vertices)
    {
        _startPoint = GetPosition(vertices[0]);
        _midPoint = GetPosition(vertices[1]);
        _endPoint = GetPosition(vertices[2]);

        var startSegment = _midPoint - _startPoint;
        _startSegmentLength = startSegment.Length();
        _startRotation = Mathf.Atan2(startSegment.Y, startSegment.X);

        var endSegment = _endPoint - _midPoint;
        var endSegmentSpan = endSegment.Length();
        _endRotation = Mathf.Atan2(endSegment.Y, endSegment.X);

        _totalLength = _startSegmentLength + endSegmentSpan;
    }

    public override float GetLength()
    {
        return _totalLength;
    }

    public override void GetPoint(float t, out Vector2 position, out float rotation)
    {
        var distanceFromStart = t * _totalLength;

        if (distanceFromStart < _startSegmentLength)
        {
            // Vector2.Lerp(_startPoint, _midPoint,
            position = _startPoint.Lerp(_midPoint,
                Mathf.InverseLerp(0,
                    _startSegmentLength / _totalLength,
                    t));
            rotation = _startRotation;
        }
        else
        {
            // Vector2.Lerp(_midPoint, _endPoint,
            position = _midPoint.Lerp(_endPoint,
                Mathf.InverseLerp(_startSegmentLength / _totalLength,
                    1,
                    t));
            rotation = _endRotation;
        }
    }
}