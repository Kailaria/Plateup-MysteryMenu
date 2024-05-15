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

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuCoffeeCakesPiesDish : GenericMysteryDishCard
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
            // Add X Mystery Ingredients (normally requires at minimum: flour, egg, sugar, [flavor])
            GDOUtils.GetCastedGDO<Item, MysterySugar>(),
            GDOUtils.GetCastedGDO<Item, MysteryCookieTray>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.CoffeeCup),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.MixingBowlEmpty)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.FillCoffee),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of Coffee, Coffee-flavored cookies, cupcakes, and sponge cakes, and Pies
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakeBatterRecipe>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesCoffeeCookieDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesCoffeeCupcakeDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesCoffeeSpongeCakeDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeBaseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeCakeStandDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryApplePieDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCherryPieDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryPumpkinPieDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color> <i>Desserts</i> - Cherry Pie, Apple Pie, Pumpkin Pie\n" +
                $"{References.ColorTextHotDrinks} - Coffee, Coffee - Cake Stand (extra)\n" +
                $"{References.ColorTextCakeFlavour} - Coffee\n" +
                $"{References.ColorTextCakeForms} - Cookies, Cupcakes, or Sponge Cake. (Also includes the base recipe of Cake Batter)\n" +
                "Adds one extra Mystery Ingredient Provider and one Mystery Tray Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Coffee Cakes & Pies",
                Description = "Adds Cherry Pie, Apple Pie, Pumpkin Pie, and Coffee as possible desserts, with Cake Stand as a " +
                $"possible extra for any {References.SpriteFillCoffee} Hot Drink.\n" +
                $"Also adds Coffee as a possible {References.SpriteCake} Cake flavour, with the ability to make either Cookies, Cupcakes, or Sponge Cake.\n" +
                "Provides one additional Mystery Ingredient Provider and one Mystery Tray Provider.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
