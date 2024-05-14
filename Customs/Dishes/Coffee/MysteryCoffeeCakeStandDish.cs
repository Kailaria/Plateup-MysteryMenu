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

namespace KitchenMysteryMenu.Customs.Dishes.Coffee
{
    public class MysteryCoffeeCakeStandDish : GenericMysteryDish
    {
        protected override string NameTag => "Coffee - Cake Stand";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.CoffeeCakeStand);
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => default;
        public override bool RequiredNoDishItem => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 2;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires tray:</color> Cake Stand\n" + 
                $"After serving a {References.ColorTextHotDrink}, customers may request the Cake Stand as an extra if available." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Coffee - Cake Stand",
                Description = "Adds Cake Stand as a possible extra after serving a cup of Coffee.",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks => new()
        {
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.CoffeeCupCoffee),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.ExtraCakeStand)
            },
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.Latte),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.ExtraCakeStand)
            },
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.IcedCoffee),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.ExtraCakeStand)
            },
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.TeaPotSteeped),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.ExtraCakeStand)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.ExtraCakeStand),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakesPiesDish>()
        };
        public override bool HasTrayIngredient => true;
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeBaseDish>();
    }
}
