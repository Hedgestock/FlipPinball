using Godot;
using System.Linq;


namespace FlipPinball
{
    internal static class BallterationGenerator
    {
        public static Ballteration CreateScorer()
        {
            Ballteration ballteration = new();
            ScoreModifier modifier = ScoreModifier.CreateRandomSimple();

            ballteration.AddChild(modifier);

            ballteration.Kind = Ballteration.Type.Score;
            ballteration.DisplayName = $"{modifier.GetGroups().First()}{((int)modifier.Prio % 2 == 1 ? " super" : "")} {((int)modifier.Prio <= 1 ? "adder" : "multiplier")}";
            ballteration.Color = (Ballteration.Rarity) modifier.AnalogRarity;

            return ballteration;
        }
    }
}
