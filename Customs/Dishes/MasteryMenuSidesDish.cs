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
using KitchenMasteryMenu.Customs.Dishes.Desserts;
using KitchenMasteryMenu.Customs.Dishes.Sides;

namespace KitchenMasteryMenu.Customs.Dishes
{
    public class MasteryMenuSidesDish : GenericMasteryDishCard
    {
        protected override string NameTag => "Sides";
        public override DishType Type => DishType.Side;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.LargeDecrease;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => true;
        public override bool RequiredNoDishItem => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override HashSet<Item> MinimumIngredients => new()
        {
            // Add X Mastery Ingredients
            GDOUtils.GetCastedGDO<Item, MasteryPotato>(),
            (Item)GDOUtils.GetExistingGDO(ItemReferences.Pot)
        };
        public override HashSet<Process> RequiredProcesses => new()
        {
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.Chop),
            (Process)GDOUtils.GetExistingGDO(ProcessReferences.RequireOven)
        };

        public override HashSet<GenericMasteryDish> ContainedMasteryRecipes => new()
        {
            // Add the Mastery versions of all sides
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySideBambooDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySideBroccoliDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySideChipsDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySideCornOnCobDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySideMashedPotatoDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySideOnionRingsDish>(),
            (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySideRoastPotatoDish>(),
        };

        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "<color=#00ffff>New possible menu items:</color>  <i>Sides</i> - Bamboo, Broccoli, " +
                "Chips, Corn on the Cob, Mashed Potato, Onion Rings, Roast Potato\n" +
                "Adds one extra Mastery Ingredient Provider."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Sides",
                Description = "Adds Bamboo, Broccoli, Chips, Corn on the Cob, Mashed Potato, Onion Rings, and Roast Potato " +
                "as possible sides.\n" +
                "Provides one additional Mastery Ingredient Provider.",
                FlavourText = "Better hope for Metal Tables before Potatoes show up as a daily ingredient!"
            })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuBaseMainsDish>()
        };
    }
}
