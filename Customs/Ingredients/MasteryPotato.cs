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
    public class MasteryPotato : GenericMasteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.Potato);
        protected override string NameTag => "Mastery Potato";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MasteryIngredientProviderSides>();
        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>()
        {
            new()
            {
                Duration = 3f,
                Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Chop),
                Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.PotatoChopped)
            },
            new()
            {
                Duration = 4f,
                Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Cook),
                Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.RoastPotatoItem)
            }
        };
    }
}
