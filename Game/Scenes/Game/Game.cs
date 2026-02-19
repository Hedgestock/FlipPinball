using Godot;
using Godot.Collections;
using WaffleStock;

public partial class Game : Node
{
    [Export]
    Array<WeightedBoard> Boards;

    [Export]
    Viewport BoardViewport;

    [Export]
    BoxContainer MainContainer;

    [Export]
    InfoBox InfoBox;
    [Export]
    Control Placeholder;
    [Export]
    Container StatusScrollContainer;
    [Export]
    StatusBox StatusBox;

    [Export]
    Container Ballterator;

    public override void _Ready()
    {
        CallDeferred(MethodName.OnScreenResize);
        GetTree().Root.Connect(Viewport.SignalName.SizeChanged, new Callable(this, MethodName.OnScreenResize));

        GameManager.Instance.Connect(GameManager.SignalName.NewBall, new Callable(this, MethodName.ResetBoard));
        GameManager.Instance.Connect(GameManager.SignalName.LevelCleared, new Callable(this, MethodName.OpenBallterator));

        GameManager.SetGame();

        ResetBoard();

        base._Ready();
    }


    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    void OpenBallterator()
    {
        GetTree().Paused = true;
        Ballterator.Show();
    }

    void ResetBoard()
    {
        InfoBox.Reset();
        StatusBox.Reset();
        foreach (var child in BoardViewport.GetChildren())
        {
            child.QueueFree();
        }
        GameManager.CurrentBoard = WeightedBoard.ChooseFrom(Boards).Instantiate<Board>();
        BoardViewport.CallDeferred(Node.MethodName.AddChild, GameManager.CurrentBoard);
    }

    void OnScreenResize()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;

        if (screenSize.X == 600)
        {
            MainContainer.CustomMinimumSize = new Vector2(screenSize.X, screenSize.Y - 1080);
            MainContainer.Size = new Vector2(screenSize.X, screenSize.Y - 1080);
            MainContainer.Position = new Vector2(0, 1080);
            MainContainer.Position = new Vector2(0, 1080);

            StatusScrollContainer.Hide();
            Placeholder.Hide();
        }
        else
        {
            MainContainer.CustomMinimumSize = Vector2.Zero;
            MainContainer.Position = Vector2.Zero;
            StatusScrollContainer.Show();
            Placeholder.Show();
        }

        GD.Print("Game.cs -> Game resizing: ", screenSize);

        TouchInputSetup();
    }

    [ExportGroup("TouchInputs")]
    [Export]
    TouchScreenButton LeftPaddleButton { get; set; }
    [Export]
    TouchScreenButton RightPaddleButton { get; set; }
    [Export]
    TouchScreenButton PlungerButton { get; set; }

    void TouchInputSetup()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;

        LeftPaddleButton.Position = new(screenSize.X / 4, screenSize.Y / 2);
        RightPaddleButton.Position = new((screenSize.X / 4) * 3, screenSize.Y / 2);
        ((RectangleShape2D)LeftPaddleButton.Shape).Size = new(screenSize.X / 2, screenSize.Y);
        ((RectangleShape2D)RightPaddleButton.Shape).Size = new(screenSize.X / 2, screenSize.Y);
    }
}
