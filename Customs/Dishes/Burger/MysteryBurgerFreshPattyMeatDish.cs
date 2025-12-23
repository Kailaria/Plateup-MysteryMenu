using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using MysteryMenu.Customs.Dishes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Burger
{
    public class MysteryBurgerFreshPattyMeatDish : GenericMysteryDish
    {
        protected override string NameTag => "Burger - Fresh Patty (Meat)";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.BurgerFreshPatties);
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredient:</color> Meat, Eggs\n" + 
                "Chop Meat. Chop Egg. Combine to form Burger Patty." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Burger - Fresh Patties (Meat)",
                Description = "Substitutes Burger Patties for Meat and Egg",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        //public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        //{
        //    new Dish.IngredientUnlock()
        //    {
        //        MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.BurgerPlated),
        //        Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.TomatoChopped)
        //    },
        //};
        //public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        //{
        //    (Item) GDOUtils.GetExistingGDO(ItemReferences.Tomato),
        //};
        public override HashSet<SubstitutionIngredientSet> SubstitutionIngredientSets => new()
        {
            new SubstitutionIngredientSet(
                GDOUtils.GetExistingGDO(ItemReferences.BurgerPattyRaw) as Item,
                new HashSet<Item>()
                {
                    GDOUtils.GetExistingGDO(ItemReferences.Meat) as Item,
                    GDOUtils.GetExistingGDO(ItemReferences.Egg) as Item
                })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuSubstitutionsComplexityDish>()
        };
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBurgerBaseDish>();
    }
}
