using KitchenData;
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
    public class MysteryStirFryRiceDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Stir Fry Rice Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.StirFryBase);
        public override DishType Type => DishType.Main;
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
            {
                Locale.English,
                "<color=yellow>Requires ingredients:</color>  <i>Stir Fry in Wok</i>, Rice\n" +
                "Add rice to wok of stir fry ingredients. **Required to make any stir fry**"
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Stir Fry Rice",
                Description = "Adds Rice as a <b>REQUIRED</b> ingredient for Stir Fry.",
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
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.StirFryPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.RiceContainerCooked)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Rice)
        };
        public override bool RequiresVariant => true;
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish) GDOUtils.GetCustomGameDataObject<MysteryStirFryBaseDish>();
    }
}
