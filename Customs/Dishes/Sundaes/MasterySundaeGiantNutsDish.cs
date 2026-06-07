using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Dishes.Sundaes
{
    public class MasterySundaeGiantNutsDish : GenericMasteryDish
    {
        protected override string NameTag => "Mastery Sundae Giant - Nuts topping";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SundaeToppingsDish);
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => null;
        public override bool IsMainThatDoesNotNeedPlates => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override Dictionary<Locale, string> Recipe => new()
        {};
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Giant Sundae - Nuts topping",
                Description = "Adds Nuts as an ingredient for all types of Sundaes when " +
                    "they are present with Sundae Glasses and an ice cream flavor.",
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
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(References.SundaeGiantPiecemeal),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.NutsChopped)
            }
        };
        public override HashSet<Item> MinimumRequiredMasteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.NutsIngredient)
        };
        public override bool RequiresBaseVariant => true;
        public override GenericMasteryDish BaseMasteryDish => (GenericMasteryDish) GDOUtils.GetCustomGameDataObject<MasterySundaeBaseDish>();
    }
}
