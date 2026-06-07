using Kitchen;
using KitchenData;
using KitchenMasteryMenu.Components;
using KitchenMasteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMasteryMenu.Systems
{
    public class RemoveMasteryMenuProvidersAtNight : StartOfNightSystem
    {
        private string LogMsgPrefix = "[RemoveMysterMenuProvidersAtNight]";

        EntityQuery MasteryProviders;

        protected override void Initialise()
        {
            base.Initialise();
            MasteryProviders = GetEntityQuery(new QueryHelper()
                .All(typeof(CItemProvider), typeof(CMasteryMenuProvider))
                .None(typeof(CPreservesContentsOvernight)));
            RequireForUpdate(MasteryProviders);
        }

        protected override void OnUpdate()
        {
            using var providerEntities = MasteryProviders.ToEntityArray(Allocator.Temp);
            using var providerItemProviders = MasteryProviders.ToComponentDataArray<CItemProvider>(Allocator.Temp);
            using var providerMasteryProviders = MasteryProviders.ToComponentDataArray<CMasteryMenuProvider>(Allocator.Temp);

            for (int i = 0; i < providerEntities.Length; i++)
            {
                if (providerMasteryProviders[i].Type == MasteryMenuType.Mastery || 
                    providerMasteryProviders[i].Type == MasteryMenuType.MasteryTray)
                {
                    // This probably makes more sense when the prefab is set up properly. For now, just do it the same way.
                    var cItemProvider = providerItemProviders[i];
                    cItemProvider.Available = 0;
                    // Issue #5: empty out the items from the providers so that they can't be used to determine what static ingredients
                    //      are available or not when new static dish cards are selected. Might want to revert it to the default instead?
                    cItemProvider.SetAsItem(0);
                    if (providerMasteryProviders[i].Type == MasteryMenuType.MasteryTray)
                    {
                        cItemProvider.AutoPlaceOnHolder = false;
                    }
                    EntityManager.SetComponentData(providerEntities[i], cItemProvider);
                }
            }
        }
    }
}
