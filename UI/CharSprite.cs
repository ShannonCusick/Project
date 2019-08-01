using Godot;
using System;
using Project.Services;

public class CharSprite : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here

    }
    void _InputEvent(Viewport viewport, InputEvent @event, int shape_idx)
    {

        var mouseButtonEvent = @event as InputEventMouseButton;
        if (mouseButtonEvent != null)
        {
            if (mouseButtonEvent.ButtonIndex == (int)ButtonList.Left && mouseButtonEvent.Pressed)
            {
                GD.Print("Clicked "+shape_idx);
            }
        }
    }

    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
}
