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
    public class MysterySpaghettiCheesyDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Spaghetti Cheesy Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SpaghettiCheesyDish);
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
                "<color=yellow>Requires ingredients:</color> Spaghetti, Butter, Flour, Milk, Cheese\n" +
                "Put raw spaghetti into a pot with water and boil, then empty the water into the trash.\n" +
                "Add butter and flour to a pot and cook. Add milk and mix, then add more milk and mix again.\n" +
                "Combine boiled pasta on a plate with the sauce. Chop cheese and add." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Cheesy Spaghetti",
                Description = "Adds cheesy spaghetti as a main when <b>Cheese</b>, <b>Butter</b>, <b>Flour</b>, <b>Milk</b>, and <b>Raw Spaghetti</b> are present",
                FlavourText = "(placeholder card, do not add with Cards Manager)"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(References.SpaghettiCheesyPlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Cheese),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Milk),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(References.SpaghettiRaw),
            (Item) GDOUtils.GetExistingGDO(References.Butter)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
