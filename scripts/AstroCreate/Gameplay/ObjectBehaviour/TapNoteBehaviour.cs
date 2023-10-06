using AstroCreate.Gameplay.Object;
using AstroCreate.Utilities;
using Godot;

namespace AstroCreate.Gameplay.ObjectBehaviour;

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
        modifiedPosition.X += gridPosition.X;
        modifiedPosition.Y += -gridPosition.Y;

        // TODO: PLEASE DO NOT MULTIPLY DELTA BY X
        var dt = Mathf.Clamp(t - obj.collection.time, -0.01f, 1.01f);
        obj.node.Position = (new Vector2(1280, 720) / 2).Lerp(modifiedPosition, dt);
        obj.node.Visible = dt is > 0 and <= 1;
    }
}