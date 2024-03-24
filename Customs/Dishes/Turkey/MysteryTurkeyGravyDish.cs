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
    public class MysteryTurkeyGravyDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Turkey Gravy Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.TurkeyGravy);
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
                "<color=yellow>Requires ingredient:</color> Onion, Turkey\n" + 
                "Cook onion in a pot of water to make broth. Add a turkey carcass to the broth and cook again.\n" +
                "Plate with  <i>Turkey</i>." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Turkey - Gravy",
                Description = "Adds Gravy as an option for Turkey",
                FlavourText = "(placeholder card, do not add with Cards Manager)"
            })
        };
        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        {
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.TurkeyPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.TurkeyGravy)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Onion),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.TurkeyIngredient),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuSaucesDish>()
        };
    }
}
