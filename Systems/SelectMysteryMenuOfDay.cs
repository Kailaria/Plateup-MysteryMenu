using Kitchen;
using KitchenMysteryMenu.Components;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
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
            RequireForUpdate(MenuItems);
        }

        protected override void OnUpdate()
        {
            // Borrowing a lot of code from SelectFishOfDay, but dish & menu selection will need to be vastly different
            //EntityManager.RemoveComponent<CDisabledMysteryMenu>(DisabledMenuItems);
            //using var menuItemEntities = MenuItems.ToEntityArray(Allocator.Temp);
            //using var menuItemMysteryComps = MenuItems.ToComponentDataArray<CMysteryMenuItem>(Allocator.Temp);
            //using var providerEntities = ItemProviders.ToEntityArray(Allocator.Temp);
            //using var providerMysteryComps = ItemProviders.ToComponentDataArray<CMysteryMenuProvider>(Allocator.Temp);

            //foreach (MysteryMenuType type in Enum.GetValues(typeof(MysteryMenuType)))
            //{
            //    List<Entity> oldMenuItemEntities = new List<Entity>();
            //    List<Entity> newMenuItemE
            //}
        }
    }
}
