﻿using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu;
using KitchenMysteryMenu.Customs.Dishes.Steaks;
using KitchenMysteryMenu.Customs.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuDish : CustomDish
    {
        public override string UniqueNameID => Mod.MOD_GUID + ":MysteryMenuDish";
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
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Wok)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };
        public override List<Dish> AlsoAddRecipes => new()
        {
            // Add the Mystery versions of every base main
            GDOUtils.GetCastedGDO<Dish, MysterySteakDish>()
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery Menu",
                Description = "Adds all default base mains as mains",
                FlavourText = "Available ingredients will vary each day." +
                    "Make sure you're ready for everything that those ingredients can make!"
            })
        };
    }
}
