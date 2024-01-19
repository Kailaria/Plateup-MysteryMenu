using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMysteryMenu.Customs.Ingredients
{
    public abstract class GenericMysteryItem : CustomItem
    {
        protected abstract string NameTag { get; }
        public abstract Item ExistingGDO { get; }
        public override string UniqueNameID => Mod.MOD_NAME + " : " + NameTag;
        public override GameObject Prefab => ExistingGDO.Prefab;
        public override ItemStorage ItemStorageFlags => ExistingGDO.ItemStorageFlags;
    }
}
