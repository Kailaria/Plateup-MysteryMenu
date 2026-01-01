using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Sandwiches
{
    public class MysterySandwichBaseDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Sandwich Base Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SandwichBaseDish);
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => null;
        public override bool IsMainThatDoesNotNeedPlates => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 4;
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Knead),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color> Flour,  <i>at least one base sandwich ingredient</i>\n" + 
                "Knead flour once (or add water) and cook to make Bread. Portion and serve piecemeal." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Sandwich",
                Description = "Adds sandwich as a main dish when <b>flour</b> and one other base sandwich ingredient (ham slices, lettuce, or tomato) are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item) GDOUtils.GetExistingGDO(References.SandwichPiecemeal),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };

        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>();
        public override bool RequiresBaseVariant => true;
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
