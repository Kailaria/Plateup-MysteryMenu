using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenMasteryMenu.Components
{
    public struct CNewMasteryRecipe : IComponentData
    {
        public int DishID;
        public int CardID;
    }
}
