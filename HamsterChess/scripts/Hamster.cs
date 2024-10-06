using Godot;
using System;
using HamsterBusiness.BusinessMain.BusinessUtil;

public partial class Hamster : Node2D
{
    public override void _Ready()
    {
        GD.Print("Hamster; " + Messages.LoremIpsum);

        var boardSquares = GetNode<Node2D>("Board").GetChildren();
        //GD.Print($"boardSquares.Count= {boardSquares.Count}");
        //foreach (var square in boardSquares)
        //{
        //    GD.Print(square.Name.ToString());
        //}
        
        var squareLabelScene = ResourceLoader.Load<PackedScene>("scenes/square_label.tscn");
        foreach (var square in boardSquares)
        {
            var square2D = square as Node2D;
            var squareLabel = squareLabelScene.Instantiate<Node2D>();
            squareLabel.GetNode<RichTextLabel>("Label").Text = $"[color=black]{square.GetName()}[/color]";
            squareLabel.Position = new Vector2(square2D!.Position.X - 27, square2D.Position.Y + 7);
            AddChild(squareLabel);
        }
    }

    public override void _Process(double delta)
    {
    }
}