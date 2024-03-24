using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Steaks
{
    public class MysterySteakSauceRedWineJusDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Steak - Red Wine Jus Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.SteakSauceRedWineJus);
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 2;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredient:</color> Onion, Wine Bottle\n" + 
                "Cook onion in a pot of water to make broth. Add red wine to the broth and cook again. " +
                "Plate with any  <i>Steak</i> dish." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Steak Sauce - Red Wine Jus",
                Description = "Adds Red Wine Jus as a sauce option for any Steak dish",
                FlavourText = "(placeholder card, do not add with Cards Manager)"
            })
        };
        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        {
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.SteakPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.SauceRedPortion)
            },
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.BonedSteakPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.SauceRedPortion)
            },
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.ThickSteakPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.SauceRedPortion)
            },
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.ThinSteakPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.SauceRedPortion)
            },
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Onion),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.WineBottle),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuSaucesDish>()
        };
    }
}
