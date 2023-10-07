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
            // TODO: Add size into account
            var gridPosition = NoteUtility.GetPosition(obj.note.location) * 50;
            var modifiedPosition = new Vector2(1280, 720) / 2;
            modifiedPosition.x += gridPosition.x;
            modifiedPosition.y += gridPosition.y;
            
            var dt = Mathf.Clamp(t - obj.collection.time, -0.01f, 1.01f);
            obj.obj.transform.position = Vector2.Lerp(new Vector2(1280, 720) / 2, modifiedPosition, dt);
            obj.obj.SetActive(dt is > 0 and <= 1);
        }
    }
}