using Godot;
using System;

public class Node2D : Godot.Node2D
{
    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here

    }

    void ClickedChar(String data)
    {
        GD.Print("Clicked " + data);
        //scene_path_to_load = scene_to_load;
    }

    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
}
