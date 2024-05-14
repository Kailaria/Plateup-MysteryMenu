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
    public class MysteryCoffeeExtraSugarDish : GenericMysteryDish
    {
        protected override string NameTag => "Coffee - Extra Sugar";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.ExtraSugar);
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
                "<color=yellow>Requires tray:</color> Sugar\n" +
                $"After serving a {References.ColorTextHotDrink}, customers may request sugar as an extra if available." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Coffee - Extra Sugar",
                Description = "Adds sugar as a possible extra after serving a coffee drink.",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks => new()
        {
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.CoffeeCupCoffee),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.Sugar)
            },
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.Latte),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.Sugar)
            },
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.IcedCoffee),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.Sugar)
            },
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.TeaPotSteeped),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.Sugar)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Sugar),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakeVarietyDish>()
        };
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeBaseDish>();
    }
}
