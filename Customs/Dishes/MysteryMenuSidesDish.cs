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
using KitchenMysteryMenu.Customs.Dishes.Desserts;
using KitchenMysteryMenu.Customs.Dishes.Sides;

namespace KitchenMysteryMenu.Customs.Dishes
{
    public class MysteryMenuSidesDish : GenericMysteryDishCard
    {
        protected override string NameTag => "Sides";
        public override DishType Type => DishType.Side;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override bool RequiredNoDishItem => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mystery Ingredients
            GDOUtils.GetCastedGDO<Item, MysteryPotato>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMysteryDish> ContainedMysteryRecipes => new()
        {
            // Add the Mystery versions of all sides
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySideBambooDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySideBroccoliDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySideChipsDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySideCornOnCobDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySideMashedPotatoDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySideOnionRingsDish>(),
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysterySideRoastPotatoDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color>  <i>Sides</i> - Bamboo, Broccoli, " +
                "Chips, Corn on the Cob, Mashed Potato, Onion Rings, Roast Potato\n" +
                "Adds one extra Mystery Ingredient Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Sides",
                Description = "Adds Bamboo, Broccoli, Chips, Corn on the Cob, Mashed Potato, Onion Rings, and Roast Potato " +
                "as possible sides.\n" +
                "Provides one additional Mystery Ingredient Provider.",
                FlavourText = "Better hope for Metal Tables before Potatoes show up as a daily ingredient!"
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
