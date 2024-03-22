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

namespace KitchenMysteryMenu.Customs.Dishes.Turkey
{
    public class MysteryTurkeyBaseDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Turkey Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.TurkeyBase);
        public override DishType Type => DishType.Base;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 1;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=yellow>Requires ingredient:</color> Turkey\n" + 
                "Cook Turkey, then portion, plate, and serve. Portions up to 4 times." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Turkey",
                Description = "Adds <b>Turkey</b> as a main when it is present",
                FlavourText = "Gobble gobble gobble!"
            })
        };
        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.TurkeyPlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.TurkeyIngredient)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
