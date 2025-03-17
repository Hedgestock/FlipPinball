using Godot;
using System;
using System.Linq;

public partial class BallterationCard : PanelContainer
{
    [Signal]
    public delegate void BallterationChosenEventHandler();

    [Export]
    VBoxContainer DescriptionContainer;
    [Export]
    Label NameLabel;

    private Ballteration _ballteration;

    public Ballteration Ballteration
    {
        set
        {
            _ballteration = value;
            NameLabel.Text = _ballteration.DisplayName;
            foreach (var effect in value.GetChildren().OfType<Effect>())
            {
                Label effectDescription = new();
                effectDescription.Text = effect.Description;
                DescriptionContainer.AddChild(effectDescription);
            }
        }

    }

    void OnClick()
    {
        GameManager.BallQueue.First().AddChild(_ballteration);
        EmitSignal(SignalName.BallterationChosen);
        GetTree().Paused = false;
    }

}
