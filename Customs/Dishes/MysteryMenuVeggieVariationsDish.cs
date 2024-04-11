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

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuVeggieVariationsDish : GenericMysteryDishCard
    {
        protected override string NameTag => "Vegetarian Variety";
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.LargeDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Large;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 4;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mystery Ingredients
            GDOUtils.GetCastedGDO<Item, MysteryApple>(),
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
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryPiesMushroomDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryPiesVegetableDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryPizzaMushroomDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryPizzaOnionDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySaladAppleDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySaladPotatoDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryStirFryBambooDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryStirFryMushroomDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryNutRoastDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color> Apple Salad, Potato Salad, Mushroom Pie, " +
                "Veggie Pie, Nut Roast, Onion Pizza, Mushroom Pizza, Bamboo Stir Fry, Mushroom Stir Fry\n" +
                "Adds two extra Mystery Ingredient Providers."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Vegetarian Variations",
                Description = "Adds Apple Salad, Potato Salad, Mushroom Pie, Veggie Pie, Nut Roast, Onion Pizza, Mushroom Pizza, Bamboo " +
                "Stir Fry, and Mushroom Stir Fry as possible Mains.\n" +
                "Provides two additional Mystery Ingredient Providers.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
