using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Customs.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.ItemGroups
{
    public class MysteryDough : GenericMysteryItemGroup
    {
        protected override string NameTag => "Mystery Dough";
        protected override ItemGroup ExistingGDO => (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.Dough);
        public override List<ItemGroup.ItemSet> Sets => new()
        {
            new ItemGroup.ItemSet
            {
                Min = 2,
                Max = 2,
                IsMandatory = true,
                Items = new List<Item>()
                {
                    GDOUtils.GetCastedGDO<Item,MysteryFlour>(),
                    (Item) GDOUtils.GetExistingGDO(ItemReferences.Water)
                }
            }
        };
        public override List<Item.ItemProcess> Processes => new()
        {
            new Item.ItemProcess
            {
                Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Knead),
                Duration = 2f,
                Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.PieCrustRaw)
            },
            new Item.ItemProcess
            {
                Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Cook),
                Duration = 20f,
                Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.BreadBaked)
            }
        };
    }
}
