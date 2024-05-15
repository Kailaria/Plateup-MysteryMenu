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

namespace KitchenMysteryMenu.Customs.Dishes.Breakfast
{
    public class MysteryBreakfastToppingTomatoDish : GenericMysteryDish
    {
        protected override string NameTag => "Breakfast - Tomato";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.BreakfastVeganExtras);
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
                "<color=yellow>Requires ingredient:</color> Tomato\n" + 
                "Chop tomato once. Plate with  <i>Breakfast</i>." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Breakfast - Tomato",
                Description = "Adds Tomato as a topping for Breakfast",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        {
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.BreakfastPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.TomatoChopped)
            },
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Tomato),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuToppingsDish>()
        };
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryBreakfastBaseDish>();
    }
}
