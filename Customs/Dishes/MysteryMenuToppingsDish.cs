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

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuToppingsDish : GenericMysteryDishCard
    {
        protected override string NameTag => "Toppings";
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 4;
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

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySteakToppingMushroomDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySteakToppingTomatoDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTurkeyStuffingDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color> Breakfast - Beans, Eggs, Tomato, Mushroom;" +
                " Burger - Cheese, Tomato, Onion; Dumplings - Seaweed; Salad - Onion, Olives; Turkey - Stuffing\n" +
                "Adds one extra Mystery Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Toppings",
                Description = "Adds Beans, Eggs, Tomato, and Mushroom as possible Breakfast toppings; Cheese, Tomato, " +
                "and Onion as possible Burger toppings; Mushroom and Tomato as possible Steak toppings; Seaweed as a " +
                "possible topping for Dumplings; Onion and Olives as possible Salad toppings; and Stuffing as a possible " +
                "Turkey topping.\n" +
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
