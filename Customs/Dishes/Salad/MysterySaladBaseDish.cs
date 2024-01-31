using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Salad
{
    public class MysterySaladBaseDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Salad Base Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.SaladBase);
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 1;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English, "Chop lettuce and plate." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Salad",
                Description = "Adds salad as a main dish when <b>Lettuce</b> is present",
                FlavourText = "No cooking, but lots of chopping!\n" +
                            "Basic lettuce salads. If <b>Tomato</b> is present, serve salads with and without chopped tomato."
            })
        };
        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item) GDOUtils.GetExistingGDO(ItemReferences.SaladPlated),
                Phase = MenuPhase.Main,
                Weight = 1,
                DynamicMenuType = References.DynamicMenuTypeMystery,
                DynamicMenuIngredient = (Item) GDOUtils.GetExistingGDO(ItemReferences.Lettuce)
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Lettuce)
        };
    }
}
