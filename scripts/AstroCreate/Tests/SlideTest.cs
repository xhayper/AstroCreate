using AstroCreate.Utilities;
using AstroDX.Contexts.Gameplay.SlideGenerators;
using Godot;

namespace AstroCreate.Tests;

public partial class SlideTest : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var squarePrefab = ResourceLoader.Load<PackedScene>("res://scenes/test_scene/prefabs/smol_square.tscn")
            .Instantiate() as Node2D;

        for (var x = 0; x < SlideUtility.TAP_LIST.Count; x++)
        {
            for (var y = 0; y < SlideUtility.TAP_LIST.Count; y++)
            {
                GD.Print($"{x} | {y}");

                if (x == y) continue;
                if (x != 0) continue;

                var START_LOCATION = SlideUtility.TAP_LIST[x];
                var END_LOCATION = SlideUtility.TAP_LIST[y];

                var LOCATION_LIST = new[] { START_LOCATION, END_LOCATION };

                var slideGenerator = new StraightGenerator(LOCATION_LIST);

                for (float i = 0; i < 1; i += .1f)
                {
                    Vector2 location;
                    float rotation;

                    slideGenerator.GetPoint(i, out location, out rotation);

                    var square = squarePrefab.Duplicate() as Node2D;
                    // GetViewport().GetVisibleRect().Size
                    square.Position = location * new Vector2(980, 980);
                    square.Rotation = rotation;
                    AddChild(square);

                    GD.Print(
                        $"{START_LOCATION.group} {START_LOCATION.index} => {END_LOCATION.group} {END_LOCATION.index} | Location: {location}, Rotation: {rotation} | t: {i}");
                }
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}