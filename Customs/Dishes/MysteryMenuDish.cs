using KitchenLib.Customs;
using KitchenMysteryMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteryMenu.Customs.Dishes
{
    public class MysteryMenuDish : CustomDish
    {
        public override string UniqueNameID => Mod.MOD_GUID + ":MysteryMenuDish";
    }
}
