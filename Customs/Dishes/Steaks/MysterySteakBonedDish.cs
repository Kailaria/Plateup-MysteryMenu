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
    public class MysterySteakBonedDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Boned Steak Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.BonedSteaks);
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
                "<color=yellow>Requires ingredient:</color> Bone-In Meat\n" + 
                "Cook steak once for rare, twice for medium, and thrice for well-done. " +
                "After the customer is done, portion off and trash the bone before washing the plate." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Bone-In Steak",
                Description = "Adds Bone-In steak as a main when <b>Bone-In Meat</b> is present",
                FlavourText = "Be sure to trash the bone when they're done!"
            })
        };
        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.BonedSteakPlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.MeatBoned)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCarnivoreVariationsDish>()
        };
    }
}
