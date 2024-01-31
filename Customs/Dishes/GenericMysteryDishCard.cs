using KitchenData;
using KitchenLib.Customs;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes
{
    public abstract class GenericMysteryDishCard : CustomDish
    {
        protected abstract string NameTag { get; }
        public override string UniqueNameID => "Mystery Menu Card : " + NameTag;
        public abstract HashSet<GenericMysteryDish> ContainedMysteryRecipes { get; }
        public override List<Dish.MenuItem> ResultingMenuItems => 
            ContainedMysteryRecipes.SelectMany(r => r.ResultingMenuItems).ToList();
        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks =>
            ContainedMysteryRecipes.SelectMany(r => r.IngredientsUnlocks).ToHashSet();
        public override List<Dish> AlsoAddRecipes => 
            ContainedMysteryRecipes.Select(r => r.GameDataObject).ToList();

        public override void OnRegister(Dish gameDataObject)
        {
            base.OnRegister(gameDataObject);
            MysteryDishCrossReference.RegisterDishCard(this);
        }
    }
}
