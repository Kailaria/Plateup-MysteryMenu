using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMysteryMenu.Components
{
    public struct SRebuildMysteryKitchen : IComponentData
    {
        public int PrevDish;
        public int CurDish;
        //[NativeFixedLength(2)]
        //public NativeArray<int> Ingredients;
    }
}
