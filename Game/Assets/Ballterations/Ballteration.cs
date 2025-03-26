using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Ballteration : Node
{
    public enum RarityColor
    {
        Black = -1,
        Gray = 1,
        Green,
        Blue,
        Purple,
        Red,
        Fixed = 100,
    }

    bool IsRarityAnalog = true;

    RarityColor _rarity = RarityColor.Gray;

    float AnalogRarity
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
                return (float)_rarity;
            }
        }
    }

    public float Rarity
    {
        // TODO: Add more checks to make sure that the value is allowed
        set
        {
            if (Enum.IsDefined((RarityColor)Math.Round(value)))
            {
                _rarity = (RarityColor)value;
                IsRarityAnalog = false;
            }
        }
        get
        {
            if (IsRarityAnalog) return AnalogRarity;
            return (float)_rarity;
        }
    }

    [Export]
    public string DisplayName;

    public void Refine(Ballteration minimum = null, Ballteration maximum = null)
    {
        if (minimum == null && maximum == null) return;

        Effect[] children = GetChildren().OfType<Effect>().ToArray();
        Effect[] minChildren = minimum?.GetChildren().OfType<Effect>().ToArray() ?? null;
        Effect[] maxChildren = maximum?.GetChildren().OfType<Effect>().ToArray() ?? null;

        if ((minChildren?.Length ?? children.Length) != children.Length || (maxChildren?.Length ?? children.Length) != children.Length)
        {
            GD.PrintErr($"Tried to refine ballterations with different number of children");
            return;
        }
        for (int i = 0; i < children.Length; i++)
        {
            var betterEffect = children[i].Refine(minChildren?[i], maxChildren?[i]);
            RemoveChild(children[i]);
            AddChild(betterEffect);
        }
    }

    public bool Meta
    {
        get
        {
            return GetChildren().All(c => c is MetaEffect);
        }
    }
}