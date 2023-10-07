using AstroCreate.Utilities;
using AstroDX.Contexts.Gameplay.PlayerScope;
using SimaiSharp.Structures;
using UnityEngine;

namespace AstroDX.Contexts.Gameplay.SlideGenerators
{
    public abstract class SlideGenerator
    {
        public abstract float GetLength();

        public abstract void GetPoint(float t, out Vector2 position, out float rotation);

        protected static float GetRotation(in Location location)
        {
            return NoteUtility.GetRotation(in location);
        }

        public static Vector2 GetPosition(in Location location)
        {
            return NoteUtility.GetPosition(in location);
        }

        protected static Vector2 GetPositionRadial(in float rotationRadians,
            in float radius = RenderManager.PlayFieldRadius)
        {
            return NoteUtility.GetPositionRadial(in rotationRadians, in radius);
        }

        protected static float GetRadiusFromCenter(Location location)
        {
            return NoteUtility.GetRadiusFromCenter(location);
        }
    }
}