using Kitchen;
using KitchenMysteryMenu.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenMysteryMenu.Systems
{
    public class HandleNewMysteryMenuDish : RestaurantSystem
    {
        private EntityQuery MysteryProviders;
        private EntityQuery NonHandledMenuItems;

        protected override void Initialise()
        {
            base.Initialise();
            MysteryProviders = GetEntityQuery(typeof(CItemProvider), typeof(CMysteryMenuProvider));
            NonHandledMenuItems = GetEntityQuery(new QueryHelper()
                .All(typeof(CMenuItem), typeof(CDynamicMenuItem))
                .None(typeof(CMysteryMenuItem), typeof(CNonMysteryMenuItem)));
            RequireForUpdate(MysteryProviders);
            RequireForUpdate(NonHandledMenuItems);
        }

        protected override void OnUpdate()
        {
            // TODO: Refer to HandleNewDish system
            // Check for DynamicMenuType = References.DynamicMenuTypeMystery
            // Add CMysteryMenuItem to whatever has CDynamicMenuItem and is actually of type Mystery
        }
    }
}
