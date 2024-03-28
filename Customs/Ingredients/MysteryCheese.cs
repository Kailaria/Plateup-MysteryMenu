using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Customs.Appliances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMysteryMenu.Customs.Ingredients
{
    public class MysteryCheese : GenericMysteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.Cheese);
        protected override string NameTag => "Mystery Cheese";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MysteryIngredientProviderExtra7>();
    }
}
