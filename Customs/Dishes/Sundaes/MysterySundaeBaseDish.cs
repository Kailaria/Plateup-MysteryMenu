using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Sundaes
{
    public class MysterySundaeBaseDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Sundae Base Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SundaeBaseDish);
        public override DishType Type => DishType.Base;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => null;
        public override bool IsMainThatDoesNotNeedPlates => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override HashSet<Process> RequiredProcesses => new()
        {
        };
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color> Sundae Glass,  <i>at least one flavor of ice cream</i>\n" + 
                "Give customers a sundae glass when requested for Dessert. Serve piecemeal." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Sundae",
                Description = "Adds sundaes as a dessert dish when <b>Sundae Glasses</b> and an ice cream flavor are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item) GDOUtils.GetExistingGDO(References.SundaePiecemeal),
                Phase = MenuPhase.Dessert,
                Weight = 1
            }
        };

        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>();
        public override bool RequiresBaseVariant => true;
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBoardsTreatsDish>()
        };
    }
}
