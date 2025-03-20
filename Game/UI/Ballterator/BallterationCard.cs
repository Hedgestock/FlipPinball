using Godot;
using System;
using System.Linq;

public partial class BallterationCard : PanelContainer
{
    [Signal]
    public delegate void BallterationChosenEventHandler(Ballteration ballteration, long price);

    [Export]
    VBoxContainer DescriptionContainer;
    [Export]
    Label NameLabel;
    [Export]
    Button BuyButton;

    long _price = 0;
    public long Price
    {
        set
        {
            _price = value;
            BuyButton.Text = $"Ballterate ({value})";
        }
    }

    Ballteration _ballteration;
    public Ballteration Ballteration
    {
        set
        {
            _ballteration = value;
            NameLabel.Text = _ballteration.DisplayName;

            PaintCard();

            // TODO: Check for float overflow
            Price = (long)(_ballteration.Color == Ballteration.Rarity.Analog ? _ballteration.AnalogRarity : (int)_ballteration.Color)* GameManager.TargetScore / 10;

            foreach (var effect in value.GetChildren().OfType<Effect>())
            {
                Label effectDescription = new();
                effectDescription.Text = effect.Description;
                effectDescription.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                DescriptionContainer.AddChild(effectDescription);
            }

            //Callable.From(() =>
            //GD.Print($"Container is: {DescriptionContainer.Size}")
            //).CallDeferred();
        }
    }

    void OnClick()
    {
        EmitSignal(SignalName.BallterationChosen, _ballteration, _price);
    }

    void PaintCard()
    {
        StyleBox sb = (StyleBox)GetThemeStylebox("panel").Duplicate();

        var lerpColors = (Color c1, Color c2, float amount) =>
        {
            Color lerped = new Color(
                Mathf.Lerp(c1.R, c2.R, amount),
                Mathf.Lerp(c1.G, c2.G, amount),
                Mathf.Lerp(c1.B, c2.B, amount)
                );
            return lerped;
        };

        switch (_ballteration.Color)
        {
            case Ballteration.Rarity.Analog:
            default: // TODO: Will I ever get to clean that?
                if (_ballteration.AnalogRarity <= -1)
                    (sb as StyleBoxFlat).BgColor = Colors.Black;
                else if (_ballteration.AnalogRarity > -1 && _ballteration.AnalogRarity <= 0)
                    (sb as StyleBoxFlat).BgColor = lerpColors(Colors.Black, new Color("#999999"), _ballteration.AnalogRarity - 1);
                else if (_ballteration.AnalogRarity > 1 && _ballteration.AnalogRarity <= 2)
                    (sb as StyleBoxFlat).BgColor = lerpColors(new Color("#999999"), Colors.DarkGreen, _ballteration.AnalogRarity - 1);
                else if (_ballteration.AnalogRarity > 2 && _ballteration.AnalogRarity <= 3)
                    (sb as StyleBoxFlat).BgColor = lerpColors(Colors.DarkGreen, Colors.DarkBlue, _ballteration.AnalogRarity - 2);
                else if (_ballteration.AnalogRarity > 3 && _ballteration.AnalogRarity <= 4)
                    (sb as StyleBoxFlat).BgColor = lerpColors(Colors.DarkBlue, Colors.RebeccaPurple, _ballteration.AnalogRarity - 3);
                else if (_ballteration.AnalogRarity > 4 && _ballteration.AnalogRarity <= 5)
                    (sb as StyleBoxFlat).BgColor = lerpColors(Colors.RebeccaPurple, Colors.DarkRed, _ballteration.AnalogRarity - 4);
                else if (_ballteration.AnalogRarity > 5)
                    (sb as StyleBoxFlat).BgColor = Colors.DarkRed;
                break;
            case Ballteration.Rarity.Black:
                (sb as StyleBoxFlat).BgColor = Colors.Black;
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
    }
}
