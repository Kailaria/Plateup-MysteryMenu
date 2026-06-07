using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu;
using KitchenMasteryMenu.Customs.Dishes.Breakfast;
using KitchenMasteryMenu.Customs.Dishes.Burger;
using KitchenMasteryMenu.Customs.Dishes.Dumplings;
using KitchenMasteryMenu.Customs.Dishes.Fish;
using KitchenMasteryMenu.Customs.Dishes.HotDog;
using KitchenMasteryMenu.Customs.Dishes.Pies;
using KitchenMasteryMenu.Customs.Dishes.Pizza;
using KitchenMasteryMenu.Customs.Dishes.Salad;
using KitchenMasteryMenu.Customs.Dishes.Sandwiches;
using KitchenMasteryMenu.Customs.Dishes.Spaghetti;
using KitchenMasteryMenu.Customs.Dishes.Steaks;
using KitchenMasteryMenu.Customs.Dishes.StirFry;
using KitchenMasteryMenu.Customs.Dishes.Tacos;
using KitchenMasteryMenu.Customs.Dishes.Turkey;
using KitchenMasteryMenu.Customs.Ingredients;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuBaseMainsDish : GenericMasteryDishCard
    {
        protected override string NameTag => "Base Mains";
        public override DishType Type => DishType.Base;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;
        //public override GameObject DisplayPrefab => 
        //public override GameObject IconPrefab => 
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Large;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => true;
        public override int Difficulty => 4;
        public override List<string> StartingNameSet => new()
        {
            "The Ferrous Chef",     // Iron Chef
            "Flayed",               // play on Chopped
            "Fieri'd Up",           
            "Heck's Kitchen",
            "The Eats Are Good",
            "Menu Whiplash",
            "RNG-licious",
            "You Want It We Got It",
            "It's Food, Sherlock!"
        };
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mastery Ingredients
            GDOUtils.GetCastedGDO<Item, MasteryMeat>(),
            GDOUtils.GetCastedGDO<Item, MasteryFlour>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Wok),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.TacoTray)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMasteryDish> ContainedMasteryRecipes => new()
        {
            // Add the Mastery versions of every base main
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBreakfastBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBurgerBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryDumplingsBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryFishBlueDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryFishPinkDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryHotdogBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryPiesBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryPiesMeatDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryPizzaBaseDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySaladBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySaladTomatoDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichBreadDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichHamSliceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichLettuceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichTomatoDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySpaghettiBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySteakBaseDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryStirFryBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryStirFryRiceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryStirFryBroccoliDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryStirFryCarrotDish>(),
            
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTacosBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTurkeyBaseDish>()
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "Make other base main recipes with the given ingredients, then plate.\n" +
                "All available Mastery Recipes will have a line at the top like \"<color=yellow>Requires ingredients:</color>\"; " +
                "all ingredients listed in that paragraph must be available from any provider (mysterious or static) for " +
                "customers to order it.\n" +
                "Text in <i>italics</i> indicates a reference to at least one other recipe."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Menu of Mastery",
                Description = "Adds all default base mains as mains and provides two Mastery Ingredient Providers to make them.",
                FlavourText = "Available ingredients will vary each day.\n" +
                    "Make sure you're ready for any and every recipe you have that those ingredients can make!"
            })
        };
    }
}
