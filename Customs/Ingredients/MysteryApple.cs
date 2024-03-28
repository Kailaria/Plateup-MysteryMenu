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
    public class MysteryApple : GenericMysteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.Apple);
        protected override string NameTag => "Mystery Apple";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MysteryIngredientProviderExtra5>();
        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>()
        {
            new()
            {
                Duration = 2f,
                Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Chop),
                Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.AppleSlices)
            }
        };
    }
}
