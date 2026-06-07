using KitchenData;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenMasteryMenu.Components
{
    public struct CMasteryMenuProvider : IApplianceProperty, IAttachableProperty, IComponentData
    {
        public MasteryMenuType Type;
    }
}
