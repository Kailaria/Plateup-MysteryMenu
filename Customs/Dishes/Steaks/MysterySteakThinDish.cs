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

namespace KitchenMysteryMenu.Customs.Dishes.Steaks
{
    public class MysterySteakThinDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Thin Steak Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.ThinSteaks);
        public override DishType Type => DishType.Main;
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
                "<color=yellow>Requires ingredient:</color> Thin Meat\n" + 
                "Cook steak once for rare, twice for medium, and thrice for well-done. " +
                "Cooks and burns faster than regular Meat" }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Thin-Cut Steak",
                Description = "Adds Thin-Cut steak as a main when <b>Thin Meat</b> is present",
                FlavourText = "Be careful, they cook fast!"
            })
        };
        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.ThinSteakPlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.MeatThin)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCarnivoreVariationsDish>()
        };
    }
}
