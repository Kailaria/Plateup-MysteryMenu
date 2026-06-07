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
    public class MasterySurfNTurf : GenericMasteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.FishBlueRaw);
        protected override string NameTag => "Mastery SurfNTurf";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MasteryIngredientProviderExtra>();
        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>()
        {
            new()
            {
                Duration = 5f,
                Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Cook),
                Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.FishBlueFried)
            }
        };
    }
}
