using KitchenData;
using KitchenLib.Customs;
using KitchenMasteryMenu.Utils;
using MasteryMenu.Customs.Dishes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Dishes
{
    public abstract class GenericMasteryDishCard : CustomDish
    {
        protected abstract string NameTag { get; }
        public override string UniqueNameID => "Mastery Card: " + NameTag;
        public abstract HashSet<GenericMasteryDish> ContainedMasteryRecipes { get; }
        public override List<Dish.MenuItem> ResultingMenuItems => 
            ContainedMasteryRecipes.SelectMany(r => r.ResultingMenuItems).ToList();
        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks =>
            ContainedMasteryRecipes.SelectMany(r => r.IngredientsUnlocks).ToHashSet();
        public override List<Dish> AlsoAddRecipes => 
            ContainedMasteryRecipes.Select(r => r.GameDataObject).ToList();
        public override HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks =>
            ContainedMasteryRecipes.SelectMany(r => r.ExtraOrderUnlocks).ToHashSet();
        public virtual HashSet<SubstitutionIngredientSet> SubstitutionIngredientSets =>
            ContainedMasteryRecipes.SelectMany(r => r.SubstitutionIngredientSets).ToHashSet();

        public virtual List<RestaurantStatus> AddsStatuses => 
            ContainedMasteryRecipes.Where(r => r.AddsStatuses != default).SelectMany(r => r.AddsStatuses).ToList();

        public override void OnRegister(Dish gameDataObject)
        {
            base.OnRegister(gameDataObject);
            MasteryDishCrossReference.RegisterDishCard(this);
        }
        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            base.AttachDependentProperties(gameData, gameDataObject);
            Dish dish = (Dish)gameDataObject;
            OverrideVariable(dish, "AddsStatuses", AddsStatuses);
        }
    }
}
