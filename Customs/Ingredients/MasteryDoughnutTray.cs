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
    public class MasteryDoughnutTray : GenericMasteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.DoughnutTray);
        protected override string NameTag => "Doughnut Tray";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MasteryTrayProviderComplexities>();
    }
}
