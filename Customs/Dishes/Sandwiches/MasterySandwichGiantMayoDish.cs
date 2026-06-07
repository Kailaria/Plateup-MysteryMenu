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
    public class MasterySandwichGiantMayoDish : GenericMasteryDish
    {
        protected override string NameTag => "Mastery Sandwich Giant - Mayo filling";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SandwichMayoDish);
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => null;
        public override bool IsMainThatDoesNotNeedPlates => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 2;
        public override Dictionary<Locale, string> Recipe => new()
        {};
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Sandwich Giant - Mayo Extra",
                Description = "Adds <b>Mayo</b> (Egg + Oil) as an ingredient for all types of Sandwiches when it's present with <b>FLour</b> and " +
                "one other sandwich ingredient.",
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
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(References.SandwichGiantPiecemeal),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.Mayonnaise)
            }
        };
        public override HashSet<Item> MinimumRequiredMasteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Egg),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Oil)
        };
        public override bool RequiresBaseVariant => true;
        public override GenericMasteryDish BaseMasteryDish => (GenericMasteryDish) GDOUtils.GetCustomGameDataObject<MasterySandwichBaseDish>();
    }
}
