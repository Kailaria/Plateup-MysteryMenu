using KitchenData;
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
    public class MysteryPieMeatRaw : GenericMysteryItemGroup
    {
        protected override string NameTag => "Mystery Pie - Meat Raw";
        protected override ItemGroup ExistingGDO => (ItemGroup) GDOUtils.GetExistingGDO(ItemGroupReferences.PieMeatRaw);

        public override List<ItemGroup.ItemSet> Sets => new()
        {
            new ItemGroup.ItemSet
            {
                Min = 2,
                Max = 2,
                IsMandatory = false,
                Items = new List<Item>()
                {
                    (Item) GDOUtils.GetExistingGDO(ItemReferences.PieCrustRaw),
                    GDOUtils.GetCastedGDO<Item,MysteryMeat>()
                }
            }
        };
        public override List<Item.ItemProcess> Processes => new()
        {
            new Item.ItemProcess
            {
                Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Cook),
                Duration = 5f,
                Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.PieMeatCooked)
            }
        };
    }

    public class MysteryPieMeatRawBlindBaked : GenericMysteryItemGroup
    {
        protected override string NameTag => "Mystery Pie - Meat Raw - Blind Baked";
        protected override ItemGroup ExistingGDO => (ItemGroup)GDOUtils.GetExistingGDO(ItemGroupReferences.PieMeatRawBlindBaked);

        public override List<ItemGroup.ItemSet> Sets => new()
        {
            new ItemGroup.ItemSet
            {
                Min = 2,
                Max = 2,
                IsMandatory = false,
                Items = new List<Item>()
                {
                    (Item) GDOUtils.GetExistingGDO(ItemReferences.PieCrustCooked),
                    GDOUtils.GetCastedGDO<Item,MysteryMeat>()
                }
            }
        };
        public override List<Item.ItemProcess> Processes => new()
        {
            new Item.ItemProcess
            {
                Process = (Process) GDOUtils.GetExistingGDO(ProcessReferences.Cook),
                Duration = 3f,
                Result = (Item) GDOUtils.GetExistingGDO(ItemReferences.PieMeatCooked)
            }
        };
    }
}
