using Godot;

namespace AstroCreate.Tests;

public partial class TapNoteTest : Node2D
{
    private Sprite2D _tapNote;

    private CompressedTexture2D BREAK_NOTE =
        ResourceLoader.Load<CompressedTexture2D>("res://textures/AstroDX/Notes/IMG_GAME_BREAK_TAP_1.png");

    private CompressedTexture2D NORMAL_NOTE =
        ResourceLoader.Load<CompressedTexture2D>("res://textures/AstroDX/Notes/IMG_GAME_SINGLE_TAP_1.png");

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        _tapNote = GetNode<Sprite2D>("TapNote");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        _tapNote.Texture = GetMeta("IsBreak").AsBool() ? BREAK_NOTE : NORMAL_NOTE;
    }
}