using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Salad
{
    public class MysterySaladDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Salad Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.SaladBase);
        public override DishType Type => DishType.Base;
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
            { Locale.English, "Chop lettuce and plate. If tomato is present, also sometimes chop tomato once and add." }
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
                Weight = 1
            }
        };
        public override List<Item> MinimumRequiredMysteryIngredients => new List<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Lettuce)
        };
        // TODO: separate this into a separate Dish/recipe and only use MinimumRequiredMysteryIngredients
        //public override List<Item> UnlockedOptionalMysteryIngredients => new List<Item>()
        //{
        //    (Item) GDOUtils.GetExistingGDO(ItemReferences.Tomato)
        //};
    }
}
