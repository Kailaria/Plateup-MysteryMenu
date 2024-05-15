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

namespace KitchenMysteryMenu.Customs.Dishes.Turkey
{
    public class MysteryTurkeyCranberrySauceDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Turkey Cranberry Sauce Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.TurkeyCranberrySauce);
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
                "<color=yellow>Requires ingredient:</color> Cranberries, Sugar\n" + 
                "Chop cranberries, then combine with sugar to make sauce.\n" +
                "Plate with  <i>Turkey</i>." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Turkey - Cranberry Sauce",
                Description = "Adds Cranberry Sauce as an option for Turkey",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        {
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.TurkeyPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.CranberrySauce)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Cranberries),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Sugar),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuSaucesSoupsDish>()
        };
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryTurkeyBaseDish>();
        public override MenuPhase MenuPhase => BaseMysteryDish.MenuPhase;
    }
}
