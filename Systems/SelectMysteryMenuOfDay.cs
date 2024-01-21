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
    public class SelectMysteryMenuOfDay : StartOfDaySystem
    {
        EntityQuery ItemProviders;
        EntityQuery MenuItems;
        EntityQuery DisabledMenuItems;

        protected override void Initialise()
        {
            base.Initialise();
            ItemProviders = GetEntityQuery(typeof(CItemProvider), typeof(CMysteryMenuProvider));
            MenuItems = GetEntityQuery(typeof(CMenuItem), typeof(CMysteryMenuItem));
            DisabledMenuItems = GetEntityQuery(typeof(CMysteryMenuItem), typeof(CDisabledMysteryMenu));
        }

        protected override void OnUpdate()
        {
            
        }
    }
}
