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

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuCoffeeCakesPiesDish : GenericMasteryDishCard
    {
        protected override string NameTag => "Coffee Cakes and Pies";
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
            // Add X Mastery Ingredients (normally requires at minimum: flour, egg, sugar, [flavor])
            GDOUtils.GetCastedGDO<Item, MasterySugar>(),
            GDOUtils.GetCastedGDO<Item, MasteryCookieTray>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.CoffeeCup),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.MixingBowlEmpty)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.FillCoffee),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMasteryDish> ContainedMasteryRecipes => new()
        {
            // Add the Mastery versions of Coffee, Coffee-flavored cookies, cupcakes, and sponge cakes, and Pies
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakeBatterRecipe>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesCoffeeCookieDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesCoffeeCupcakeDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCakesCoffeeSpongeCakeDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCoffeeBaseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCoffeeCakeStandDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryApplePieDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCherryPieDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryPumpkinPieDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color> <i>Desserts</i> - Cherry Pie, Apple Pie, Pumpkin Pie\n" +
                $"{References.ColorTextHotDrinks} - Coffee, Coffee - Cake Stand (extra)\n" +
                $"{References.ColorTextCakeFlavour} - Coffee\n" +
                $"{References.ColorTextCakeForms} - Cookies, Cupcakes, or Sponge Cake. (Also includes the base recipe of Cake Batter)\n" +
                "Adds one extra Mastery Ingredient Provider and one Mastery Tray Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Coffee Cakes & Pies",
                Description = "Adds Cherry Pie, Apple Pie, Pumpkin Pie, and Coffee as possible desserts, with Cake Stand as a " +
                $"possible extra for any {References.SpriteFillCoffee} Hot Drink.\n" +
                $"Also adds Coffee as a possible {References.SpriteCake} Cake flavour, with the ability to make either Cookies, Cupcakes, or Sponge Cake.\n" +
                "Provides one additional Mastery Ingredient Provider and one Mastery Tray Provider.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuBaseMainsDish>()
        };
    }
}
