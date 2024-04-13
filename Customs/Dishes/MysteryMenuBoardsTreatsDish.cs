using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu;
using KitchenMysteryMenu.Customs.Dishes.Steaks;
using KitchenMysteryMenu.Customs.Dishes.Spaghetti;
using KitchenMysteryMenu.Customs.Dishes.Turkey;
using KitchenMysteryMenu.Customs.Ingredients;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KitchenMysteryMenu.Customs.Dishes.Starters;
using KitchenMysteryMenu.Customs.Dishes.Desserts;

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuBoardsTreatsDish : GenericMysteryDishCard
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
            // Add X Mystery Ingredients
            GDOUtils.GetCastedGDO<Item, MysteryIceCreamStrawberry>(),
            GDOUtils.GetCastedGDO<Item, MysteryIceCreamVanilla>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.ServingBoard)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of the remaining starters, plus Cheese Board & Ice Cream
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBreadBoardDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryChristmasCrackersDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryMandarinStarterDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryPumpkinSeedDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCheeseBoardDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryIceCreamServingDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryIceCreamChocolateDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryIceCreamStrawberryDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryIceCreamVanillaDish>()
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color>  <i>Starters</i> - Pumpkin Seeds" +
                ", Bread (Boards), Christmas Crackers, Mandarin Starter.\n" +
                "<i>Desserts</i> - Ice Cream (each flavor individually), Cheese Boards" +
                "Adds two extra Mystery Ingredient Providers."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Boards & Treats",
                Description = "Adds Bread Boards, Christmas Crackers, Mandarin Starter, and Pumpkin Seeds as possible starters.\n" +
                "Adds Cheese Boards and Ice Cream as possible desserts (where each Ice Cream flavor takes up an entire provider)\n" +
                "Provides two additional Mystery Ingredient Providers.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
