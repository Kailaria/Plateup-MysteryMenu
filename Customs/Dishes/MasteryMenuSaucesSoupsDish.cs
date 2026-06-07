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
using KitchenMasteryMenu.Customs.Dishes.Sandwiches;

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuSaucesSoupsDish : GenericMasteryDishCard
    {
        protected override string NameTag => "Sauces and Soups";
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.LargeDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 4;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mastery Ingredients
            GDOUtils.GetCastedGDO<Item, MasteryOnion>(),
            GDOUtils.GetCastedGDO<Item, MasteryWine>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Water)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMasteryDish> ContainedMasteryRecipes => new()
        {
            // Add the Mastery versions of every sauce
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryBroccoliCheeseSoupDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryCarrotSoupDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryMeatSoupDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryPumpkinSoupDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTomatoSoupDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySteakSauceMushroomSauceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySteakSauceRedWineJusDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTurkeyCranberrySauceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTurkeyGravyDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySpaghettiBologneseDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySpaghettiCheesyDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichMayoDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichGiantMayoDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichToastMayoDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color>  <i>Any Steak</i> - Mushroom Sauce" +
                ", Red Wine Jus --  <i>Turkey</i> - Gravy, Cranberry Sauce --  <i>Spaghetti</i> - Bolognese, Cheesy Spaghetti --  <i>Sandwiches</i> - Mayo\n" +
                "<i>Starters</i> - Broccoli Cheese Soup, Carrot Soup, Meat Soup, Pumpkin Soup, Tomato Soup\n" +
                "Adds two extra Mastery Ingredient Providers."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Sauces & Soups",
                Description = "Adds Mushroom Sauce and Red Wine Jus as possible Extras for Steaks, Gravy and Cranberry Sauce " +
                "as possible Extras for Turkey, Mayo as a possible Extra for Sandwiches, and Bolognese Sauce and Cheesy Spaghetti as alternative Mains with Spaghetti.\n" +
                "Also adds Broccoli Cheese Soup, Carrot Soup, Meat Soup, Pumpkin Soup, and Tomato Soup as possible starters.\n" +
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
