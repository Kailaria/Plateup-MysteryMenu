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

namespace KitchenMasteryMenu.Customs.Dishes.Cakes
{
    public class MasteryCakesLemonDoughnutDish : GenericMasteryDish
    {
        protected override string NameTag => "Cake - Lemon Doughnut";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.Doughnuts);
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
                "<color=yellow>Requires ingredients:</color> Doughnut tray (tray), Flour, Egg, Sugar, Milk, Oil, Lemon\n" +
                $"Mix {References.ColorTextCakeBatter} in a mixing bowl. Add milk to the Cake Batter, then pour into doughnut tray and let set (provides 12 portions).\n" +
                "Put Oil in a pot. Portion a raw doughnut from the tray and place in the pot of oil to fry it.\n" +
                "Chop a lemon. Portion the cooked doughnut from the pot, then combine with the chopped lemon and serve to customers " +
                $"ordering lemon flavour {References.PinkTintCakesText} for dessert."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Cakes - Lemon Doughnut",
                Description = "Adds lemon doughnuts as a dessert when flour, egg, sugar, milk, oil, lemon, and a doughnut tray are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.LemonFlavour),
                Phase = MenuPhase.Dessert,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMasteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.DoughnutTray),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Flour),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Sugar),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Egg),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Milk),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Lemon),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Oil)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuSubstitutionsComplexityDish>()
        };
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
        public override bool HasTrayIngredient => true;
    }
}
