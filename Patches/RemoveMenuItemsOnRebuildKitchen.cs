using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Patches
{
    public class RemoveMenuItemsOnRebuildKitchen
    {

        // Need to patch over RebuildKitchen to remove menu item entities that can't be ordered
        // Might not be necessary depending on if AlsoAddRecipes adding the cards also adds the dishes *in the restaurant*,
        //  unlike with the kitchen
    }
}
