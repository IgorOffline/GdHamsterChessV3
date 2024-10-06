using Godot;
using System;
using Godot.Collections;
using HamsterBusiness.BusinessMain.BusinessBoard;
using HamsterBusiness.BusinessMain.BusinessGame;
using HamsterBusiness.BusinessMain.BusinessUtil;

public partial class Hamster : Node2D
{
    private PackedScene? _whiteKingScene;
    private PackedScene? _whiteRookScene;
    private PackedScene? _blackKingScene;
    
    public override void _Ready()
    {
        var boardSquares = GetNode<Node2D>("Board").GetChildren();
        _whiteKingScene = GD.Load<PackedScene>("res://scenes/piece_wk.tscn");
        _whiteRookScene = GD.Load<PackedScene>("res://scenes/piece_wr.tscn");
        _blackKingScene = GD.Load<PackedScene>("res://scenes/piece_bk.tscn");
        
        Ready2(boardSquares);
        
        var gameMaster = new GameMaster();
        
        for (var row = 0; row < 8; row++)
        {
            for (var column = 0; column < 8; column++)
            {
                var square = gameMaster.PBoard.PBoard[row][column];
                
                if (square.Piece != Piece.None)
                {
                    GD.Print(square);
                    var newPiece = Instantiate(square);
                    var squareNode2D = boardSquares[square.GetIndex()] as Node2D;
                    newPiece.Position = squareNode2D!.Position;
                    AddChild(newPiece);
                }
            }
        }

        var labelParent = GetNode<VBoxContainer>("PanelContainer/VBoxContainer");
        labelParent.GetNode<RichTextLabel>("LabelWhiteToMove").Text = $"{("White to move:"),-23} {gameMaster.WhiteToMove.ToString()}";
        labelParent.GetNode<RichTextLabel>("LabelWKCheck").Text = $"{"White in check:",-24} {gameMaster.WhiteKingInCheck.ToString()}";
        labelParent.GetNode<RichTextLabel>("LabelBKCheck").Text = $"{"Black in check:",-25} {gameMaster.BlackKingInCheck.ToString()}";
        labelParent.GetNode<RichTextLabel>("LabelWKCheckmate").Text = $"{"White checkmated:",-19} {gameMaster.WhiteKingCheckmated.ToString()}";
        labelParent.GetNode<RichTextLabel>("LabelBKCheckmate").Text = $"{"Black checkmated:",-20} {gameMaster.BlackKingCheckmated.ToString()}";
    }

    private Node2D Instantiate(Square square)
    {
        if (square.Piece == Piece.King)
        {
            if (square.PieceColor == PieceColor.White)
            {
                return _whiteKingScene!.Instantiate<Node2D>();
            }
            
            return _blackKingScene!.Instantiate<Node2D>();
        }

        if (square.Piece == Piece.Rook)
        {
            return _whiteRookScene!.Instantiate<Node2D>();
        }

        throw new ArgumentException("Invalid square");
    }

    private void Ready2(Array<Node> boardSquares)
    {
        GD.Print("Hamster; " + Messages.LoremIpsum);

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