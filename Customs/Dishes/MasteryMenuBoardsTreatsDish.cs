using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu;
using KitchenMasteryMenu.Customs.Dishes.Steaks;
using KitchenMasteryMenu.Customs.Dishes.Spaghetti;
using KitchenMasteryMenu.Customs.Dishes.Turkey;
using KitchenMasteryMenu.Customs.Ingredients;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KitchenMasteryMenu.Customs.Dishes.Starters;
using KitchenMasteryMenu.Customs.Dishes.Desserts;
using KitchenMasteryMenu.Customs.Dishes.Sundaes;

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuBoardsTreatsDish : GenericMasteryDishCard
    {
        protected override string NameTag => "Boards and Treats";
        public override DishType Type => DishType.Dessert;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override bool RequiredNoDishItem => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mastery Ingredients
            GDOUtils.GetCastedGDO<Item, MasteryIceCreamStrawberry>(),
            GDOUtils.GetCastedGDO<Item, MasteryIceCreamVanilla>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.ServingBoard)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMasteryDish> ContainedMasteryRecipes => new()
        {
            // Add the Mastery versions of the remaining starters, plus Cheese Board & Ice Cream
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBreadBoardDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryChristmasCrackersDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryMandarinStarterDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryPumpkinSeedDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCheeseBoardDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryIceCreamServingDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryIceCreamChocolateDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryIceCreamStrawberryDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryIceCreamVanillaDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeChocolateIceCreamDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeGlassDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeStrawberryIceCreamDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeVanillaIceCreamDish>()
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color>  <i>Starters</i> - Pumpkin Seeds" +
                ", Bread (Boards), Christmas Crackers, Mandarin Starter.\n" +
                "<i>Desserts</i> - Ice Cream (each flavor individually), Cheese Boards, Sundaes" +
                "Adds two extra Mastery Ingredient Providers."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Boards & Treats",
                Description = "Adds Bread Boards, Christmas Crackers, Mandarin Starter, and Pumpkin Seeds as possible starters.\n" +
                "Adds Cheese Boards, Ice Cream, and Sundaes as possible desserts (where each Ice Cream flavor takes up an entire provider)\n" +
                "Provides two additional Mastery Ingredient Providers.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuBaseMainsDish>()
        };
    }
}
