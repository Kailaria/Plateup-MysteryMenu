using KitchenLib.Customs;
using KitchenMysteryMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryMenu.Customs.Appliances
{
    public class SourceMysteryIngredient : CustomAppliance
    {
        public override string UniqueNameID => Mod.MOD_GUID + ":SourceMysteryIngredient";
    }
}
