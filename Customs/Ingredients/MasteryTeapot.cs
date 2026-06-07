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
    public class MasteryTeapot : GenericMasteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.TeaPot);
        protected override string NameTag => "Mastery Teapot";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MasteryIngredientProviderCakes3>();
    }
}
