using Godot;
using System;

public partial class Ballterator : ScrollContainer
{
    [Signal]
    public delegate void BallterationChosenEventHandler(Ballteration ballteration);

    [Export]
    FlowContainer Container;
    public override void _Ready()
    {
        base._Ready();
        Container.AddChild(CreateBalterationCard());
        Container.AddChild(CreateBalterationCard());
        Container.AddChild(CreateBalterationCard());
    }

    Control CreateBalterationCard()
    {
        MarginContainer cardMargin = new();
        BallterationCard card = GD.Load<PackedScene>("res://Game/UI/Ballterator/BallterationCard.tscn").Instantiate<BallterationCard>();
        card.BallterationChosen += Hide;

        //var bt = GD.Load<PackedScene>("res://Game/Assets/Ballterations/Ballteration.tscn").Instantiate<Ballteration>();
        //bt.DisplayName = "Prout";
        //bt.AddChild(GD.Load<PackedScene>("res://Game/Assets/Ballterations/Effects/ScoreModifier/Tests/GlobalAdder.tscn").Instantiate<ScoreModifier>());

        card.Ballteration = ScoreModifier.CreateRandomSimple();
        cardMargin.AddChild(card);
        return cardMargin;
    }


}
