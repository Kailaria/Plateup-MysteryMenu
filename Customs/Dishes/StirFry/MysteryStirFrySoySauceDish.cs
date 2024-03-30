using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.StirFry
{
    public class MysteryStirFrySoySauceDish : GenericMysteryDish
    {
        protected override string NameTag => "Stir Fry - Soy Sauce";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.StirFrySoySauce);
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 2;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredient:</color> Soy Sauce\n" + 
                "After serving  <i>Stir Fry</i>, customers may request Soy Sauce as an extra if available." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Stir Fry - Soy Sauce",
                Description = "Adds Soy Sauce as a possible extra after serving Dumplings.",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks => new()
        {
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.StirFryPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.CondimentSoySauce)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.CondimentSoySauce),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCondimentsDish>()
        };
    }
}
