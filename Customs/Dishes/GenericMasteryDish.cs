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
    public abstract class GenericMasteryDish : CustomDish
    {
        protected abstract string NameTag { get; }
        // Make sure OrigDish is a unique ID
        public abstract Dish OrigDish { get; }
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override string UniqueNameID => "Mastery Dish: " + NameTag;
        public override bool IsUnlockable => false;
        public override bool IsAvailableAsLobbyOption => false;

        public virtual HashSet<Item> MinimumRequiredMasteryIngredients => default;
        public virtual HashSet<SubstitutionIngredientSet> SubstitutionIngredientSets => default;

        public override HashSet<Process> RequiredProcesses => OrigDish.RequiredProcesses;
        /**
         * RequiresVariant
         * Override as true if the base dish is not enough to be ordered on its own. Especially useful for plated dishes
         *   like Pies and Stir Fry to ensure that their normal bases aren't needed to be available in order to be served.
         */
        public virtual bool RequiresBaseVariant => false;
        public virtual bool HasTrayIngredient => false;
        public virtual GenericMasteryDish BaseMasteryDish => default;
        public virtual int BaseResultingItem => 0;
        public virtual MenuPhase MenuPhase => MenuPhase.Main;
        public virtual HashSet<Item> PreventIngredientReturns => default;
        public virtual List<RestaurantStatus> AddsStatuses => default;

        public override void OnRegister(Dish gameDataObject)
        {
            base.OnRegister(gameDataObject);
            MasteryDishCrossReference.RegisterDish(this);
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            base.AttachDependentProperties(gameData, gameDataObject);
            Dish dish = (Dish)gameDataObject;
            OverrideVariable(dish, "AddsStatuses", AddsStatuses);
        }
    }
}
