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
using KitchenMasteryMenu.Customs.Appliances;
using KitchenMasteryMenu.Customs.Dishes.Cakes;
using KitchenMasteryMenu.Customs.Dishes.Coffee;
using KitchenMasteryMenu.Customs.Dishes.Burger;
using KitchenMasteryMenu.Customs.Dishes.Sandwiches;
using KitchenMasteryMenu.Customs.Dishes.Sundaes;

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuSubstitutionsComplexityDish : GenericMasteryDishCard
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
            // Add X Mastery Ingredients (normally requires at minimum: )
            GDOUtils.GetCastedGDO<Item, MasteryLasagnePastaSheet>(),
            GDOUtils.GetCastedGDO<Item, MasteryDoughnutTray>()
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Knead),
            (Process)GDOUtils.GetExistingGDO(References.ProcessFreeze)
        };

        // To block permanent versions of these items in Variety/Autumn
        public override HashSet<Item> BlockProviders => new()
        {
            (Item)GDOUtils.GetExistingGDO(ItemReferences.BurgerPattyRaw),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.IceCreamChocolate),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.IceCreamStrawberry),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.IceCreamVanilla)
        };

        public override HashSet<GenericMasteryDish> ContainedMasteryRecipes => new()
        {
            // Add the Mastery versions of Lasagne, Doughnuts, Brownies, Fresh Burger Patties (Meat)
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBurgerFreshPattyMeatDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySpaghettiStarchyDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySpaghettiLasagneDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesChocolateBrownieDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesChocolateDoughnutDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesCoffeeDoughnutDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesLemonDoughnutDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichClubDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichGiantDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichGiantHamSliceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichGiantLettuceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichGiantTomatoDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryHomemadeChocolateIceCreamDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryHomemadeStrawberryIceCreamDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryHomemadeVanillaIceCreamDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeGiantDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeGiantCherryDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeGiantChocolateIceCreamDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeGiantChocolateSyrupDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeGiantGlassDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeGiantNutsDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeGiantStrawberryIceCreamDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeGiantStrawberrySyrupDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeGiantVanillaIceCreamDish>()
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                $"<color=#00ffff>New possible menu items:</color> {References.ColorTextCakeForms} - Doughnuts, Brownies\n" +
                "Mains - Lasagne, Giant Sandwiches, Club Sandwiches, Giant Sundaes\n" +
                "Substitute recipes - Fresh Burger Patties, Starchy Spaghetti, Homemade Ice Cream & Sundaes (and blocks related substituted items' providers in Autumn)\n" +
                "Adds one extra Mastery Ingredient Provider, one extra Mastery Tray Provider, and Freezers (if you have none)."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Substitutions and Complexities",
                Description = $"Adds Doughnuts and Brownies as possible {References.SpriteCake} Cake forms.\n" +
                $"Also adds Lasagne, Giant Sandwiches, Club Sandwiches, and Giant Sundaes as new mains.\n" +
                $"Also, Burger Patties to be made Fresh with chopped Meat, Spaghetti must be thrown in bins instead of sinks, and all styles of Ice Cream & Sundae flavors " +
                $"must be homemade, even if you take Burgers or Ice Cream/Sundaes in Autumn/Variety.\n" +
                "Provides one additional Mastery Ingredient Provider, one additional Mastery Tray Provider, and Freezers (if you have none).",
                FlavourText = ""
            })
        };
        
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuSaucesSoupsDish>(),
            GDOUtils.GetCastedGDO<Dish, MasteryMenuCoffeeCakeVarietyDish>()
        };
    }
}
