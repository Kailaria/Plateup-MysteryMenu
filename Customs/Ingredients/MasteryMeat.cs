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
    public class MasteryMeat : GenericMasteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.Meat);
        protected override string NameTag => "Mastery Meat";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MasteryIngredientProvider>();
        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>()
        {
            new()
            {
                Duration = 5f,
                Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Cook),
                Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.SteakRare)
            },
            new()
            {
                Duration = 2f,
                Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Chop),
                Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.MeatChopped)
            }
        };
    }
}
