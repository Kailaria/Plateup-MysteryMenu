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
    public class MasteryIceCreamStrawberry : GenericMasteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.IceCreamStrawberry);
        protected override string NameTag => "Strawberry Ice Cream Scoop";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MasteryIngredientProviderBoard>();
    }
}
