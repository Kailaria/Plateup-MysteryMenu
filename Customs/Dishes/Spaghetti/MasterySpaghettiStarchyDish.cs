using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Dishes.Spaghetti
{
    public class MasterySpaghettiStarchyDish : GenericMasteryDish
    {
        protected override string NameTag => "Mastery Spaghetti - Extra Starchy Variant";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SpaghettiStarchyVariantDish);
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
                "<color=yellow>Requires ingredients:</color> Raw Spaghetti\n" +
                "After cooking spaghetti in the pot, the water can only be dumped in bins now." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Spaghetti - Extra Starch",
                Description = "Prevents Cooked Spaghetti pots from being drained into sinks. Only dump into a bin.",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<RestaurantStatus> AddsStatuses => new()
        {
            RestaurantStatus.BlockSinkBins
        };

        //public override List<Dish.MenuItem> ResultingMenuItems => new()
        //{
        //    new()
        //    {
        //        Item = (Item)GDOUtils.GetExistingGDO(ItemGroupReferences.LasagnePlated),
        //        Phase = MenuPhase.Main,
        //        Weight = 1
        //    }
        //};
        //public override HashSet<Item> MinimumRequiredMasteryIngredients => new HashSet<Item>()
        //{
        //    (Item) GDOUtils.GetExistingGDO(ItemReferences.Tomato),
        //    (Item) GDOUtils.GetExistingGDO(ItemReferences.Mince),
        //    (Item) GDOUtils.GetExistingGDO(ItemReferences.Milk),
        //    (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
        //    (Item) GDOUtils.GetExistingGDO(ItemReferences.LasagnePastaSheet),
        //    (Item) GDOUtils.GetExistingGDO(ItemReferences.Butter),
        //    (Item) GDOUtils.GetExistingGDO(ItemReferences.LasagneTray)
        //};
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuSubstitutionsComplexityDish>()
        };
    }
}
