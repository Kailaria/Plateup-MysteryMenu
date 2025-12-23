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
    public class MysterySpaghettiLasagneDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Lasagne Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.Lasagne);
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
                "<color=yellow>Requires ingredients:</color> Lasagne Tray (Tray), Lasagne Pasta Sheets, Butter, Flour, Milk, Cheese, Mince, Tomato\n" +
                "Make  <i>White Sauce</i> and  <i>Bolognese Sauce</i>.\n" +
                "Take a Lasange Tray, add the following in order: Bolognese, Lasagne Pasta Sheet, White Sauce; repeat a second time.\n" +
                "Cook the lasagne, then portion & plate to serve. Provides 4 portions per batch." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Lasagne",
                Description = "Adds Lasagne as a main when <b>Tomato</b>, <b>Mince</b>, <b>Butter</b>, <b>Flour</b>, <b>Milk</b>, and <b>Lasagne Pasta Sheets</b> are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemGroupReferences.LasagnePlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Tomato),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Mince),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Milk),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.LasagnePastaSheet),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Butter),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.LasagneTray)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuSubstitutionsComplexityDish>()
        };
    }
}
