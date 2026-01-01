using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu;
using KitchenMysteryMenu.Customs.Dishes.Salad;
using KitchenMysteryMenu.Customs.Dishes.Pizza;
using KitchenMysteryMenu.Customs.Dishes.Pies;
using KitchenMysteryMenu.Customs.Ingredients;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KitchenMysteryMenu.Customs.Dishes.Turkey;
using KitchenMysteryMenu.Customs.Dishes.StirFry;
using KitchenMysteryMenu.Customs.Dishes.Steaks;
using KitchenMysteryMenu.Customs.Dishes.Dumplings;
using KitchenMysteryMenu.Customs.Dishes.Burger;
using KitchenMysteryMenu.Customs.Dishes.Breakfast;
using KitchenMysteryMenu.Customs.Dishes.Tacos;
using KitchenMysteryMenu.Customs.Dishes.Sandwiches;

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuToppingsDish : GenericMysteryDishCard
    {
        protected override string NameTag => "Toppings";
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.LargeDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Large;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 5;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mystery Ingredients
            GDOUtils.GetCastedGDO<Item, MysteryMushroom>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of every veggie-like variant
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBreakfastToppingBeansDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBreakfastToppingEggsDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBreakfastToppingMushroomDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBreakfastToppingTomatoDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBurgerToppingCheeseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBurgerToppingOnionDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBurgerToppingTomatoDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryDumplingsSeaweedDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySaladToppingOnionDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySaladToppingOliveDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichCheeseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichEggDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichOliveDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichPickleDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichGiantCheeseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichGiantEggDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichGiantOliveDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichGiantPickleDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichToastCheeseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichToastEggDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichToastOliveDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySandwichToastPickleDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySteakToppingMushroomDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySteakToppingTomatoDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTacosToppingCheeseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTacosToppingLettuceDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTacosToppingOnionDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTacosToppingTomatoDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTurkeyStuffingDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color> Breakfast - Beans, Eggs, Tomato, Mushroom; " +
                "Burger - Cheese, Tomato, Onion; Dumplings - Seaweed; Salad - Onion, Olives; Sandwich - Cheese, Egg, Olive, Pickle; " +
                "Steak - Mushroom, Tomato; Tacos - Cheese, Lettuce, Onion, Tomato; Turkey - Stuffing\n" +
                "Adds one extra Mystery Ingredient Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Toppings",
                Description = "Adds Beans, Eggs, Tomato, and Mushroom as possible Breakfast toppings; Cheese, Tomato, " +
                "and Onion as possible Burger toppings; Mushroom and Tomato as possible Steak toppings; Seaweed as a " +
                "possible topping for Dumplings; Onion and Olives as possible Salad toppings; Cheese, Egg, Olive, and " +
                "Pickle as possible Sandwich toppings; Cheese, Lettuce, Onion, and Tomato " +
                "as possible Tacos toppings; and Stuffing as a possible Turkey topping.\n" +
                "Provides one additional Mystery Ingredient Provider.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
