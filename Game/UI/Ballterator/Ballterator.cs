using Godot;
using System;

public partial class Ballterator : ScrollContainer
{
    [Export]
    FlowContainer BallterationsContainer;
    [Export]
    FlowContainer BallSelectionContainer;

    int BallterationCount = 3;

    void StartBallterating()
    {
        if (Visible == false) return;
        BallterationCyclesLeft = GameManager.BallQueue.Count;
        DisplayBallterations();
    }

    void DisplayBallterations()
    {
        foreach (var child in BallterationsContainer.GetChildren())
        {
            child.QueueFree();
        }
        for (int i = 0; i < BallterationCount; i++)
        {
            BallterationsContainer.AddChild(CreateBalterationCard());
        }
        BallterationsContainer.Show();
    }

    Control CreateBalterationCard()
    {
        MarginContainer cardMargin = new();
        BallterationCard card = GD.Load<PackedScene>("res://Game/UI/Ballterator/BallterationCard.tscn").Instantiate<BallterationCard>();
        card.BallterationChosen += (Ballteration ballteration) =>
        {
            SelectedBallteration = ballteration;
            BallterationsContainer.Hide();
            DisplayBallSelector();
        };

        card.Ballteration = ScoreModifier.CreateRandomSimple();
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
                BallSelectionContainer.Hide();
                BallterationCycleEnd();
            };
            BallSelectionContainer.AddChild(selector);
        }
        BallSelectionContainer.Show();
    }

    int BallterationCyclesLeft;

    void BallterationCycleEnd()
    {
        BallterationCyclesLeft--;
        if (BallterationCyclesLeft <= 0)
        {
            Hide();
            GetTree().Paused = false;
        } else
        {
            DisplayBallterations();
        }

    }
}
