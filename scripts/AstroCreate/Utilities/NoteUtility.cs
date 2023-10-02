using System;
using AstroDX.Contexts.Gameplay.PlayerScope;
using AstroDX.Utilities;
using Godot;
using SimaiSharp.Structures;

namespace AstroCreate.Utilities;

public static class NoteUtility
{
    private static float GetRotation(in Location location)
    {
        const float initialAngle = Trigonometry.Tau / 4f - Trigonometry.Tau / 16f;

        var angle = initialAngle - Trigonometry.Tau / 8f * location.index;

        if (location.group is NoteGroup.DSensor or NoteGroup.ESensor) angle += Trigonometry.Tau / 16f;

        return angle;
    }

    public static Vector2 GetPosition(in Location location)
    {
        var radius = GetRadiusFromCenter(location);

        return GetPositionRadial(GetRotation(location), radius);
    }

    private static Vector2 GetPositionRadial(in float rotationRadians,
        in float radius = RenderManager.PlayFieldRadius)
    {
        return new Vector2(Mathf.Cos(rotationRadians) * radius,
            Mathf.Sin(rotationRadians) * radius);
    }

    private static float GetRadiusFromCenter(Location location)
    {
        return location.group switch
        {
            NoteGroup.Tap => RenderManager.PlayFieldRadius,
            NoteGroup.ASensor => RenderManager.AreaARadius,
            NoteGroup.BSensor => RenderManager.AreaBRadius,
            NoteGroup.CSensor => 0,
            NoteGroup.DSensor => RenderManager.AreaDRadius,
            NoteGroup.ESensor => RenderManager.AreaERadius,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}