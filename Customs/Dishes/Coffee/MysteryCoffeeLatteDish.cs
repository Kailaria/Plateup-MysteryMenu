using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Customs.Dishes.Turkey;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Coffee
{
    public class MysteryCoffeeLatteDish : GenericMysteryDish
    {
        protected override string NameTag => "Coffee - Latte";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.CoffeeLatte);
        public override DishType Type => DishType.Dessert;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override bool RequiredNoDishItem => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color> Coffee Cup, Milk\n" +
                "Place a cup of coffee on a coffee machine to brew. Place milk into the frother. " +
                "Place the cup of coffee on the milk frother and interact to make a Latte, then serve as a dessert.\n" +
                $"Counts as a {References.ColorTextHotDrink}."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Latte",
                Description = "Adds latte as a dessert when coffee cups and milk are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.Latte),
                Phase = MenuPhase.Dessert,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.CoffeeCup),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Milk)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakeVarietyDish>()
        };
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeBaseDish>();
    }
}
