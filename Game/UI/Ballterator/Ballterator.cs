using Godot;
using System;
using System.Linq;

public partial class Ballterator : ScrollContainer
{
    [Export]
    Container Ballterations;
    [Export]
    Container Balls;

    [Export]
    FlowContainer BallterationsContainer;
    [Export]
    FlowContainer BallSelectionContainer;

    [Export]
    Button RerollButton;
    [Export]
    Label CreditsLabel;

    int BallterationCount = 3;
    long _creditsLeft = 0;
    long CreditsLeft
    {
        get { return _creditsLeft; }
        set
        {
            _creditsLeft = value;
            if (value >= 0)
                CreditsLabel.Text = $"Credits: ({CreditsLeft:N0})";
            else
                CreditsLabel.Text = $"Debt: ({CreditsLeft:N0})";
        }
    }
    long RerollPrice = 0;

    void StartBallterating()
    {
        if (Visible == false) return;

        CreditsLeft = ScoreManager.ScoreValue - GameManager.TargetScore;
        RerollPrice = GameManager.TargetScore / 2;

        BallterationCyclesLeft = GameManager.BallQueue.Count;
        DisplayBallterations();
    }

    void Reroll()
    {
        CreditsLeft -= RerollPrice;
        RerollPrice *= 2;
        DisplayBallterations();
    }

    void DisplayBallterations()
    {
        RerollButton.Text = $"Reroll ({RerollPrice:N0})";
        foreach (var child in BallterationsContainer.GetChildren())
        {
            child.QueueFree();
        }
        for (int i = 0; i < BallterationCount; i++)
        {
            BallterationsContainer.AddChild(CreateBalterationCard(i));
        }
        Ballterations.Show();
    }

    Control CreateBalterationCard(int i)
    {
        MarginContainer cardMargin = new();
        BallterationCard card = GD.Load<PackedScene>("res://Game/UI/Ballterator/BallterationCard.tscn").Instantiate<BallterationCard>();
        card.BallterationChosen += (Ballteration ballteration) =>
        {
            Ballterations.Hide();
            var children = ballteration.GetChildren();


            if (Ballteration.Type.Meta == ballteration.Kind)
            {
                foreach (var metaEffect in ballteration.GetChildren().OfType<MetaEffect>())
                {
                    metaEffect.Activate();
                }
                BallterationCycleEnd();
            }
            else
            {
                SelectedBallteration = ballteration;
                DisplayBallSelector();
            }
        };

        if ((BallterationCyclesLeft == 1 && i == 0) || GameManager.BallQueue.Count == 0)
        {
            card.Ballteration = GD.Load<PackedScene>("res://Game/Assets/Ballterations/PoolCommon/NewBall.tscn").Instantiate<Ballteration>();
        }
        else
        {
            card.Ballteration = ScoreModifier.CreateRandomSimple();
        }

        cardMargin.AddChild(card);
        return cardMargin;
    }

    Ballteration SelectedBallteration;

    void DisplayBallSelector()
    {
        foreach (var child in BallSelectionContainer.GetChildren())
        {
            child.QueueFree();
        }
        foreach (var ball in GameManager.BallQueue)
        {
            BallSelector selector = GD.Load<PackedScene>("res://Game/UI/Ballterator/BallSelector.tscn").Instantiate<BallSelector>();
            selector.Ball = ball;
            selector.BallSelected += (Ball ball) =>
            {
                ball.AddChild(SelectedBallteration);
                Balls.Hide();
                BallterationCycleEnd();
            };
            BallSelectionContainer.AddChild(selector);
        }
        Balls.Show();
    }

    int BallterationCyclesLeft;

    void BallterationCycleEnd()
    {
        BallterationCyclesLeft--;
        if (BallterationCyclesLeft <= 0)
        {
            GameManager.Debt = Math.Min(CreditsLeft, 0);

            Hide();
            GetTree().Paused = false;
        }
        else
        {
            DisplayBallterations();
        }
    }
}
