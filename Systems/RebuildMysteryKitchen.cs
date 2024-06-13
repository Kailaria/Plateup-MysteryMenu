using HarmonyLib;
using Kitchen;
using KitchenLib.References;
using KitchenMysteryMenu.Components;
using KitchenMysteryMenu.Customs.Dishes;
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
        private string LogMsgPrefix = "[RebuildMysteryKitchen]";

        private EntityQuery HqItemProviders;
        private EntityQuery FranchiseMenuItems;
        private EntityQuery FranchiseMenuOptions;

        protected override void Initialise()
        {
            base.Initialise();
            HqItemProviders = GetEntityQuery(typeof(RebuildKitchen.CFranchiseKitchenAppliance), typeof(CItemProvider));
            FranchiseMenuItems = GetEntityQuery(typeof(RebuildKitchen.CFranchiseKitchenMenuItem), typeof(CMenuItem));
            FranchiseMenuOptions = GetEntityQuery(new QueryHelper()
                .All(typeof(RebuildKitchen.CFranchiseKitchenMenuItem), typeof(CAvailableIngredient))
                .None(typeof(CMenuItem)));
            RequireSingletonForUpdate<RebuildKitchen.SCurrentKitchen>();
        }

        protected override void OnUpdate()
        {
            //Mod.Logger.LogInfo("RebuildMysteryKitchen Updating");
            var sRebuildMysteryKitchen = GetOrCreate<SRebuildMysteryKitchen>();

            if (!TryGetSingleton<RebuildKitchen.SCurrentKitchen>(out var sCurrentKitchen))
            {
                Mod.Logger.LogInfo($"{LogMsgPrefix} - SCurrentKitchen not created yet.");
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
            AddMysteryMenuComponents(currentDish);
            //UpdateMysteryIngredients();
            SelectMysteryMenuOfDay selectMysteryMenuOfDayService = World.GetExistingSystem<SelectMysteryMenuOfDay>();
            selectMysteryMenuOfDayService.CreateMysteryMenu();
            Mod.Logger.LogInfo("RebuildMysteryKitchen - Done updating ingredients");
        }

        private void AddMysteryMenuComponents(int currentDishID)
        {
            Mod.Logger.LogInfo($"{LogMsgPrefix} - AddMysteryMenuComponents start");
            using var franchiseMenuEntities = FranchiseMenuItems.ToEntityArray(Allocator.Temp);
            using var franchiseMenuItemComps = FranchiseMenuItems.ToComponentDataArray<CMenuItem>(Allocator.Temp);
            using var franchiseMenuOptionEntities = FranchiseMenuOptions.ToEntityArray(Allocator.Temp);
            using var franchiseMenuOptionComps = FranchiseMenuOptions.ToComponentDataArray<CAvailableIngredient>(Allocator.Temp);

            GenericMysteryDishCard dishCard = MysteryDishCrossReference.GetMysteryCardById(currentDishID);

            // Go through each contained mystery dish, making sure to only use each resulting menu item and
            //  option once by checking against its source dish id to ensure it's not still the card's id
            foreach (GenericMysteryDish mysteryDish in dishCard.ContainedMysteryRecipes)
            {
                Mod.Logger.LogInfo($"{LogMsgPrefix} - AddMysteryMenuComponents() - Starting dish {{{mysteryDish.UniqueNameID}}} with " +
                    $"{{ResultingMenuItems.Count = {mysteryDish.ResultingMenuItems.Count}}} and " +
                    $"{{IngredientsUnlocks.Count = {mysteryDish.IngredientsUnlocks.Count}}}");
                int menuItemCount = 0;
                int attempts = 0;
                bool allFound = false;
                while (menuItemCount < mysteryDish.ResultingMenuItems.Count)
                {
                    attempts++;
                    for (int i = 0; i < franchiseMenuEntities.Length; i++)
                    {
                        Entity entity = franchiseMenuEntities[i];
                        CMenuItem cMenuItem = franchiseMenuItemComps[i];

                        // Skip this entity if it has already been assigned its recipe or does not have a
                        //  matching menu item in the current recipe. Franchise kitchen CMenuItem's source dishes
                        //  are 0's by default.
                        if ((cMenuItem.SourceDish != dishCard.GameDataObject.ID && cMenuItem.SourceDish != 0) || 
                            !mysteryDish.ResultingMenuItems.Any(mItem => mItem.Item.ID == cMenuItem.Item))
                        {
                            continue;
                        }

                        int mysteryDishGDOId = mysteryDish.GameDataObject.ID;
                        cMenuItem.SourceDish = mysteryDishGDOId;
                        EntityManager.AddComponentData(entity, cMenuItem);
                        EntityManager.AddComponentData(entity, new CMysteryMenuItem()
                        {
                            SourceMysteryDish = mysteryDishGDOId,
                            Type = MysteryMenuType.Mystery,
                            HasBeenProvided = false
                        });
                        menuItemCount++;
                        if (menuItemCount >= mysteryDish.ResultingMenuItems.Count)
                        {
                            Mod.Logger.LogInfo($"{LogMsgPrefix} - Found all CMenuItems for recipe {{{mysteryDish.UniqueNameID}}}");
                            allFound = true;
                            break;
                        }
                    }
                    if (!allFound && attempts >= 3)
                    {
                        Mod.Logger.LogWarning($"{LogMsgPrefix} - FAILED to find all CMenuItems for recipe {{{mysteryDish.UniqueNameID}}}");
                        break;
                    }
                }
                int menuOptionCount = 0;
                attempts = 0;
                allFound = false;
                while (menuOptionCount < mysteryDish.IngredientsUnlocks.Count)
                {
                    attempts++;
                    for (int i = 0; i < franchiseMenuOptionEntities.Length; i++)
                    {
                        Entity entity = franchiseMenuOptionEntities[i];
                        CAvailableIngredient cAvailableIngredient = franchiseMenuOptionComps[i];

                        // Skip this entity if it isn't a match (Menu Options don't have duplicates)
                        if (!mysteryDish.IngredientsUnlocks.Any(ui => ui.MenuItem.ID == cAvailableIngredient.MenuItem && ui.Ingredient.ID == cAvailableIngredient.Ingredient))
                        {
                            continue;
                        }

                        EntityManager.AddComponentData(entity, new CMysteryMenuItem()
                        {
                            SourceMysteryDish = mysteryDish.GameDataObject.ID,
                            Type = MysteryMenuType.Mystery,
                            HasBeenProvided = false
                        });
                        menuOptionCount++;
                        if (menuOptionCount >= mysteryDish.IngredientsUnlocks.Count)
                        {
                            Mod.Logger.LogInfo($"{LogMsgPrefix} - Found all CAvailableIngredients for recipe {{{mysteryDish.UniqueNameID}}}");
                            allFound = true;
                            break;
                        }
                    }
                    if (!allFound && attempts >= 3)
                    {
                        Mod.Logger.LogInfo($"{LogMsgPrefix} - FAILED to find all CAvailableIngredients for recipe {{{mysteryDish.UniqueNameID}}}");
                        break;
                    }
                }
            }
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
                if (MysteryDishUtils.IsLimitedContainer(cItemProvider.ProvidedItem))
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
