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
    public class MysterySaladPotatoDish : GenericMysteryDish
    {
        protected override string NameTag => "Salad - Potato Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.SaladPotato);
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 4;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredients:</color> Potato, Egg, Oil, Onion\n" +
                "Chop potato, add to a pot with water and boil. Crack an egg, combine it with oil to make mayonnaise.\n" +
                "Chop an onion, then combine the boiled chopped potato, mayo, and onion onto a plate." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Potato Salad",
                Description = "Adds potato salad as a main when <b>Potato</b>, <b>Eggs</b>, <b>Oil</b>, and <b>Onions</b> are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemGroupReferences.SaladPotatoPlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Potato),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Egg),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Oil),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Onion),
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuVeggieVariationsDish>()
        };
    }
}
