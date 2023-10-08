using AstroCreate.Gameplay.Object;
using AstroCreate.Utilities;
using UnityEngine;

namespace AstroCreate.Gameplay.ObjectBehaviour
{
    public class TapNoteBehaviour : ObjectBehaviour
    {
        private readonly TapNoteObject obj;

        public TapNoteBehaviour(TapNoteObject obj)
        {
            this.obj = obj;
        }

        // TODO: Find a way to adjust slide speed
        public override void Update(float t)
        {
            var dt = Mathf.Clamp(t - obj.collection.time, -0.01f, 1.01f);
            obj.obj.transform.position = Vector2.Lerp(Vector2.zero, NoteUtility.GetPosition(obj.note.location), dt);
            obj.obj.SetActive(dt is > 0 and <= 1);
        }
    }
}