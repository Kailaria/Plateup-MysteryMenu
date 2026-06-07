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

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuVeggieVariationsDish : GenericMasteryDishCard
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
            // Add X Mastery Ingredients
            GDOUtils.GetCastedGDO<Item, MasteryApple>(),
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
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryPiesMushroomDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryPiesVegetableDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryPizzaMushroomDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryPizzaOnionDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySaladAppleDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySaladPotatoDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryStirFryBambooDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryStirFryMushroomDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryNutRoastDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color> Apple Salad, Potato Salad, Mushroom Pie, " +
                "Veggie Pie, Nut Roast, Onion Pizza, Mushroom Pizza, Bamboo Stir Fry, Mushroom Stir Fry\n" +
                "Adds two extra Mastery Ingredient Providers."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Vegetarian Variations",
                Description = "Adds Apple Salad, Potato Salad, Mushroom Pie, Veggie Pie, Nut Roast, Onion Pizza, Mushroom Pizza, Bamboo " +
                "Stir Fry, and Mushroom Stir Fry as possible Mains.\n" +
                "Provides two additional Mastery Ingredient Providers.",
                FlavourText = ""
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuBaseMainsDish>()
        };
    }
}
