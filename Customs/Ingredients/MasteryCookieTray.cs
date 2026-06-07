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
    public class MasteryCookieTray : GenericMasteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.CookieTray);
        protected override string NameTag => "Cookie Tray";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MasteryTrayProviderCakes>();
    }
}
