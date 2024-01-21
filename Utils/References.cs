using KitchenData;
using KitchenLib.Utils;
using KitchenMysteryMenu.Customs.Dishes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Utils
{
    internal class References
    {
        public static readonly int MYSTERY_MENU_ID = GDOUtils.GetCastedGDO<Dish, MysteryMenuDish>().ID;
    }
}
