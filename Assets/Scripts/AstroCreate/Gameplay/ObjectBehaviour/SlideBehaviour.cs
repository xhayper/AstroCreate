using AstroCreate.Gameplay.Object;
using UnityEngine;

namespace AstroCreate.Gameplay.ObjectBehaviour
{
    public class SlideBehaviour : ObjectBehaviour
    {
        private readonly SlideObject obj;

        public SlideBehaviour(SlideObject obj)
        {
            this.obj = obj;
        }


        // TODO: Find a way to adjust slide speed
        public override void Update(float t)
        {
            var dt = Mathf.Clamp(t - obj.collection.time, -0.01f, 1.01f);
            if (0 > dt) obj.slide.SetVisible(1);
            else obj.slide.SetVisible(dt);
        }
    }
}