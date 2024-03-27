using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Pizza
{
    public class MysteryPizzaBaseDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Pizza Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.PizzaBase);
        public override DishType Type => DishType.Base;
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
                "<color=yellow>Requires ingredients:</color> Flour, Oil, Tomato, Cheese\n" +
                "Knead (or add water to) flour to make dough, then combine with oil to make Pizza Crust. " +
                "Chop a tomato twice to make tomato sauce. Chop cheese. " +
                "Combine the pizza crust with the sauce and cheese, then cook. Portions 4 slices." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Pizza",
                Description = "Adds pizzas as a main when <b>Flour</b>, <b>Oil</b>, <b>Tomato</b>, and <b>Cheese</b> are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.PizzaPlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Oil),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Tomato),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Cheese)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
