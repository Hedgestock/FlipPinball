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

            PaintCard();

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


        AddThemeStyleboxOverride("panel", sb);
    } 
}
