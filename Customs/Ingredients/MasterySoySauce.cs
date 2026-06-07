using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Customs.Appliances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMasteryMenu.Customs.Ingredients
{
    public class MasterySoySauce : GenericMasteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.CondimentSoySauce);
        protected override string NameTag => "Mastery Soy Sauce";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MasteryIngredientProviderExtra4>();
    }
}
