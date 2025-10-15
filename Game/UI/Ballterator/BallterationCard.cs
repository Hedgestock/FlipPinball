using Godot;
using System;
using System.Linq;

public partial class BallterationCard : PanelContainer
{
    [Signal]
    public delegate void BallterationChosenEventHandler(Ballteration ballteration, long price);

    [Export]
    Label NameLabel;
    [Export]
    Button BuyButton;
    [Export]
    VBoxContainer DescriptionContainer;

    long _price = 0;
    public long Price
    {
        set
        {
            _price = value;
            BuyButton.Text = $"Ballterate ({value:N0})";
            GD.Print($"ballteration credits {value} > {GameManager.Credits} {value > GameManager.Credits}");
            BuyButton.Modulate = value > GameManager.Credits ? Colors.Red : Colors.White;
        }
        get
        {
            return _price;
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
            Price = (long)(_ballteration.Rarity * GameManager.TargetScore / 10);

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
        EmitSignalBallterationChosen(_ballteration, _price);
    }

    void PaintCard()
    {
        StyleBox sb = (StyleBox)GetThemeStylebox("panel", "Card").Duplicate();

        if (_ballteration.Rarity == (float)Ballteration.RarityColor.Fixed)
            (sb as StyleBoxFlat).BgColor = Colors.DarkGoldenrod;
        else if (_ballteration.Rarity < -1)
            (sb as StyleBoxFlat).BgColor = Colors.Black;
        else if (_ballteration.Rarity >= -1 && _ballteration.Rarity < 0)
            (sb as StyleBoxFlat).BgColor = LerpColors(Colors.Black, new Color("#999999"), _ballteration.Rarity - 1);
        else if (_ballteration.Rarity >= 1 && _ballteration.Rarity < 2)
            (sb as StyleBoxFlat).BgColor = LerpColors(new Color("#999999"), Colors.DarkGreen, _ballteration.Rarity - 1);
        else if (_ballteration.Rarity >= 2 && _ballteration.Rarity < 3)
            (sb as StyleBoxFlat).BgColor = LerpColors(Colors.DarkGreen, Colors.DarkBlue, _ballteration.Rarity - 2);
        else if (_ballteration.Rarity >= 3 && _ballteration.Rarity < 4)
            (sb as StyleBoxFlat).BgColor = LerpColors(Colors.DarkBlue, Colors.RebeccaPurple, _ballteration.Rarity - 3);
        else if (_ballteration.Rarity >= 4 && _ballteration.Rarity < 5)
            (sb as StyleBoxFlat).BgColor = LerpColors(Colors.RebeccaPurple, Colors.DarkRed, _ballteration.Rarity - 4);
        else if (_ballteration.Rarity >= 5)
            (sb as StyleBoxFlat).BgColor = Colors.DarkRed;

        AddThemeStyleboxOverride("panel", sb);
    }

    static Color LerpColors(Color c1, Color c2, float amount)
    {
        Color lerped = new Color(
            Mathf.Lerp(c1.R, c2.R, amount),
            Mathf.Lerp(c1.G, c2.G, amount),
            Mathf.Lerp(c1.B, c2.B, amount)
            );
        return lerped;
    }
}
