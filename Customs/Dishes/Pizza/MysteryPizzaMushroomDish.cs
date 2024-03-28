using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Pizza
{
    public class MysteryPizzaMushroomDish : GenericMysteryDish
    {
        protected override string NameTag => "Pizza - Mushroom";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.PizzaMushroom);
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color> Flour, Tomato, Cheese, Oil, Mushroom\n" +
                "Knead (or add water to) flour to make dough, then combine with oil to make Pizza Crust. " +
                "Chop a tomato twice to make tomato sauce. Chop cheese. Chop mushroom.\n" +
                "Combine the pizza crust with the sauce, cheese, and mushroom, then cook. Portions 4 slices."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Pizza - Mushroom",
                Description = "Adds mushroom pizza as a main when <b>Flour</b>, <b>Oil</b>, <b>Tomato</b>, " +
                    "<b>Cheese</b>, and <b>Mushroom</b> are present",
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
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.PizzaPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.MushroomCookedWrapped)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Oil),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Tomato),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Cheese),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Mushroom),
        };
        public override bool RequiresVariant => false;
        public override GenericMysteryDish BaseMysteryDish => (GenericMysteryDish) GDOUtils.GetCustomGameDataObject<MysteryPizzaBaseDish>();
    }
}
