using HarmonyLib;
using Kitchen;
using KitchenLib.References;
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
    internal class RebuildMysteryKitchen : FranchiseSystem
    {
        private EntityQuery HqItemProviders;

        protected override void Initialise()
        {
            base.Initialise();
            HqItemProviders = GetEntityQuery(typeof(RebuildKitchen.CFranchiseKitchenAppliance), typeof(CItemProvider));
            RequireSingletonForUpdate<RebuildKitchen.SCurrentKitchen>();
        }

        protected override void OnUpdate()
        {
            Mod.Logger.LogInfo("RebuildMysteryKitchen Updating");
            var sRebuildMysteryKitchen = GetOrCreate<SRebuildMysteryKitchen>();

            if (!TryGetSingleton<RebuildKitchen.SCurrentKitchen>(out var sCurrentKitchen))
            {
                Mod.Logger.LogInfo("RebuildMysteryKitchen - SCurrentKitchen not created yet.");
                return;
            }
            int currentDish = sCurrentKitchen.Dish;
            sRebuildMysteryKitchen.CurDish = currentDish;

            // If the dish hasn't changed since the last update, or the current dish isn't Mystery Menu, we're done.
            // #TODO - Eventual QoL appliance to trigger mystery ingredients randomization in the lobby for practice purposes
            //      without unloading and loading the dish itself.
            if (sRebuildMysteryKitchen.CurDish == sRebuildMysteryKitchen.PrevDish)
            {
                return;
            }

            // Update the Prev Dish, then check if we need to update the Item Providers
            sRebuildMysteryKitchen.PrevDish = currentDish;
            SetSingleton(sRebuildMysteryKitchen);
            if (currentDish != References.MysteryMenuBaseDish.ID)
            {
                return;
            }

            // Finally, actually update the mystery providers to (TODO: randomize and) set the ingredients so that they're
            //  vanilla ingredients and not the Mystery placeholders.
            UpdateMysteryIngredients();
            Mod.Logger.LogInfo("RebuildMysteryKitchen - Done updating ingredients");
        }

        private void UpdateMysteryIngredients()
        {
            Mod.Logger.LogInfo("RebuildMysteryKitchen - It's the Mystery Menu dish!");
            List<int> itemIDs = GetReplacementIngredients();
            int ingredientIndex = 0;
            using var itemProviders = HqItemProviders.ToEntityArray(Allocator.Temp);
            using var existingCItemProviders = HqItemProviders.ToComponentDataArray<CItemProvider>(Allocator.Temp);
            Mod.Logger.LogInfo("RebuildMysteryKitchen - Updating ingredients");
            for (int i = 0; i < itemProviders.Length; i++)
            {
                var entity = itemProviders[i];
                var cItemProvider = existingCItemProviders[i];

                // Plate & Wok Stacks are also Item Providers, so don't change those.
                if (cItemProvider.ProvidedItem == ItemReferences.Plate
                    || cItemProvider.ProvidedItem == ItemReferences.Wok)
                {
                    continue;
                }

                cItemProvider.ProvidedItem = itemIDs[ingredientIndex];
                EntityManager.SetComponentData(entity, cItemProvider);
                Mod.Logger.LogInfo("Component Data Set! Cyclically increment ingredientIndex.");
                ingredientIndex = (ingredientIndex + 1) % itemIDs.Count;
            }
        }

        private List<int> GetReplacementIngredients()
        {
            List<int> itemIDs = new()
            {
                ItemReferences.Meat,
                ItemReferences.Flour
            };
            return itemIDs;
        }
    }
}
