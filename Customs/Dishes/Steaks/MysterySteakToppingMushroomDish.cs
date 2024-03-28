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
    public class MysterySteakToppingMushroomDish : GenericMysteryDish
    {
        protected override string NameTag => "Steak - Mushroom Topping";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.SteakToppingMushroom);
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
                "<color=yellow>Requires ingredient:</color> Mushroom\n" + 
                "Chop a mushroom, then plate with any  <i>Steak</i> dish." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Steak Topping - Mushroom",
                Description = "Adds Mushroom as a topping for any Steak dish",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        {
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.SteakPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.MushroomChopped)
            },
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.BonedSteakPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.MushroomChopped)
            },
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.ThickSteakPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.MushroomChopped)
            },
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.ThinSteakPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.MushroomChopped)
            },
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Mushroom),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuToppingsDish>()
        };
    }
}
