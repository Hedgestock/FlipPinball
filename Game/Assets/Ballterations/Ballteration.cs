using Godot;
using System;
using System.Linq;

public partial class Ballteration : Node
{
    public enum Rarity
    {
        Black = -1,
        Yellow,
        Grey,
        Green,
        Blue,
        Purple,
        Red,
        Analog,
    }

    [Flags]
    public enum Type
    {
        Score = 0001,
        Physics = 0010,
        Meta = 0100,
        Other = 1000,
    }

    [Export]
    public Type Kind = Type.Other;

    [Export]
    public Rarity Color = Rarity.Analog;

    public float AnalogRarity
    {
        get
        {
            try
            {
                return GetChildren().OfType<Effect>().Select(e => e.AnalogRarity).Sum();
            }
            catch (Exception ex)
            {
                GD.PrintErr(ex);
                return 1;
            }
        }
    }

    [Export]
    public string DisplayName;

    public void Ameliorate()
    {
        foreach (var effect in GetChildren().OfType<Effect>())
        {
            var betterEffect = effect.Ameliorate();
            RemoveChild(effect);
            AddChild(betterEffect);
        }
    }

    public void Worsen()
    {
        foreach (var effect in GetChildren().OfType<Effect>())
        {
            var betterEffect = effect.Worsen();
            RemoveChild(effect);
            AddChild(betterEffect);
        }
    }

    //public Ballteration()
    //{
    //    Kind = 0000;
    //    var children = GetChildren();
    //    if (children.Any(c => c is MetaEffect))
    //    {
    //        Kind |= Type.Meta;
    //    }
    //    if (children.Any(c => c is ScoreModifier))
    //    {
    //        Kind |= Type.Score;
    //    }
    //}
}