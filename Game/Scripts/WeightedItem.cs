using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static ItemType ChooseFrom(IEnumerable<WeightedItem<ItemType>> list)
        {
            return GetFrom(list).Item;
        }

        public static WeightedItem<ItemType> GetFrom(IEnumerable<WeightedItem<ItemType>> list)
        {
            uint index = GD.Randi() % GetTotalWeight(list);

            uint currentWeight = 0;

            foreach (var weightedItem in list)
            {
                currentWeight += weightedItem.Weight;
                if (currentWeight > index)
                    return weightedItem;
            }
            return list.ToArray()[0];
        }


        static uint GetTotalWeight(IEnumerable<WeightedItem<ItemType>> list)
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
