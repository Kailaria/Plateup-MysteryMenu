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
using KitchenMasteryMenu.Customs.Dishes.Sundaes;

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuCoffeeCakeVarietyDish : GenericMasteryDishCard
    {
        protected override string NameTag => "Coffee Cake Variety";
        public override DishType Type => DishType.Dessert;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.LargeDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override bool RequiredNoDishItem => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mastery Ingredients (normally requires at minimum: flour, egg, sugar, [flavor])
            GDOUtils.GetCastedGDO<Item, MasteryChocolate>(),
            GDOUtils.GetCastedGDO<Item, MasteryTeapot>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.MixingBowlEmpty),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Ice)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.FillCoffee),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.FrothMilk),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.SteepTea)
        };

        public override HashSet<GenericMasteryDish> ContainedMasteryRecipes => new()
        {
            // Add the Mastery versions of Coffee, Coffee-flavored cookies, cupcakes, and sponge cakes, and Pies
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesChocolateCookieDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesChocolateCupcakeDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesChocolateSpongeCakeDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesLemonCookieDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesLemonCupcakeDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesLemonSpongeCakeDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCoffeeExtraMilkDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCoffeeExtraSugarDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCoffeeIcedDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCoffeeLatteDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTeaDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeCherryDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeChocolateSyrupDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeNutsDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeStrawberrySyrupDish>()
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                $"<color=#00ffff>New possible menu items:</color> {References.ColorTextCakeFlavours} - Chocolate, Lemon\n" +
                $"{References.ColorTextHotDrinks} - Lattes, Iced Coffee, Tea, Extra Milk, Extra Sugar\n" +
                $"Sundaes - Cherries, Nuts, Chocolate Syrup, Strawberry Syrup\n" +
                "Adds two extra Mastery Ingredient Providers."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Coffee, Cake, Sundae - Varieties",
                Description = $"Adds Chocolate and Lemon as possible {References.SpriteCake} Cake flavours.\n" +
                $"Also adds Lattes, Iced Coffee, and Tea as alternative {References.SpriteFillCoffee} Hot Drinks, and Extra Sugar and Extra Milk for them when available.\n" +
                $"Finally, adds Cherries, Nuts, Chocolate Syrup, and Strawberry Syrup as possible Sundae toppings." +
                "Provides two additional Mastery Ingredient Providers.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuCoffeeCakesPiesDish>(),
            GDOUtils.GetCastedGDO<Dish, MasteryMenuBoardsTreatsDish>()
        };
    }
}
