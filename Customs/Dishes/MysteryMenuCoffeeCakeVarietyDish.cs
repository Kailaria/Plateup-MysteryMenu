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
    public class MysteryMenuCoffeeCakeVarietyDish : GenericMysteryDishCard
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
            // Add X Mystery Ingredients (normally requires at minimum: flour, egg, sugar, [flavor])
            GDOUtils.GetCastedGDO<Item, MysteryChocolate>(),
            GDOUtils.GetCastedGDO<Item, MysteryTeapot>(),
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

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of Coffee, Coffee-flavored cookies, cupcakes, and sponge cakes, and Pies
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesChocolateCookieDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesChocolateCupcakeDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesChocolateSpongeCakeDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesLemonCookieDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesLemonCupcakeDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCakesLemonSpongeCakeDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeExtraMilkDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeExtraSugarDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeIcedDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryCoffeeLatteDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTeaDish>()
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                $"<color=#00ffff>New possible menu items:</color> {References.ColorTextCakeFlavours} - Chocolate, Lemon\n" +
                $"{References.ColorTextHotDrinks} - Lattes, Iced Coffee, Tea, Extra Milk, Extra Sugar\n" +
                "Adds two extra Mystery Ingredient Providers."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Coffee & Cake - Varieties",
                Description = $"Adds Chocolate and Lemon as possible {References.SpriteCake} Cake flavours.\n" +
                $"Also adds Lattes, Iced Coffee, and Tea as alternative {References.SpriteFillCoffee} Hot Drinks, and Extra Sugar and Extra Milk for them when available.\n" +
                "Provides two additional Mystery Ingredient Providers.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakesPiesDish>()
        };
    }
}
