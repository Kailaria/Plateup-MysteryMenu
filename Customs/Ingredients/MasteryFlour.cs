using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Customs.Appliances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Customs.Ingredients
{
    public class MasteryFlour : GenericMasteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.Flour);
        protected override string NameTag => "Mastery Flour";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MasteryIngredientProvider2>();
        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>()
            {
                new()
                {
                    Duration = 1f,
                    Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Knead),
                    Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.Dough)
                }
        };
    }
}
