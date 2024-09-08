using Godot;
using System;
using HamsterBusiness.BusinessMain.BusinessUtil;

public partial class Hamster : Node2D
{
    public override void _Ready()
    {
        GD.Print("Hamster; " + Messages.LoremIpsum);
    }

    public override void _Process(double delta)
    {
    }
}