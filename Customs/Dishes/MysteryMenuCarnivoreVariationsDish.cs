using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu;
using KitchenMysteryMenu.Customs.Dishes.Fish;
using KitchenMysteryMenu.Customs.Dishes.Steaks;
using KitchenMysteryMenu.Customs.Dishes.StirFry;
using KitchenMysteryMenu.Customs.Ingredients;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuCarnivoreVariationsDish : GenericMysteryDishCard
    {
        protected override string NameTag => "Carnivorous Variety";
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mystery Ingredients
            GDOUtils.GetCastedGDO<Item, MysterySurfNTurf>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of every steak & fish variant plus steak stir fry
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySteakThinDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySteakThickDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySteakBonedDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryFishCrabCakeDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryFishFilletDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryFishOysterDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryFishSpinyDish>(),

            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryStirFrySteakDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color> Thin-Cut Steak, Thick-Cut Steak, Bone-In Steak, " +
                "Spiny Fish, Oysters, Fish Fillet, Crab Cakes, Steak Stir Fry\n" +
                "Adds one extra Mystery Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Carnivorous Variations",
                Description = "Adds Thin Steak, Thick Steak, Bone-in Steak, Spiny Fish, Fish Fillet, Oysters, Crab Cakes, " +
                "and Steak Stir Fry as possible Mains. Provides one additional Mystery Ingredient Provider.",
                FlavourText = "Also known as: Surf & Turf"
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
