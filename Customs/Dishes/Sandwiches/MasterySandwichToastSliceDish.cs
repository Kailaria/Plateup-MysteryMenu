using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Dishes.Sandwiches
{
    public class MasterySandwichToastSliceDish : GenericMasteryDish
    {
        protected override string NameTag => "Mastery Sandwich - Toast Slice";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SandwichToastDish);
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => null;
        public override bool IsMainThatDoesNotNeedPlates => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 4;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color>  <i>Flour</i>\n" +
                "Knead flour once (or add water) and cook to make Bread. Portion and cook a slice of bread to make Toast.\n" +
                "Serve when requested to start/finish with sandwich fillings." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Sandwich Toast",
                Description = "Adds <b>Toast</b> as an ingredient and type of sandwich when it's present with one other ingredient",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override List<Unlock> HardcodedRequirements => new List<Unlock>()
        {
            BaseMasteryDish.GameDataObject
        };

        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        {
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(References.SandwichToastPiecemeal),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.BreadToast)
            }
        };
        public override HashSet<Item> MinimumRequiredMasteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour)
        };
        public override bool RequiresBaseVariant => true;
        public override GenericMasteryDish BaseMasteryDish => (GenericMasteryDish) GDOUtils.GetCustomGameDataObject<MasterySandwichBaseDish>();
    }
}
