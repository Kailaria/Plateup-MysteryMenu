using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Pies
{
    public class MysteryPiesMushroomDish : GenericMysteryDish
    {
        protected override string NameTag => "Pies - Mushroom";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.PieMushroom);
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
                "<color=yellow>Requires ingredients:</color> Flour, Mushroom\n" + 
                "Knead flour (or add water) to make dough, then knead into pie crust. Add mushroom, then cook." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Pies - Mushroom",
                Description = "Adds mushroom pie as a main when <b>Flour</b> and <b>Mushroom</b> are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override List<Unlock> HardcodedRequirements => new List<Unlock>()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuVeggieVariationsDish>()
        };

        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        {
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.PiePlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.PieMushroomCooked)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Mushroom),
        };
        public override bool RequiresVariant => false;
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish) GDOUtils.GetCustomGameDataObject<MysteryPiesBaseDish>();
    }
}
