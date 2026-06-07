using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Utils;
using MasteryMenu.Customs.Dishes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Dishes.Sundaes
{
    public class MasteryHomemadeChocolateIceCreamDish : GenericMasteryDish
    {
        protected override string NameTag => "Sundae/Ice Cream - Homemade Chocolate";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(References.SundaeHomemadeVariantDish);
        public override DishType Type => DishType.Base;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => null;
        public override bool RequiredNoDishItem => true;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 5;
        public override Dictionary<Locale, string> Recipe => new()
        {
            { Locale.English,
                "<color=yellow>Requires ingredient:</color> Milk, Sugar, Chocolate\n" + 
                "Combine Milk and Sugar into a Mixing Bowl, then freeze it. After it has frozen, " +
                "combine with Chocolate, then knead to make Chocolate Ice Cream. Provides 5 portions." }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mastery - Homemade Chocolate Ice Cream",
                Description = "Substitutes Chocolate Ice Cream for its homemade variant.",
                FlavourText = $"{References.DishCardDoNotAddFlavorText}"
            })
        };
        public override HashSet<SubstitutionIngredientSet> SubstitutionIngredientSets => new()
        {
            new SubstitutionIngredientSet(
                GDOUtils.GetExistingGDO(ItemReferences.IceCreamChocolate) as Item,
                new HashSet<Item>()
                {
                    GDOUtils.GetExistingGDO(ItemReferences.Milk) as Item,
                    GDOUtils.GetExistingGDO(ItemReferences.Chocolate) as Item,
                    GDOUtils.GetExistingGDO(ItemReferences.Sugar) as Item
                })
        };
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MasteryMenuSubstitutionsComplexityDish>()
        };
        public override GenericMasteryDish BaseMasteryDish => (GenericMasteryDish)GDOUtils.GetCustomGameDataObject<MasterySundaeBaseDish>();
    }
}
