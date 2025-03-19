using Godot;
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

            for (int i = 0; i < GD.RandRange(1, 10); i++)
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
    }
}
