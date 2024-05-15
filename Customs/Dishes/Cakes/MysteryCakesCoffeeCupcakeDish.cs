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

namespace KitchenMysteryMenu.Customs.Dishes.Cakes
{
    public class MysteryCakesCoffeeCupcakeDish : GenericMysteryDish
    {
        protected override string NameTag => "Cake - Coffee Cupcakes";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.Cupcake);
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
                "<color=yellow>Requires ingredients:</color> Cupcake Tray, Flour, Egg, Sugar, Milk, Coffee Cup\n" +
                $"Mix {References.ColorTextCakeBatter} in a mixing bowl. Add milk to the Cake Batter, then pour into cupcake tray and cook the cupcakes.\n" +
                "Brew a cup of coffee. Portion a cupcake (up to 4 per tray) and combine with the cup of coffee. " +
                $"Serve to customers ordering coffee flavour {References.PinkTintCakesText} for dessert."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Cakes - Coffee Cupcakes",
                Description = "Adds coffee cupcakes as a dessert when flour, egg, sugar, milk, coffee, and a cupcake tray are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.CoffeeFlavour),
                Phase = MenuPhase.Dessert,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.CupcakeTray),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Sugar),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Egg),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Milk),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.CoffeeCup)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakesPiesDish>()
        };
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
        public override bool HasTrayIngredient => true;
    }
}
