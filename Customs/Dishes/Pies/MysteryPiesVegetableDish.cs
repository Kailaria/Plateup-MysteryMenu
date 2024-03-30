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
    public class MysteryPiesVegetableDish : GenericMysteryDish
    {
        protected override string NameTag => "Pies - Vegetable";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.PieVegetable);
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
                "<color=yellow>Requires ingredients:</color> Flour, Carrot, Broccoli\n" + 
                "Knead flour (or add water) to make dough, then knead into pie crust. Add carrots and broccoli, then cook." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Pies - Vegetable",
                Description = "Adds vegetable pie as a main when <b>Flour</b>, <b>Carrots</b>, and <b>Broccoli</b> are present",
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
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.PieVegetableCooked)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Carrot),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.BroccoliRaw),
        };
        public override bool RequiresVariant => false;
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish) GDOUtils.GetCustomGameDataObject<MysteryPiesBaseDish>();
    }
}
