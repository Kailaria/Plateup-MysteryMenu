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
    public class MysteryCakesLemonCookieDish : GenericMysteryDish
    {
        protected override string NameTag => "Cake - Lemon Cookies";
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
                "<color=yellow>Requires ingredients:</color> Cookie Tray, Flour, Egg, Sugar, Lemon\n" +
                $"Mix {References.ColorTextCakeBatter} in a mixing bowl. Chop a lemon. Combine the lemon in the " +
                "cake batter mixing bowl, and then pour into cookie tray. Cook the tray of cookies.\n" +
                $"Portion up to 4 times and serve to customers ordering lemon flavour {References.PinkTintCakesText} for dessert."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Cakes - Lemon Cookies",
                Description = "Adds lemon cookies as a dessert when flour, egg, sugar, lemon, and a cookie tray are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.LemonFlavour),
                Phase = MenuPhase.Dessert,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.CookieTray),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Sugar),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Egg),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Lemon)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakeVarietyDish>()
        };
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
        public override bool HasTrayIngredient => true;
    }
}
