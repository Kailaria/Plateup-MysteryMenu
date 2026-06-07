using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Customs.Dishes.Turkey;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Dishes.Desserts
{
    public class MasteryIceCreamVanillaDish : GenericMasteryDish
    {
        protected override string NameTag => "Ice Cream Vanilla";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.IceCream);
        public override DishType Type => DishType.Dessert;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override bool RequiredNoDishItem => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 3;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color> Vanilla Ice Cream\n" +
                "Combine 2 to 3 ice cream scoops from the flavors available, then serve as a dessert."}
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Dessert - Vanilla Ice Cream",
                Description = "Adds vanilla ice cream as a potential ice cream flavor",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new()
        {
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.IceCreamServing),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.IceCreamVanilla)
            }
        };
        public override HashSet<Item> MinimumRequiredMasteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.IceCreamVanilla)
        };
        public override bool RequiresBaseVariant => false;
        public override List<Unlock> HardcodedRequirements => new()
        {
            BaseMasteryDish.GameDataObject
        };
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
        public override GenericMasteryDish BaseMasteryDish => (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasteryIceCreamServingDish>();
        public override HashSet<Item> PreventIngredientReturns => new()
        {
            (Item)GDOUtils.GetExistingGDO(ItemReferences.IceCreamVanilla)
        };
    }
}
