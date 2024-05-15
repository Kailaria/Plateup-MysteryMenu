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
            ItemReferences.ExtraCakeStand,
            References.LasagneTray
        };

        private static HashSet<int> Trays = new HashSet<int>()
        {
            ItemReferences.CookieTray,
            ItemReferences.BrownieTray,
            ItemReferences.CupcakeTray,
            ItemReferences.DoughnutTray,
            ItemReferences.BigCakeTin,
            ItemReferences.ExtraCakeStand,
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

        public static string ColorizeSpriteTextToCake(string innerStr)
        {
            return ColorizeSpriteText(innerStr, References.ColorCakeHex, References.SpriteCakeTint1);
        }

        public static string ColorizeSpriteTextToHotDrink(string innerStr)
        {
            return ColorizeSpriteText(innerStr, References.ColorHotDrinkHex, References.SpriteFillCoffeeTint1);
        }

        public static string ColorizeSpriteText(string innerStr, string color, string spriteStrWithTint)
        {
            string spriteAndTextStr = $"{spriteStrWithTint}{innerStr}";
            return $"{ColorizeText(spriteAndTextStr, color)}";
        }

        public static string ColorizeText(string innerStr, string color)
        {
            return $"<color={color}>{innerStr}</color>";
        }
    }
}
