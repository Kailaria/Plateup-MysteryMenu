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
    public class MysteryCakesCoffeeCookieDish : GenericMysteryDish
    {
        protected override string NameTag => "Cakes - Coffee Cookies";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.Cakes);
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
                "<color=yellow>Requires ingredients:</color> Cookie Tray, Flour, Egg, Sugar, Coffee\n" +
                "Mix  <color=pink><i>Cake Batter</i></color> in a mixing bowl. Brew a cup of coffee. Combine the coffee in the" +
                "cake batter mixing bowl, and then pour into cookie tray. Cook the tray of cookies.\n" +
                "Portion up to 4 times and serve to customers ordering coffee flavour cakes for dessert."}
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Cakes - Coffee Cookies",
                Description = "Adds coffee cookies as a dessert when flour, egg, sugar, coffee, and a cookie tray are present",
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
            (Item) GDOUtils.GetExistingGDO(ItemReferences.CookieTray),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Sugar),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Egg)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBoardsTreatsDish>()
        };
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
    }
}
