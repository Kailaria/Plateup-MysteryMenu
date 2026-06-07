using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu;
using KitchenMasteryMenu.Customs.Dishes.Salad;
using KitchenMasteryMenu.Customs.Dishes.Pizza;
using KitchenMasteryMenu.Customs.Dishes.Pies;
using KitchenMasteryMenu.Customs.Ingredients;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KitchenMasteryMenu.Customs.Dishes.Turkey;
using KitchenMasteryMenu.Customs.Dishes.StirFry;
using KitchenMasteryMenu.Customs.Dishes.Steaks;
using KitchenMasteryMenu.Customs.Dishes.Dumplings;
using KitchenMasteryMenu.Customs.Dishes.Burger;
using KitchenMasteryMenu.Customs.Dishes.Breakfast;
using KitchenMasteryMenu.Customs.Dishes.Tacos;
using KitchenMasteryMenu.Customs.Dishes.Sandwiches;

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuToppingsDish : GenericMasteryDishCard
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
            // Add X Mastery Ingredients
            GDOUtils.GetCastedGDO<Item, MasteryMushroom>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMasteryDish> ContainedMasteryRecipes => new()
        {
            // Add the Mastery versions of every veggie-like variant
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBreakfastToppingBeansDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBreakfastToppingEggsDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBreakfastToppingMushroomDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBreakfastToppingTomatoDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBurgerToppingCheeseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBurgerToppingOnionDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBurgerToppingTomatoDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryDumplingsSeaweedDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySaladToppingOnionDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySaladToppingOliveDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichCheeseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichEggDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichOliveDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichPickleDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichGiantCheeseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichGiantEggDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichGiantOliveDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichGiantPickleDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichToastCheeseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichToastEggDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichToastOliveDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichToastPickleDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySteakToppingMushroomDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySteakToppingTomatoDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTacosToppingCheeseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTacosToppingLettuceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTacosToppingOnionDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTacosToppingTomatoDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTurkeyStuffingDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color> Breakfast - Beans, Eggs, Tomato, Mushroom; " +
                "Burger - Cheese, Tomato, Onion; Dumplings - Seaweed; Salad - Onion, Olives; Sandwich - Cheese, Egg, Olive, Pickle; " +
                "Steak - Mushroom, Tomato; Tacos - Cheese, Lettuce, Onion, Tomato; Turkey - Stuffing\n" +
                "Adds one extra Mastery Ingredient Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Toppings",
                Description = "Adds Beans, Eggs, Tomato, and Mushroom as possible Breakfast toppings; Cheese, Tomato, " +
                "and Onion as possible Burger toppings; Mushroom and Tomato as possible Steak toppings; Seaweed as a " +
                "possible topping for Dumplings; Onion and Olives as possible Salad toppings; Cheese, Egg, Olive, and " +
                "Pickle as possible Sandwich toppings; Cheese, Lettuce, Onion, and Tomato " +
                "as possible Tacos toppings; and Stuffing as a possible Turkey topping.\n" +
                "Provides one additional Mastery Ingredient Provider.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuBaseMainsDish>()
        };
    }
}
