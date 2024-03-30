using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Salad
{
    public class MysterySaladTomatoDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Salad Tomato Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.SaladBase);
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 1;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color>  <i>Salad</i>, Tomato\n" + 
                "Chop tomato once and serve when ordered with salad." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery Salad - Tomato Topping",
                Description = "Adds chopped tomato as a topping option when <b>Tomato</b> is present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        { 
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup) GDOUtils.GetExistingGDO(ItemGroupReferences.SaladPlated),
                Ingredient = (Item) GDOUtils.GetExistingGDO(ItemReferences.TomatoChopped)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Tomato)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            BaseMysteryDish.GameDataObject
        };
        public override GenericMysteryDish BaseMysteryDish => 
            (GenericMysteryDish) GDOUtils.GetCustomGameDataObject<MysterySaladBaseDish>();
    }
}
