using KitchenData;
using KitchenLib.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Utils
{
    public class MysteryDishUtils
    {
        private static HashSet<int> ReusableItems = new HashSet<int>()
        {
            ItemReferences.Plate,
            ItemReferences.Wok,
            ItemReferences.Water,
            ItemReferences.MixingBowlEmpty,
            ItemReferences.Pot,
            ItemReferences.CookieTray,
            ItemReferences.BrownieTray,
            ItemReferences.DoughnutTray,
            ItemReferences.CupcakeTray,
            ItemReferences.BigCakeTin,
            References.LasagneTray
        };

        private static HashSet<int> Trays = new HashSet<int>()
        {
            ItemReferences.CookieTray,
            ItemReferences.BrownieTray,
            ItemReferences.CupcakeTray,
            ItemReferences.DoughnutTray,
            ItemReferences.BigCakeTin,
            References.LasagneTray
        };

        public static bool IsLimitedContainer(int providedItem)
        {
            return ReusableItems.Contains(providedItem);
        }

        public static bool IsTray(int providedItem)
        {
            return Trays.Contains(providedItem);
        }
    }
}
