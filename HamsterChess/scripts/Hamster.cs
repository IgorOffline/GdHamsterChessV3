using Godot;
using System;

public partial class Hamster : Node2D
{
    public override void _Ready()
    {
        GD.Print("Hamster");
    }

    public override void _Process(double delta)
    {
    }
}