using Kitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Systems
{
    public class HandleNewMysteryMenuDish : RestaurantSystem
    {
        protected override void Initialise()
        {
            base.Initialise();
            // Only run when there are mystery providers..?
        }

        protected override void OnUpdate()
        {
            // Check for DynamicMenuType = References.DynamicMenuTypeMystery
            // Add CMysteryMenuItem to whatever has CDynamicMenuItem and is actually of type Mystery
        }
    }
}
