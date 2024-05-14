using Kitchen;
using KitchenData;
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
    public class RemoveMysteryMenuProvidersAtNight : StartOfNightSystem
    {
        private string LogMsgPrefix = "[RemoveMysterMenuProvidersAtNight]";

        EntityQuery MysteryProviders;

        protected override void Initialise()
        {
            base.Initialise();
            MysteryProviders = GetEntityQuery(new QueryHelper()
                .All(typeof(CItemProvider), typeof(CMysteryMenuProvider))
                .None(typeof(CPreservesContentsOvernight)));
            RequireForUpdate(MysteryProviders);
        }

        protected override void OnUpdate()
        {
            using var providerEntities = MysteryProviders.ToEntityArray(Allocator.Temp);
            using var providerItemProviders = MysteryProviders.ToComponentDataArray<CItemProvider>(Allocator.Temp);
            using var providerMysteryProviders = MysteryProviders.ToComponentDataArray<CMysteryMenuProvider>(Allocator.Temp);

            for (int i = 0; i < providerEntities.Length; i++)
            {
                if (providerMysteryProviders[i].Type == MysteryMenuType.Mystery || 
                    providerMysteryProviders[i].Type == MysteryMenuType.MysteryTray)
                {
                    // This probably makes more sense when the prefab is set up properly. For now, just do it the same way.
                    var cItemProvider = providerItemProviders[i];
                    cItemProvider.Available = 0;
                    // Issue #5: empty out the items from the providers so that they can't be used to determine what static ingredients
                    //      are available or not when new static dish cards are selected. Might want to revert it to the default instead?
                    cItemProvider.SetAsItem(0);
                    if (providerMysteryProviders[i].Type == MysteryMenuType.MysteryTray)
                    {
                        cItemProvider.AutoPlaceOnHolder = false;
                    }
                    EntityManager.SetComponentData(providerEntities[i], cItemProvider);
                }
            }
        }
    }
}
