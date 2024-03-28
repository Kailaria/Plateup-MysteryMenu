using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Turkey
{
    public class MysteryNutRoastDish : GenericMysteryDish
    {
        protected override string NameTag => "Nut Roast";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.NutRoastBase);
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
                "<color=yellow>Requires ingredients:</color> Onion, Nuts\n" +
                "Chop onions. Chop nuts. Combine and cook.\n" +
                "Portion nut roast onto a plate. Serves 3 portions." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Nut Roast",
                Description = "Adds nut roast as a main when <b>Nuts</b> and <b>Onions</b> are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemGroupReferences.NutRoastPlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.NutsIngredient),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Onion),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuVeggieVariationsDish>()
        };
    }
}
