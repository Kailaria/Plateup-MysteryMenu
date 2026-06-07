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
using KitchenMasteryMenu.Customs.Dishes.HotDog;
using KitchenMasteryMenu.Customs.Dishes.Dumplings;
using KitchenMasteryMenu.Customs.Dishes.StirFry;

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuCondimentsDish : GenericMasteryDishCard
    {
        protected override string NameTag => "Condiments";
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 2;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mastery Ingredients
            GDOUtils.GetCastedGDO<Item, MasterySoySauce>(),
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
        };

        public override HashSet<GenericMasteryDish> ContainedMasteryRecipes => new()
        {
            // Add the Mastery versions of every condiment
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryHotdogKetchupDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryHotdogMustardDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryDumplingsSoySauceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryStirFrySoySauceDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color>  <i>Hot Dogs</i> - Extra Ketchup, Extra Mustard.  " +
                "<i>Dumplings</i> - Soy Sauce.  <i>Stir Fry</i> - Soy Sauce\n" +
                "Adds one extra Mastery Ingredient Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Condiments",
                Description = "Adds Ketchup and Mustard as possible condiments that customers may request after receiving " +
                "Hot Dogs, and Soy Sauce as a possible condiment for both Dumplings and Stir Fry.\n" +
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
