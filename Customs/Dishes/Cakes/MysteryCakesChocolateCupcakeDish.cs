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
    public class MysteryCakesChocolateCupcakeDish : GenericMysteryDish
    {
        protected override string NameTag => "Cake - Chocolate Cupcakes";
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
                "<color=yellow>Requires ingredients:</color> Cupcake Tray, Flour, Egg, Sugar, Milk, Chocolate\n" +
                $"Mix {References.ColorTextCakeBatter} in a mixing bowl. Add milk to the Cake Batter, then pour into cupcake tray and cook the cupcakes.\n" +
                "Melt (cook) a chocolate. Portion a cupcake (up to 4 per tray) and combine with the melted chocolate. " +
                $"Serve to customers ordering chocolate flavour {References.PinkTintCakesText} for dessert."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Cakes - Chocolate Cupcakes",
                Description = "Adds chocolate cupcakes as a dessert when flour, egg, sugar, milk, chocolate, and a cupcake tray are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.ChocolateFlavour),
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
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Chocolate)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakeVarietyDish>()
        };
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
        public override bool HasTrayIngredient => true;
    }
}
