using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu;
using KitchenMasteryMenu.Customs.Dishes.Fish;
using KitchenMasteryMenu.Customs.Dishes.Sandwiches;
using KitchenMasteryMenu.Customs.Dishes.Steaks;
using KitchenMasteryMenu.Customs.Dishes.StirFry;
using KitchenMasteryMenu.Customs.Ingredients;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuCarnivoreVariationsDish : GenericMasteryDishCard
    {
        protected override string NameTag => "Carnivorous Variety";
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.LargeDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mastery Ingredients
            GDOUtils.GetCastedGDO<Item, MasterySurfNTurf>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMasteryDish> ContainedMasteryRecipes => new()
        {
            // Add the Mastery versions of every steak & fish variant plus steak stir fry and Toast Sandwiches
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySteakThinDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySteakThickDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySteakBonedDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryFishCrabCakeDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryFishFilletDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryFishOysterDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryFishSpinyDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryStirFrySteakDish>(),

            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichToastDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichToastHamSliceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichToastLettuceDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySandwichToastTomatoDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color> Thin-Cut Steak, Thick-Cut Steak, Bone-In Steak, " +
                "Spiny Fish, Oysters, Fish Fillet, Crab Cakes, Steak Stir Fry, Toast Sandwich\n" +
                "Adds one extra Mastery Ingredient Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Carnivorous Variations",
                Description = "Adds Thin Steak, Thick Steak, Bone-in Steak, Spiny Fish, Fish Fillet, Oysters, Crab Cakes, " +
                "Steak Stir Fry, and Toast Sandwiches as possible Mains. Provides one additional Mastery Ingredient Provider.",
                FlavourText = "Also known as: Surf & Turf"
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuBaseMainsDish>()
        };
    }
}
