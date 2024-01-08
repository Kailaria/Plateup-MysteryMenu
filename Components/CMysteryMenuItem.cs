using MysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace MysteryMenu.Components
{
    public struct CMysteryMenuItem : IComponentData
    {
        public int Ingredient;

        public MysteryMenuType Type;

        public bool HasBeenProvided;
    }
}
