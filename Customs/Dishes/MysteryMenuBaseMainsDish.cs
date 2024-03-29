﻿using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu;
using KitchenMysteryMenu.Customs.Dishes.Breakfast;
using KitchenMysteryMenu.Customs.Dishes.Burger;
using KitchenMysteryMenu.Customs.Dishes.Dumplings;
using KitchenMysteryMenu.Customs.Dishes.Fish;
using KitchenMysteryMenu.Customs.Dishes.HotDog;
using KitchenMysteryMenu.Customs.Dishes.Pies;
using KitchenMysteryMenu.Customs.Dishes.Pizza;
using KitchenMysteryMenu.Customs.Dishes.Salad;
using KitchenMysteryMenu.Customs.Dishes.Spaghetti;
using KitchenMysteryMenu.Customs.Dishes.Steaks;
using KitchenMysteryMenu.Customs.Dishes.StirFry;
using KitchenMysteryMenu.Customs.Dishes.Turkey;
using KitchenMysteryMenu.Customs.Ingredients;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuBaseMainsDish : GenericMysteryDishCard
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
        public override int Difficulty => 3;
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
            // Add X Mystery Ingredients
            GDOUtils.GetCastedGDO<Item, MysteryMeat>(),
            GDOUtils.GetCastedGDO<Item, MysteryFlour>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Wok),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot)
            //,
            //(Item)GDOUtils.GetExistingGDO(ItemReferences.Pot)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of every base main
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBreakfastBaseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBurgerBaseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryDumplingsBaseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryFishBlueDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryFishPinkDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryHotdogBaseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryPiesBaseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryPiesMeatDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryPizzaBaseDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySaladBaseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySaladTomatoDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySpaghettiBaseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySteakBaseDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryStirFryBaseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryStirFryRiceDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryStirFryBroccoliDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryStirFryCarrotDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTurkeyBaseDish>()
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "Make other base main recipes with the given ingredients, then plate.\n" +
                "All available Mystery Recipes will have a line at the top like \"<color=yellow>Requires ingredients:</color>\"; " +
                "all ingredients listed in that paragraph must be available from any provider (mysterious or static) for " +
                "customers to order it.\n" +
                "Text in <i>italics</i> indicates a reference to at least one other recipe."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery Menu",
                Description = "Adds all default base mains as mains and provides two Mystery Ingredient Providers to make them.",
                FlavourText = "Available ingredients will vary each day.\n" +
                    "Make sure you're ready for any and every recipe you have that those ingredients can make!"
            })
        };
    }
}
