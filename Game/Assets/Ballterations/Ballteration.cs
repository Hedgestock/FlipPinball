using Godot;
using System;
using System.Linq;

public partial class Ballteration : Node
{
    public enum Rarity
    {
        Yellow,
        Grey,
        Green,
        Blue,
        Red,
        Purple,
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
    public Rarity Color = Rarity.Yellow;

    public float AnalogRarity
    {
        get
        {
            try
            {
                return GetChildren().OfType<Effect>().Select(e => e.AnalogRarity).Sum();
            }
            catch
            {
                return 1;
            }
        }
    }

    [Export]
    public string DisplayName;

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