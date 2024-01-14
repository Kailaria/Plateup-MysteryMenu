using KitchenData;
using KitchenLib.Customs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.ItemGroups
{
    public abstract class GenericMysteryItemGroup : CustomItemGroup
    {
        protected abstract string NameTag { get; }
        protected abstract ItemGroup ExistingGDO { get; }
        public override string UniqueNameID => Mod.MOD_NAME + " : " + NameTag;

    }
}
