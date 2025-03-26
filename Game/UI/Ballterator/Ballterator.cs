using Godot;
using System;
using System.Linq;

public partial class Ballterator : Container
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
    long RerollPrice = 0;
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

    void StartBallterating()
    {
        if (Visible == false) return;

        CreditsLeft = Math.Max(GameManager.Credits, 0) + ScoreManager.ScoreValue - GameManager.TargetScore;
        RerollPrice = GameManager.TargetScore / 2;

        BallterationCycleNumber = 0;
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
        CreditsLabel.Text = $"{(CreditsLeft < 0 ? "Debt" : "Credits")}: ({CreditsLeft:N0})";
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

        // TODO: Look into optimisation maybe ? Not critical
        BallterationCard card = GD.Load<PackedScene>("res://Game/UI/Ballterator/BallterationCard.tscn").Instantiate<BallterationCard>();
        card.BallterationChosen += (Ballteration ballteration, long price) =>
        {
            Ballterations.Hide();

            CreditsLeft -= price;

            foreach (var metaEffect in ballteration.GetChildren().OfType<MetaEffect>())
            {
                metaEffect.Activate();
                ballteration.RemoveChild(metaEffect);
            }
            if (ballteration.GetChildren().Any())
            {
                SelectedBallteration = ballteration;
                DisplayBallSelector();
            }
            else
            {
                BallterationCycleEnd();
            }
        };


        card.Ballteration = BallterationGenerator.GenerateToRarityCurve();


        //if ((BallterationCycleNumber == 0 && i == 0) || GameManager.BallQueue.Count == 0)
        //{
        //    var tmp = BallterationGenerator.CreateNewBall();
        //    tmp.Color = Ballteration.Rarity.Yellow;
        //    card.Ballteration = tmp;
        //    card.Price = 0;
        //}

        //else
        //{
        //    var tmp = BallterationGenerator.CreateSimpleScoreModifier();
        //    tmp.Color = Ballteration.Rarity.Blue;
        //    card.Ballteration = tmp;
        //}

        //else if (BallterationCycleNumber == 0)
        //{
        //    card.Ballteration = BallterationGenerator.CreateChaosScoreModifier();
        //}
        //else if (BallterationCycleNumber == 1)
        //{
        //    card.Ballteration = BallterationGenerator.CreateScoreModifier();
        //}
        //else
        //{
        //    card.Ballteration = BallterationGenerator.CreateSimpleScoreModifier();
        //}

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
            // TODO: Preload that
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

    int BallterationCycleNumber;

    void BallterationCycleEnd()
    {
        BallterationCycleNumber++;
        if (CreditsLeft < 0)
        {
            Close();
        }
        else
        {
            DisplayBallterations();
        }
    }

    void Close()
    {
        GameManager.Credits = CreditsLeft;
        GameManager.AdvanceLevel();

        Hide();
        GetTree().Paused = false;
    }
}
