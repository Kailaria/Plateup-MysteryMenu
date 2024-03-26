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
    public class MysterySoySauce : GenericMysteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.CondimentSoySauce);
        protected override string NameTag => "Mystery Soy Sauce";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MysteryIngredientProviderExtra4>();
    }
}
