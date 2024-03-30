using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Salad
{
    public class MysterySaladAppleDish : GenericMysteryDish
    {
        protected override string NameTag => "Salad - Apple Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.SaladApple);
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color> Lettuce, Apple, Egg, Oil, Nuts\n" +
                "Chop lettuce. Chop an apple. Crack an egg, combine it with oil to make mayonnaise.\n" +
                "Combine the lettuce, apple, mayo, and nuts onto a plate." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Apple Salad",
                Description = "Adds apple salad as a main when <b>Lettuce</b>, <b>Apples</b>, <b>Eggs</b>, <b>Oil</b>, and <b>Nuts</b> are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemGroupReferences.SaladApplePlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Lettuce),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Apple),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Egg),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Oil),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.NutsIngredient),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuVeggieVariationsDish>()
        };
    }
}
