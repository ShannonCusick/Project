using Godot;
using System;
using Project.Domain;
using Project.Services;

public class NewGame : TextureRect
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here

    }

    public override void _GuiInput(InputEvent @event)
    {
        var mouseButtonEvent = @event as InputEventMouseButton;
        if (mouseButtonEvent != null)
        {
            if (mouseButtonEvent.ButtonIndex == (int)ButtonList.Left && mouseButtonEvent.Pressed)
            {
                GameService gs = new GameService();
                if (gs.NewGame()==true)
                {
                    GetTree().ChangeScene("res://NewGame.tscn");
                }
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
