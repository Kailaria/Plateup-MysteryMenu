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
    public abstract class GenericMysteryDish : CustomDish
    {
        protected abstract string NameTag { get; }
        // Make sure OrigDish is a unique ID
        public abstract Dish OrigDish { get; }
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override string UniqueNameID => "Mystery Dish: " + NameTag;
        public override bool IsUnlockable => false;
        public override bool IsAvailableAsLobbyOption => false;

        public abstract HashSet<Item> MinimumRequiredMysteryIngredients { get; }

        public override HashSet<Process> RequiredProcesses => OrigDish.RequiredProcesses;
        /**
         * RequiresVariant
         * Override as true if the base dish is not enough to be ordered on its own. Especially useful for plated dishes
         *   like Pies and Stir Fry to ensure that their normal bases aren't needed to be available in order to be served.
         */
        public virtual bool RequiresVariant => false;
        public virtual GenericMysteryDish BaseMysteryDish => default;

        public override void OnRegister(Dish gameDataObject)
        {
            base.OnRegister(gameDataObject);
            MysteryDishCrossReference.RegisterDish(this);
        }
    }
}
