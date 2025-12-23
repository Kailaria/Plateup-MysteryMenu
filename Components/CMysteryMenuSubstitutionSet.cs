using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenMysteryMenu.Components
{
    // Keeping component data small since the recipe from the GDO will be available, anyway
    public struct CMysteryMenuSubstitutionSet : IComponentData
    {
        public int SubstitutedItemID; // To reference in SelectMysteryMenuOfDay
        public int SourceMysteryDishID; // To find the substitution recipe
    }
}
