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

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuSaucesSoupsDish : GenericMysteryDishCard
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
            // Add X Mystery Ingredients
            GDOUtils.GetCastedGDO<Item, MysteryOnion>(),
            GDOUtils.GetCastedGDO<Item, MysteryWine>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Water)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of every sauce
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBroccoliCheeseSoupDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySteakSauceMushroomSauceDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySteakSauceRedWineJusDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTurkeyCranberrySauceDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTurkeyGravyDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySpaghettiBologneseDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySpaghettiCheesyDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color>  <i>Any Steak</i> - Mushroom Sauce" +
                ", Red Wine Jus;  <i>Turkey</i> - Gravy, Cranberry Sauce;  <i>Spaghetti</i> - Bolognese, Cheesy Spaghetti\n" +
                "Adds two extra Mystery Providers."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Sauces & Soups",
                Description = "Adds Mushroom Sauce and Red Wine Jus as possible Extras for Steaks, Gravy and Cranberry Sauce " +
                "as possible Extras for Turkey, and Bolognese Sauce and Cheesy Spaghetti as alternative Mains with Spaghetti.\n" +
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
