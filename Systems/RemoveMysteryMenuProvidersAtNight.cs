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
                if (providerMysteryProviders[i].Type == MysteryMenuType.Mystery)
                {
                    // This probably makes more sense when the prefab is set up properly. For now, just do it the same way.
                    var cItemProvider = providerItemProviders[i];
                    cItemProvider.Available = 0;
                    EntityManager.SetComponentData(providerEntities[i], cItemProvider);
                }
            }
        }
    }
}
