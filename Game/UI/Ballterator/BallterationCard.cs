using Godot;
using System;
using System.Linq;

public partial class BallterationCard : PanelContainer
{
    [Signal]
    public delegate void BallterationChosenEventHandler(Ballteration ballteration);

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

            StyleBox sb = (StyleBox)GetThemeStylebox("panel").Duplicate();
            switch (_ballteration.Color)
            {
                default:
                    break;
                case Ballteration.Rarity.Green:
                    (sb as StyleBoxFlat).BgColor = Colors.DarkGreen;
                    break;
                case Ballteration.Rarity.Blue:
                    (sb as StyleBoxFlat).BgColor = Colors.DarkBlue;
                    break;
                case Ballteration.Rarity.Red:
                    (sb as StyleBoxFlat).BgColor = Colors.DarkRed;
                    break;
                case Ballteration.Rarity.Purple:
                    (sb as StyleBoxFlat).BgColor = Colors.RebeccaPurple;
                    break;
                case Ballteration.Rarity.Yellow:
                    (sb as StyleBoxFlat).BgColor = Colors.DarkGoldenrod;
                    break;
            }
            AddThemeStyleboxOverride("panel", sb);

            foreach (var effect in value.GetChildren().OfType<Effect>())
            {
                Label effectDescription = new();
                effectDescription.Text = effect.Description;
                effectDescription.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                DescriptionContainer.AddChild(effectDescription);
            }
        }
    }

    void OnClick()
    {
        EmitSignal(SignalName.BallterationChosen, _ballteration);
    }

}
