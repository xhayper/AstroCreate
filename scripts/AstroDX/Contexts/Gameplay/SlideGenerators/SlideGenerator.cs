using AstroCreate.Utilities;
using Godot;
using SimaiSharp.Structures;

namespace AstroDX.Contexts.Gameplay.SlideGenerators;

public abstract class SlideGenerator
{
    public abstract float GetLength();

    public abstract void GetPoint(float t, out Vector2 position, out float rotation);

    public static Vector2 GetPosition(in Location location)
    {
        return NoteUtility.GetPosition(in location);
    }
}