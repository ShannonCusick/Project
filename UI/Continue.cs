using Godot;
using System;
using Project.Services;
using Project.Domain;

public class Continue : TextureRect
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        
    }

    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
    public override void _GuiInput(InputEvent @event)
    {
        var mouseButtonEvent = @event as InputEventMouseButton;
        if (mouseButtonEvent != null)
        {
            if (mouseButtonEvent.ButtonIndex == (int)ButtonList.Left && mouseButtonEvent.Pressed)
            {
                GameService GameService = new GameService();
                GameState gs = GameService.LoadGame();
                if (gs.ship.Characters.Count > 0)
                {
                    GetTree().ChangeScene("res://MainGame.tscn");
                }else
                {

                }
            }
        }
    }
}
