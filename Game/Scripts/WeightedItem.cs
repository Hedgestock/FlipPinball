using Godot;
using System;

namespace Godot.FlipPinball
{
    public struct WeightedItem<ItemType>
    {
        public WeightedItem(ItemType item, uint weight = 100)
        {
            Weight = weight;
            Item = item;
        }

        public uint Weight;
        public ItemType Item;

        public static ItemType ChooseFrom(WeightedItem<ItemType>[] list)
        {
            uint index = GD.Randi() % GetTotalWeight(list);

            uint currentWeight = 0;

            foreach (var weightedItem in list)
            {
                currentWeight += weightedItem.Weight;
                if (currentWeight > index)
                    return weightedItem.Item;
            }
            return list[0].Item;
        }

        static uint GetTotalWeight(WeightedItem<ItemType>[] list)
        {
            uint totalWeight = 0;

            foreach (var item in list)
            {
                totalWeight += item.Weight;
            }

            return totalWeight;
        }

    }
}
