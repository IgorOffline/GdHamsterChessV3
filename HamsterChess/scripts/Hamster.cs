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
        PrepareInit();

        ClearScrollable();
        
        PrepareScrollable();

        var txtNextMove = GetNode<TextEdit>("PanelContainer/HBoxContainer/VBoxContainer/TxtNextMove");
        var btnNextMove = GetNode<Button>("PanelContainer/HBoxContainer/VBoxContainer/BtnNextMove");
        btnNextMove.ButtonDown += () =>
        {
            GD.Print($"txtNextMove.Text:{txtNextMove.Text}");
        };
    }

    private void PrepareInit()
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

    private void PrepareScrollable()
    {
        var moveButtonScene = GD.Load<PackedScene>("res://scenes/move_label.tscn");
        
        var dict = _gameMaster.PLegalMoves.PLegalMoves;
        foreach (var key in dict.Keys)
        {
            var moveButton = moveButtonScene.Instantiate<Button>();
            moveButton.Text = key.ToShortString();
            moveButton.ButtonDown += () =>
            {
                GD.Print(moveButton.Text);
                    
                ClearFromToIndicators();

                var newHand = _handScene.Instantiate<Node2D>();
                newHand.Name = "Hand_" + Guid.NewGuid();
                var newHandPosition = SquareToPosition(key, true);
                newHand.Position = newHandPosition;
                AddChild(newHand);

                var values = dict[key];
                foreach (var value in values)
                {
                    var newBlueCircle = _blueCircleScene.Instantiate<Node2D>();
                    newBlueCircle.Name = "BlueCircle_" + Guid.NewGuid();
                    var newBlueCirclePosition = SquareToPosition(value, false);
                    newBlueCircle.Position = newBlueCirclePosition;
                    AddChild(newBlueCircle);
                }
            };
            _scrollable.AddChild(moveButton);
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
        var children = GetChildren();
        foreach (var child in children)
        {
            if (child.Name.ToString().Contains("Hand") || child.Name.ToString().Contains("BlueCircle"))
            {
                RemoveChild(child);
                child.QueueFree();
            }
        }
    }

    private Vector2 SquareToPosition(Square square, bool isHand)
    {
        var letterIndex = LetterNumber.GetLetterIndex(square.Letter);
        var letterPosition = 50 * letterIndex + (isHand ? 25 : 35);
        var numberIndex = LetterNumber.GetNumberIndexReverse(square.Number);
        var numberPosition = 50 * numberIndex + (isHand ? 50 : 35);
        
        return new Vector2(letterPosition, numberPosition);
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