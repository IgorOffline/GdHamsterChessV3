using Godot;
using System;
using HamsterBusiness.BusinessMain.BusinessBoard;
using HamsterBusiness.BusinessMain.BusinessGame;
using HamsterBusiness.BusinessMain.BusinessUtil;

public partial class Hamster : Node2D
{
    private PackedScene? _whiteKingScene;
    private PackedScene? _whiteRookScene;
    private PackedScene? _blackKingScene;
    private PackedScene? _blackRookScene;
    private PackedScene? _handScene;
    private PackedScene? _blueCircleScene;
    
    private GameMaster? _gameMaster;
    
    private VBoxContainer? _scrollable;
    
    private double _uiRefreshTimer;
    private double _uiRefreshTimerMax = 0.25;
    
    private const string HandPrefix = "Hand_";
    private const string BlueCirclePrefix = "BlueCircle_";
    private const string PiecePrefix = "Piece_";
    
    public override void _Ready()
    {
        _gameMaster = new GameMaster();
        
        AddSquareLabels();
        
        AddPieces();

        ClearScrollable();
        
        AddScrollableElements();

        var txtNextMove = GetNode<TextEdit>("PanelContainer/HBoxContainer/VBoxContainer/TxtNextMove");
        var btnNextMove = GetNode<Button>("PanelContainer/HBoxContainer/VBoxContainer/BtnNextMove");
        btnNextMove.ButtonDown += () =>
        {
            if (txtNextMove.Text.Length == 5)
            {
                GD.Print($"txtNextMove.Text:{txtNextMove.Text}");

                var fromRow = -1;
                var fromCol = -1;
                var toRow = -1;
                var toCol = -1;
                
                var split = txtNextMove.Text.Split("-");
                var fromLetterNumberString = split[0];
                var toLetterNumberString = split[1];

                for (var row = 0; row < 8; row++)
                {
                    for (var col = 0; col < 8; col++)
                    {
                        var currentLetterNumber = _gameMaster!.PBoard.PBoard[row][col].ToLetterNumberString();
                        
                        if (fromLetterNumberString.Equals(currentLetterNumber))
                        {
                            fromRow = row;
                            fromCol = col;
                        }
                        else if (toLetterNumberString.Equals(currentLetterNumber))
                        {
                            toRow = row;
                            toCol = col;
                        }
                    }
                }

                if (fromRow != -1 && fromCol != -1 && toRow != -1 && toCol != -1)
                {
                    _gameMaster!.MoveAndCalculate(fromRow, fromCol, toRow, toCol);

                    ClearPieces();
                    
                    ClearScrollable();

                    ClearFromToIndicators();
                    
                    AddPieces();
                    
                    AddScrollableElements();
                    
                    GD.Print("Frontend: MoveAndCalculate Done");
                }
            }
        };
    }

    private void AddPieces()
    {
        var boardSquares = GetNode<Node2D>("Board").GetChildren();
        _whiteKingScene = GD.Load<PackedScene>("res://scenes/piece_wk.tscn");
        _whiteRookScene = GD.Load<PackedScene>("res://scenes/piece_wr.tscn");
        _blackKingScene = GD.Load<PackedScene>("res://scenes/piece_bk.tscn");
        _blackRookScene = GD.Load<PackedScene>("res://scenes/piece_br.tscn");
        _handScene = GD.Load<PackedScene>("res://scenes/hand.tscn");
        _blueCircleScene = GD.Load<PackedScene>("res://scenes/blue_circle.tscn");
        
        _scrollable = GetNode<VBoxContainer>("PanelContainer/HBoxContainer/ScrollContainer/VBoxContainer");
        
        for (var row = 0; row < 8; row++)
        {
            for (var column = 0; column < 8; column++)
            {
                var square = _gameMaster!.PBoard.PBoard[row][column];
                
                if (square.Piece != Piece.None)
                {
                    GD.Print(square);
                    var newPiece = Instantiate(square);
                    newPiece.Name = PiecePrefix + Guid.NewGuid();
                    var squareNode2D = boardSquares[square.GetIndex()] as Node2D;
                    newPiece.Position = squareNode2D!.Position;
                    AddChild(newPiece);
                }
            }
        }
    }

    private void ClearPieces()
    {
        foreach (var node in GetChildren())
        {
            if (node.Name.ToString().StartsWith(PiecePrefix))
            {
                RemoveChild(node);
                node.QueueFree();
            }
        }
    }
    
    private Node2D Instantiate(Square square)
    {
        return square.Piece switch
        {
            Piece.King when square.PieceColor == PieceColor.White => _whiteKingScene!.Instantiate<Node2D>(),
            Piece.King => _blackKingScene!.Instantiate<Node2D>(),
            Piece.Rook when square.PieceColor == PieceColor.White => _whiteRookScene!.Instantiate<Node2D>(),
            Piece.Rook => _blackRookScene!.Instantiate<Node2D>(),
            _ => throw new ArgumentException("Invalid square")
        };
    }
    
    private void AddSquareLabels()
    {
        GD.Print("Hamster; " + Messages.LoremIpsum);
        
        var boardSquares = GetNode<Node2D>("Board").GetChildren();
        
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

    private void AddScrollableElements()
    {
        var moveButtonScene = GD.Load<PackedScene>("res://scenes/move_label.tscn");
        
        var dict = _gameMaster!.PLegalMoves.PLegalMoves;
        foreach (var key in dict.Keys)
        {
            var moveButton = moveButtonScene.Instantiate<Button>();
            moveButton.Text = key.ToShortString();
            moveButton.ButtonDown += () =>
            {
                GD.Print(moveButton.Text);
                    
                ClearFromToIndicators();

                var newHand = _handScene!.Instantiate<Node2D>();
                newHand.Name = HandPrefix + Guid.NewGuid();
                var newHandPosition = SquareToPosition(key, true);
                newHand.Position = newHandPosition;
                AddChild(newHand);

                var values = dict[key];
                foreach (var value in values)
                {
                    var newBlueCircle = _blueCircleScene!.Instantiate<Node2D>();
                    newBlueCircle.Name = BlueCirclePrefix + Guid.NewGuid();
                    var newBlueCirclePosition = SquareToPosition(value, false);
                    newBlueCircle.Position = newBlueCirclePosition;
                    AddChild(newBlueCircle);
                }
            };
            _scrollable!.AddChild(moveButton);
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
            if (child.Name.ToString().StartsWith(HandPrefix) || child.Name.ToString().StartsWith(BlueCirclePrefix))
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