using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Customs.Dishes.Turkey;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Coffee
{
    public class MysteryTeaDish : GenericMysteryDish
    {
        protected override string NameTag => "Tea";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.Tea);
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
                "<color=yellow>Requires ingredients:</color> Tea pots, Tea bags, Tea Cups\n" +
                "Combine a tea pot with a tea bag and water, then let steep. Serve the pot and a tea cup " +
                "as a dessert. One tea pot serves up to 3 customers, but each needs their own cup.\n" +
                $"Counts as a {References.ColorTextHotDrink}."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Tea",
                Description = "Adds tea as a dessert when tea cups, tea pots, and tea bags are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemGroupReferences.TeaPotSteeped), // TODO: check vanilla for the actual resulting menu item
                Phase = MenuPhase.Dessert,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.TeaPot),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.TeaBag),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.TeaCup)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakeVarietyDish>()
        };
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
    }
}
