using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Customs.Appliances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Ingredients
{
    public class MysteryFlour : GenericMysteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.Flour);
        protected override string NameTag => "Mystery Flour";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MysteryIngredientProvider2>();
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
