using Godot;
using System;
using System.Linq;


namespace FlipPinball
{
    internal static class BallterationGenerator
    {
        public static Ballteration CreateSimpleScoreModifier()
        {
            Ballteration ballteration = new();
            ScoreModifier modifier = ScoreModifier.CreateRandomSimple();

            ballteration.AddChild(modifier);

            ballteration.Kind = Ballteration.Type.Score;
            ballteration.DisplayName = $"{modifier.GetGroups().First()}{((int)modifier.Prio % 2 == 1 ? " super" : "")} {((int)modifier.Prio <= 1 ? "adder" : "multiplier")}";

            return ballteration;
        }
        public static Ballteration CreateScoreModifier()
        {
            Ballteration ballteration = new();
            ScoreModifier modifier = ScoreModifier.CreateRandom();

            ballteration.AddChild(modifier);

            ballteration.Kind = Ballteration.Type.Score;
            ballteration.DisplayName = $"Score {((int)modifier.Prio % 2 == 1 ? " super" : "")} {((int)modifier.Prio <= 1 ? "adder" : "multiplier")}";

            return ballteration;
        }

        public static Ballteration CreateChaosScoreModifier()
        {
            Ballteration ballteration = new();

            for (int i = 0; i < GD.RandRange(1, 4); i++)
            {
                ScoreModifier modifier = ScoreModifier.CreateRandom();
                ballteration.AddChild(modifier);
            }

            ballteration.Kind = Ballteration.Type.Score;
            ballteration.DisplayName = $"Chaotic score modifier";

            return ballteration;
        }

        public static Ballteration CreateNewBall()
        {
            return GD.Load<PackedScene>("uid://cyd2kjk3ugtu6").Instantiate<Ballteration>();
        }

        public static Ballteration EnsureRarity(Func<Ballteration> Creator, Ballteration.Rarity rarity, bool max = true, int retries = 20)
        {
            Ballteration ballteration = Creator();
            for (int i = 0; i < retries; i++)
            {
                if ((max && ballteration.AnalogRarity <= (float)rarity) || (!max && ballteration.AnalogRarity >= (float)rarity))
                {
                    //GD.Print($"Generated ballteration in {i} retries (max: {max}, rarity: {rarity})");
                    return ballteration;
                }
                ballteration = Creator();
            }

            GD.PrintErr($"Failed to generate ballteration meeting rarity criterias (max: {max}, rarity: {rarity})");
            return ballteration;
        }
    }
}
