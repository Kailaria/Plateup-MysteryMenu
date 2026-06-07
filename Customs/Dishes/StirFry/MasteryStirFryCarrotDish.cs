using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Dishes.StirFry
{
    public class MasteryStirFryCarrotDish : GenericMasteryDish
    {
        protected override string NameTag => "Mastery Stir Fry Carrot Dish";
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
            { Locale.English,
                "<color=yellow>Requires ingredients:</color>  <i>Stir Fry in Wok</i>, Carrot\n" + 
                "Chop carrot and add to wok of stir fry ingredients." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Stir Fry Carrot",
                Description = "Adds <b>Carrot</b> as an ingredient for Stir Fry when it's present with <b>Rice</b>",
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
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.StirFryPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.CarrotChoppedContainerCooked)
            }
        };
        public override HashSet<Item> MinimumRequiredMasteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Carrot),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Rice)
        };
        public override bool RequiresBaseVariant => false;
        public override GenericMasteryDish BaseMasteryDish => (GenericMasteryDish) GDOUtils.GetCustomGameDataObject<MasteryStirFryBaseDish>();
    }
}
