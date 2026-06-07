using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Dishes.Tacos
{
    public class MasteryTacosToppingCheeseDish : GenericMasteryDish
    {
        protected override string NameTag => "Tacos - Cheese";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.TacosCheese);
        public override DishType Type => DishType.Extra;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override bool RequiredNoDishItem => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 2;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredient:</color> Cheese\n" + 
                "Chop cheese. Combine with Taco Tray of Tacos." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Tacos - Cheese",
                Description = "Adds Cheese as a topping for Tacos",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        {
            new Dish.IngredientUnlock()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.TacoIndividual),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.CheeseGrated)
            },
        };
        public override HashSet<Item> MinimumRequiredMasteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Cheese),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuToppingsDish>()
        };
        public override GenericMasteryDish BaseMasteryDish => (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryTacosBaseDish>();
    }
}
