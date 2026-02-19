using Godot;
using WaffleStock;

[GlobalClass]
public partial class WeightedBoard : WeightedItem<PackedScene>
{
    [Export]
    new public PackedScene Item
    {
        get { return base.Item; }
        set { base.Item = value; }
    }
}