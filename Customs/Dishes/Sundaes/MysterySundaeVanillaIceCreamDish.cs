using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Sundaes
{
    public class MysterySundaeVanillaIceCreamDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Sundae - Vanilla Ice Cream ingredient";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SundaeBaseDish);
        public override DishType Type => DishType.Base;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => null;
        public override bool IsMainThatDoesNotNeedPlates => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color>  <i>Sundae</i>, Vanilla Ice Cream\n" + 
                "Serve when requested to as part of a Sundae dessert." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Sundae - Vanilla Ice Cream",
                Description = "Adds Vanilla Ice Cream as an ingredient for all types of Sundaes when " +
                    "it is present with Sundae Glasses.",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override List<Unlock> HardcodedRequirements => new List<Unlock>()
        {
            BaseMysteryDish.GameDataObject
        };

        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        {
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(References.SundaePiecemeal),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.IceCreamVanilla)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.IceCreamVanilla),
            (Item) GDOUtils.GetExistingGDO(References.SundaeGlass)
        };
        public override bool RequiresBaseVariant => false;
        public override HashSet<Item> PreventIngredientReturns => new()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.IceCreamVanilla)
        };
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish) GDOUtils.GetCustomGameDataObject<MysterySundaeBaseDish>();
    }
}
