using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Dishes.Sandwiches
{
    public class MasterySandwichClubDish : GenericMasteryDish
    {
        protected override string NameTag => "Mastery Sandwich Club Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SandwichTurkeyClubDish);
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => null;
        public override bool IsMainThatDoesNotNeedPlates => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 5;
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Knead),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color> Flour, Lettuce, Tomato, Turkey, Egg, Oil\n" + 
                "Toast sandwich, but larger. Knead flour once (or add water) to make Dough and cook to make Bread. Cook raw turkey.\n" +
                "Portion 3 Bread Slices and cook to make 3 Toast. Chop Lettuce & Tomato as requested. Portion Turkey as requested. Chop Egg and combine with Oil to make Mayo as requested.\n" +
                "Serve ingredients to customers piecemeal." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Sandwich - Club",
                Description = "Adds club sandwiches as a main dish when <b>flour</b>, <b>Lettuce</b>, <b>Tomato</b>, " +
                    "<b>Turkey</b>, <b>Egg</b>, and <b>Oil</b> are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item) GDOUtils.GetExistingGDO(References.SandwichClubPiecemeal),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };

        public override HashSet<Item> MinimumRequiredMasteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Lettuce),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Tomato),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.TurkeyIngredient),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Egg),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Oil)
        };
        public override bool RequiresBaseVariant => false;
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuSubstitutionsComplexityDish>()
        };
    }
}
