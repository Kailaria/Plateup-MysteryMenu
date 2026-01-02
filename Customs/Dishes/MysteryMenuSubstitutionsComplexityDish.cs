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
using KitchenMysteryMenu.Customs.Dishes.Sandwiches;
using KitchenMysteryMenu.Customs.Dishes.Sundaes;

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
            GDOUtils.GetCastedGDO<Item, MysteryDoughnutTray>()
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

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of Lasagne, Doughnuts, Brownies, Fresh Burger Patties (Meat)
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBurgerFreshPattyMeatDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySpaghettiStarchyDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySpaghettiLasagneDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesChocolateBrownieDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesChocolateDoughnutDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesCoffeeDoughnutDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesLemonDoughnutDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichClubDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichGiantDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichGiantHamSliceDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichGiantLettuceDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichGiantTomatoDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryHomemadeChocolateIceCreamDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryHomemadeStrawberryIceCreamDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryHomemadeVanillaIceCreamDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySundaeGiantDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySundaeGiantCherryDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySundaeGiantChocolateIceCreamDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySundaeGiantChocolateSyrupDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySundaeGiantGlassDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySundaeGiantNutsDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySundaeGiantStrawberryIceCreamDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySundaeGiantStrawberrySyrupDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySundaeGiantVanillaIceCreamDish>()
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                $"<color=#00ffff>New possible menu items:</color> {References.ColorTextCakeForms} - Doughnuts, Brownies\n" +
                "Mains - Lasagne, Giant Sandwiches, Club Sandwiches, Giant Sundaes\n" +
                "Substitute recipes - Fresh Burger Patties, Starchy Spaghetti, Homemade Ice Cream & Sundaes (and blocks related substituted items' providers in Autumn)\n" +
                "Adds one extra Mystery Ingredient Provider, one extra Mystery Tray Provider, and Freezers (if you have none)."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Substitutions and Complexities",
                Description = $"Adds Doughnuts and Brownies as possible {References.SpriteCake} Cake forms.\n" +
                $"Also adds Lasagne, Giant Sandwiches, Club Sandwiches, and Giant Sundaes as new mains.\n" +
                $"Also, Burger Patties to be made Fresh with chopped Meat, Spaghetti must be thrown in bins instead of sinks, and all styles of Ice Cream & Sundae flavors " +
                $"must be homemade, even if you take Burgers or Ice Cream/Sundaes in Autumn/Variety.\n" +
                "Provides one additional Mystery Ingredient Provider, one additional Mystery Tray Provider, and Freezers (if you have none).",
                FlavourText = ""
            })
        };
        
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuSaucesSoupsDish>(),
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakeVarietyDish>()
        };
    }
}
