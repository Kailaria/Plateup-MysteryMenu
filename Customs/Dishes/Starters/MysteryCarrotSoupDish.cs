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

namespace KitchenMysteryMenu.Customs.Dishes.Starters
{
    public class MysteryCarrotSoupDish : GenericMysteryDish
    {
        protected override string NameTag => "Carrot Soup";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.CarrotSoup);
        public override DishType Type => DishType.Starter;
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
                "<color=yellow>Requires ingredients:</color> Onion, Carrot\n" +
                "Put onion into a pot with water and boil to make broth. Combine broth with carrot" +
                "then cook again. Portion to serve as a Starter." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Starter - Carrot Soup",
                Description = "Adds carrot soup as a starter when onion and carrots are present",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.ServedSoupCarrot),
                Phase = MenuPhase.Starter,
                Weight = 1
            }
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Onion),
            (Item) GDOUtils.GetExistingGDO(ItemReferences.Carrot)
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuSaucesSoupsDish>()
        };
        public override MenuPhase MenuPhase => MenuPhase.Starter;
    }
}
