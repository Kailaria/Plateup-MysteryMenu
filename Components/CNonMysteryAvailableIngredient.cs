using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace MysteryMenu.Components
{
    // Used to prevent constantly running "HandleNewMysteryMenuDish" on AvailableIngredients
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct CNonMysteryAvailableIngredient : IComponentData
    {
    }
}
