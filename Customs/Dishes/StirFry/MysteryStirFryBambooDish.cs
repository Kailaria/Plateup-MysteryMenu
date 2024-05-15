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
    public class MysteryStirFryBambooDish : GenericMysteryDish
    {
        protected override string NameTag => "Stir Fry - Bamboo Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.StirFryBamboo);
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
                "<color=yellow>Requires ingredients:</color>  <i>Stir Fry in Wok</i>, Bamboo\n" + 
                "Add raw bamboo to pot with water and boil. Portion and add to wok of stir fry ingredients." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Stir Fry Bamboo",
                Description = "Adds <b>Bamboo</b> as an ingredient for Stir Fry when it's present with <b>Rice</b>",
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
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(ItemReferences.StirFryPlated),
                Ingredient = (Item)GDOUtils.GetExistingGDO(ItemReferences.BambooCookedContainerCooked)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.BambooRaw),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Rice)
        };
        public override GenericMysteryDish BaseMysteryDish =>
            (GenericMysteryDish)GDOUtils.GetCustomGameDataObject<MysteryStirFryBaseDish>();
    }
}
