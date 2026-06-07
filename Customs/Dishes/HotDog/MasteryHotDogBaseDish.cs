using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Dishes.HotDog
{
    public class MasteryHotdogBaseDish : GenericMasteryDish
    {
        protected override string NameTag => "Mastery Hot Dog Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.HotdogBase);
        public override DishType Type => DishType.Base;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 1;
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color> Hot Dog Bun, Hot Dog\n" + 
                "Cook hot dog link, add hot dog bun, then serve.\n" +
                "You don't need to worry about Extra Ketchup with only the base Mastery Menu." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Hot Dog",
                Description = "Adds hot dogs as a main when <b>Hot Dog Buns</b> and <b>Hot Dog Links</b> are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.HotdogPlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMasteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.HotdogBun),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.HotdogRaw)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuBaseMainsDish>()
        };
    }
}
