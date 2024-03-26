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

namespace KitchenMysteryMenu.Customs.Dishes.HotDog
{
    public class MysteryHotdogMustardDish : GenericMysteryDish
    {
        protected override string NameTag => "Hot Dog Mustard";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.HotdogCondimentMustard);
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
                "<color=yellow>Requires ingredient:</color> Mustard\n" + 
                "After serving a  <i>Hot Dog</i>, customers may request Mustard as an extra if available." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Hot Dog - Extra Mustard",
                Description = "Adds Mustard as a possible extra after serving a Hot Dog.",
                FlavourText = "(alsoAddsRecipes card, do not add with Cards Manager)"
            })
        };
        public override HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks => new()
        {
            new()
            {
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.HotdogPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.CondimentMustard)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.CondimentMustard),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCondimentsDish>()
        };
        public override bool RequiresVariant => true;
    }
}
