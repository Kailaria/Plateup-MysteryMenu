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
            ItemProviders = GetEntityQuery(
                new QueryHelper()
                    .All(typeof(CItemProvider),typeof(CMysteryMenuProvider),typeof(CPosition))
                );
            MenuItems = GetEntityQuery(
                new QueryHelper()
                    .All(typeof(CMenuItem), typeof(CMysteryMenuItem))
                );
            DisabledMenuItems = GetEntityQuery(
                new QueryHelper()
                    .All(typeof(CMysteryMenuItem), typeof(CDisabledMysteryMenu))
                );
        }

        protected override void OnUpdate()
        {
            
        }
    }
}
