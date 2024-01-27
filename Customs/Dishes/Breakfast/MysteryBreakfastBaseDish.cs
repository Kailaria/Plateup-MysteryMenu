using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Breakfast
{
    public class MysteryBreakfastBaseDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Breakfast Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.BreakfastBase);
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
            { Locale.English, "Knead flour to make dough, then cook to make bread. Interact to cut a slice and cook to make toast." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Breakfast",
                Description = "Adds toast as a main when <b>Flour</b> is present",
                FlavourText = "The most important meal of the day"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.BreakfastPlated),
                Phase = MenuPhase.Main,
                Weight = 1,
                DynamicMenuType = References.DynamicMenuTypeMystery,
                DynamicMenuIngredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.Flour)
            }
        };
        public override List<Item> MinimumRequiredMysteryIngredients => new List<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour)
        };
        public override List<Item> UnlockedOptionalMysteryIngredients => new List<Item>();
    }
}
