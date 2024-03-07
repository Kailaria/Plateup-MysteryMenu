using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMysteryMenu.Components
{
    public struct CMysteryMenuItem : IComponentData
    {
        public int SourceMysteryDish;

        public MysteryMenuType Type;

        public bool HasBeenProvided;
    }
}
