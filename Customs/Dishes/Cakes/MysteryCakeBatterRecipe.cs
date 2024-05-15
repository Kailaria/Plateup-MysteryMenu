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

namespace KitchenMysteryMenu.Customs.Dishes.Cakes
{
    public class MysteryCakeBatterRecipe : GenericMysteryDish
    {
        protected override string NameTag => "Cake Batter Recipe";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.Cakes);
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
                "<color=yellow>Requires ingredients:</color> Flour, Egg, Sugar\n" +
                "RECIPE ONLY\n" +
                $"Crack (chop) an egg. Combine cracked egg with a mixing bowl, flour, and sugar and knead to make " +
                $"{References.ColorTextCakeBatter}."}
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Cake Batter (recipe only)",
                Description = "",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
        };
        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>()
        {
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuCoffeeCakesPiesDish>()
        };
        public override MenuPhase MenuPhase => MenuPhase.Dessert;
        public override bool HasTrayIngredient => true;
    }
}
