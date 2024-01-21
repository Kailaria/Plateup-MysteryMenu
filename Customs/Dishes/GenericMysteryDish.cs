using KitchenData;
using KitchenLib.Customs;
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
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override string UniqueNameID => "Mystery : " + NameTag;
        public override bool IsUnlockable => false;
        public override bool IsAvailableAsLobbyOption => false;

        public abstract List<Item> MinimumRequiredMysteryIngredients { get; }
        public abstract List<Item> UnlockedOptionalMysteryIngredients { get; }
    }
}
