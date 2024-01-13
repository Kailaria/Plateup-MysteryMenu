using KitchenData;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenMysteryMenu.Components
{
    public struct CMysteryMenuProvider : IApplianceProperty, IAttachableProperty, IComponentData
    {
        public MysteryMenuType Type;
    }
}
