using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMasteryMenu.Components
{
    public struct CMasteryMenuItem : IComponentData
    {
        public int SourceMasteryDish;

        public MasteryMenuType Type;

        public bool HasBeenProvided;
    }
}
