using Godot;
using System;
using Project.Services;

public class Hire1 : Button
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
                gs.HireCharacter(1);
                //do something
                GetTree().ReloadCurrentScene();
                //GD.Print("Yo yo");
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
