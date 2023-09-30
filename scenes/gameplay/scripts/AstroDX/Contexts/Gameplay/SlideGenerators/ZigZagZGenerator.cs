﻿using System.Collections.Generic;
using AstroDX.Contexts.Gameplay.PlayerScope;
using AstroDX.Utilities;
using Godot;
using SimaiSharp.Structures;

namespace AstroDX.Contexts.Gameplay.SlideGenerators;

public sealed class ZigZagZGenerator : SlideGenerator
{
    private readonly Vector2 _endPoint;
    private readonly float _endRotation;
    private readonly Vector2 _endZagPoint;
    private readonly float _midRotation;
    private readonly float _midSegmentLength;
    private readonly Vector2 _startPoint;

    private readonly float _startRotation;

    private readonly float _startSegmentLength;
    private readonly Vector2 _startZagPoint;
    private readonly float _totalLength;

    public ZigZagZGenerator(IReadOnlyList<Location> vertices)
    {
        const float distance = RenderManager.PlayFieldRadius;
        const float inner = RenderManager.CenterRadius;

        var startRotation = GetRotation(vertices[0]);
        var endRotation = GetRotation(vertices[1]);

        var startZag = startRotation - Trigonometry.Tau / 4f;
        var endZag = endRotation - Trigonometry.Tau / 4f;

        _startPoint = new Vector2(distance * Mathf.Cos(startRotation),
            distance * Mathf.Sin(startRotation));

        _startZagPoint = new Vector2(inner * Mathf.Cos(startZag),
            inner * Mathf.Sin(startZag));

        var startSegment = _startZagPoint - _startPoint;
        _startSegmentLength = startSegment.Length();
        _startRotation = Mathf.Atan2(startSegment.Y, startSegment.X);

        _endZagPoint = new Vector2(inner * Mathf.Cos(endZag),
            inner * Mathf.Sin(endZag));

        var midSegment = _endZagPoint - _startZagPoint;
        _midSegmentLength = midSegment.Length();
        _midRotation = Mathf.Atan2(midSegment.Y, midSegment.X);

        _endPoint = new Vector2(distance * Mathf.Cos(endRotation),
            distance * Mathf.Sin(endRotation));

        var endSegment = _endPoint - _endZagPoint;
        var endSegmentLength = endSegment.Length();
        _endRotation = Mathf.Atan2(endSegment.Y, endSegment.X);

        _totalLength = _startSegmentLength + _midSegmentLength + endSegmentLength;
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
            // Vector2.Lerp(_startPoint, _startZagPoint,
            position = _startPoint.Lerp(_startZagPoint,
                Mathf.InverseLerp(0, _startSegmentLength, distanceFromStart));
            rotation = _startRotation;
        }
        else if (distanceFromStart < _startSegmentLength + _midSegmentLength)
        {
            var midLength = _startSegmentLength + _midSegmentLength;

            // Vector2.Lerp(_startZagPoint, _endZagPoint,
            position = _startZagPoint.Lerp(_endZagPoint,
                Mathf.InverseLerp(_startSegmentLength, midLength, distanceFromStart));
            rotation = _midRotation;
        }
        else
        {
            var midLength = _startSegmentLength + _midSegmentLength;

            // Vector2.Lerp(_endZagPoint, _endPoint,
            position = _endZagPoint.Lerp(_endPoint,
                Mathf.InverseLerp(midLength, _totalLength, distanceFromStart));
            rotation = _endRotation;
        }
    }
}