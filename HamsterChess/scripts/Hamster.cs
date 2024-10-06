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
    private PackedScene? _handScene;
    private PackedScene? _blueCircleScene;
    
    private GameMaster? _gameMaster;
    
    private VBoxContainer? _scrollable;
    
    private double _uiRefreshTimer;
    private double _uiRefreshTimerMax = 0.25;
    
    public override void _Ready()
    {
        var boardSquares = GetNode<Node2D>("Board").GetChildren();
        _whiteKingScene = GD.Load<PackedScene>("res://scenes/piece_wk.tscn");
        _whiteRookScene = GD.Load<PackedScene>("res://scenes/piece_wr.tscn");
        _blackKingScene = GD.Load<PackedScene>("res://scenes/piece_bk.tscn");
        _handScene = GD.Load<PackedScene>("res://scenes/hand.tscn");
        _blueCircleScene = GD.Load<PackedScene>("res://scenes/blue_circle.tscn");
        
        Ready2(boardSquares);
        
        _gameMaster = new GameMaster();
        
        _scrollable = GetNode<VBoxContainer>("PanelContainer/HBoxContainer/ScrollContainer/VBoxContainer");
        
        for (var row = 0; row < 8; row++)
        {
            for (var column = 0; column < 8; column++)
            {
                var square = _gameMaster.PBoard.PBoard[row][column];
                
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

        ClearScrollable();
        
        var moveButtonScene = GD.Load<PackedScene>("res://scenes/move_label.tscn");
        
        var dict = _gameMaster.PLegalMoves.PLegalMoves;
        foreach (var kv in dict)
        {
            foreach (var value in kv.Value)
            {
                GD.Print($"__{kv.Key}__{value}");
                
                var moveButton = moveButtonScene.Instantiate<Button>();
                moveButton.Text = $"{kv.Key.ToShortString()}-{value.ToShortString()}";
                moveButton.ButtonDown += () =>
                {
                    GD.Print(moveButton.Text);
                    
                    ClearFromToIndicators();

                    var newHand = _handScene.Instantiate<Node2D>();
                    var newHandPosition = SquareToPosition(kv.Key);
                    newHand.Position = newHandPosition;
                    AddChild(newHand);
                };
                _scrollable.AddChild(moveButton);
            }
        }
    }

    private void ClearScrollable()
    {
        foreach (var node in _scrollable!.GetChildren())
        {
            _scrollable.RemoveChild(node);
            node.QueueFree();
        }
    }

    private void ClearFromToIndicators()
    {
        //
    }

    private Vector2 SquareToPosition(Square square)
    {
        var letterIndex = LetterNumber.GetLetterIndex(square.Letter);
        var letterPosition = 50 * letterIndex + 25;
        var numberIndex = LetterNumber.GetNumberIndexReverse(square.Number);
        var numberPosition = 50 * numberIndex + 50;
        
        return new Vector2(letterPosition, numberPosition);
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
        _uiRefreshTimer += delta;

        if (_uiRefreshTimer > _uiRefreshTimerMax)
        {
            var labelParent = GetNode<VBoxContainer>("PanelContainer/HBoxContainer/VBoxContainer");
            labelParent.GetNode<RichTextLabel>("LabelWhiteToMove").Text = $"{"White to move:",-23} {_gameMaster!.WhiteToMove.ToString()}";
            labelParent.GetNode<RichTextLabel>("LabelWKCheck").Text = $"{"White in check:",-24} {_gameMaster.WhiteKingInCheck.ToString()}";
            labelParent.GetNode<RichTextLabel>("LabelBKCheck").Text = $"{"Black in check:",-25} {_gameMaster.BlackKingInCheck.ToString()}";
            labelParent.GetNode<RichTextLabel>("LabelWKCheckmate").Text = $"{"White checkmated:",-19} {_gameMaster.WhiteKingCheckmated.ToString()}";
            labelParent.GetNode<RichTextLabel>("LabelBKCheckmate").Text = $"{"Black checkmated:",-20} {_gameMaster.BlackKingCheckmated.ToString()}";
            
            _uiRefreshTimer = 0;
        }
    }
}