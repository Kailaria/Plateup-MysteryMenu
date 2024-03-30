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
using KitchenMysteryMenu.Customs.Dishes.HotDog;
using KitchenMysteryMenu.Customs.Dishes.Dumplings;
using KitchenMysteryMenu.Customs.Dishes.StirFry;

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuCondimentsDish : GenericMysteryDishCard
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
            // Add X Mystery Ingredients
            GDOUtils.GetCastedGDO<Item, MysterySoySauce>(),
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
        };

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of every condiment
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryHotdogKetchupDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryHotdogMustardDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryDumplingsSoySauceDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryStirFrySoySauceDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color>  <i>Hot Dogs</i> - Extra Ketchup, Extra Mustard;  " +
                "<i>Dumplings</i> - Soy Sauce;  <i>Stir Fry</i> - Soy Sauce\n" +
                "Adds one extra Mystery Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Condiments",
                Description = "Adds Ketchup and Mustard as possible condiments that customers may request after receiving " +
                "Hot Dogs, and Soy Sauce as a possible condiment for both Dumplings and Stir Fry.\n" +
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
