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
using KitchenMysteryMenu.Customs.Appliances;
using KitchenMysteryMenu.Customs.Dishes.Cakes;
using KitchenMysteryMenu.Customs.Dishes.Coffee;
using KitchenMysteryMenu.Customs.Dishes.Burger;

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuSubstitutionsComplexityDish : GenericMysteryDishCard
    {
        protected override string NameTag => "Substitutions and Complexity";
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.LargeDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Large;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 5;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mystery Ingredients (normally requires at minimum: )
            GDOUtils.GetCastedGDO<Item, MysteryLasagnePastaSheet>(),
            GDOUtils.GetCastedGDO<Item, MysteryDoughnutTray>(),
            //(Item)GDOUtils.GetExistingGDO(ItemReferences.MixingBowlEmpty),
            //(Item)GDOUtils.GetExistingGDO(ItemReferences.Ice)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Knead)
        };

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of Lasagne, Doughnuts, Brownies, Fresh Burger Patties (Meat), and Fresh Patties (Mince)
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBurgerFreshPattyMeatDish>(),
            //(GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesChocolateCupcakeDish>(),
            //(GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesChocolateSpongeCakeDish>(),
            //(GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesLemonCookieDish>(),
            //(GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesLemonCupcakeDish>(),
            //(GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesLemonSpongeCakeDish>(),

            //(GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeExtraMilkDish>(),
            //(GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeExtraSugarDish>(),
            //(GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeIcedDish>(),
            //(GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeLatteDish>(),
            //(GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTeaDish>()
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                $"<color=#00ffff>New possible menu items:</color> {References.ColorTextCakeForms} - Doughnuts, Brownies\n" +
                $"Mains - Lasagne -- " + //Toppings - Giant Sandwiches, Giant Sundaes, Turkey Club Sandwich\n" +
                $"Substitute recipes - Fresh Burger Patties\n" +
                "Adds one extra Mystery Ingredient Provider and one extra Mystery Tray Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Substitutions and Complexities",
                Description = $"Adds Doughnuts and Brownies as possible {References.SpriteCake} Cake forms.\n" +
                $"Also adds Lasagne as a new main, and requires Burger Patties to be made Fresh with chopped Meat or Mince.\n" +
                "Provides one additional Mystery Ingredient Provider and one additional Mystery Tray Provider.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakesPiesDish>(),
            GDOUtils.GetCastedGDO<Dish, MysteryMenuSaucesSoupsDish>(),
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakeVarietyDish>()
        };
    }
}
