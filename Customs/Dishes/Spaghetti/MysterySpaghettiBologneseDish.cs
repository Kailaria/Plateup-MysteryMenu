using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Spaghetti
{
    public class MysterySpaghettiBologneseDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Spaghetti Bolognese Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SpaghettiBologneseDish);
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color> Tomato, Spaghetti, Mince\n" +
                "Put raw spaghetti into a pot with water and boil, then empty the water into the trash. " +
                "Cook mince. Chop tomato twice to make sauce. Combine cooked mince and tomato sauce in a pot, then cook. " +
                "Combine boiled pasta on a plate with the pot of sauce." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Spaghetti Bolognese",
                Description = "Adds spaghetti bolognese as a main when <b>Tomatoes</b>, <b>Mince</b>, and <b>Raw Spaghetti</b> are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(References.SpaghettiBolognesePlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Tomato),
            (Item) GDOUtils.GetExistingGDO(References.SpaghettiRaw),
            (Item) GDOUtils.GetExistingGDO(References.Mince)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuSaucesDish>()
        };
    }
}
