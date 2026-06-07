using HarmonyLib;
using Kitchen;
using KitchenLib.References;
using KitchenMasteryMenu.Components;
using KitchenMasteryMenu.Customs.Dishes;
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
    internal class RebuildMasteryKitchen : FranchiseSystem
    {
        private string LogMsgPrefix = "[RebuildMasteryKitchen]";

        private EntityQuery HqItemProviders;
        private EntityQuery FranchiseMenuItems;
        private EntityQuery FranchiseMenuOptions;

        protected override void Initialise()
        {
            base.Initialise();
            HqItemProviders = GetEntityQuery(typeof(RebuildKitchen.CFranchiseKitchenAppliance), typeof(CItemProvider));
            FranchiseMenuItems = GetEntityQuery(typeof(RebuildKitchen.CFranchiseKitchenMenuItem), typeof(CMenuItem));
            FranchiseMenuOptions = GetEntityQuery(new QueryHelper()
                .All(typeof(CAvailableIngredient))
                .None(typeof(CMenuItem)));
            RequireSingletonForUpdate<RebuildKitchen.SCurrentKitchen>();
        }

        protected override void OnUpdate()
        {
            //Mod.Logger.LogInfo($"{LogMsgPrefix} Updating");
            var sRebuildMasteryKitchen = GetOrCreate<SRebuildMasteryKitchen>();

            if (!TryGetSingleton<RebuildKitchen.SCurrentKitchen>(out var sCurrentKitchen))
            {
                Mod.Logger.LogInfo($"{LogMsgPrefix} - SCurrentKitchen not created yet.");
                return;
            }
            int currentDish = sCurrentKitchen.Dish;
            sRebuildMasteryKitchen.CurDish = currentDish;

            // If the dish hasn't changed since the last update, or the current dish isn't Mastery Menu, we're done.
            // #TODO - Eventual QoL appliance to trigger Mastery ingredients randomization in the lobby for practice purposes
            //      without unloading and loading the dish itself.
            if (sRebuildMasteryKitchen.CurDish == sRebuildMasteryKitchen.PrevDish)
            {
                return;
            }

            // Update the Prev Dish, then check if we need to update the Item Providers
            sRebuildMasteryKitchen.PrevDish = currentDish;
            SetSingleton(sRebuildMasteryKitchen);
            if (currentDish != References.MasteryMenuBaseDish.ID)
            {
                return;
            }

            // Finally, actually update the Mastery providers to (TODO: randomize and) set the ingredients so that they're
            //  vanilla ingredients and not the Mastery placeholders.
            AddMasteryMenuComponents(currentDish);
            //UpdateMasteryIngredients();
            SelectMasteryMenuOfDay selectMasteryMenuOfDayService = World.GetExistingSystem<SelectMasteryMenuOfDay>();
            selectMasteryMenuOfDayService.CreateMasteryMenu();
            Mod.Logger.LogInfo("RebuildMasteryKitchen - Done updating ingredients");
        }

        private void AddMasteryMenuComponents(int currentDishID)
        {
            Mod.Logger.LogInfo($"{LogMsgPrefix} - AddMasteryMenuComponents start");
            using var franchiseMenuEntities = FranchiseMenuItems.ToEntityArray(Allocator.Temp);
            using var franchiseMenuItemComps = FranchiseMenuItems.ToComponentDataArray<CMenuItem>(Allocator.Temp);
            using var franchiseMenuOptionEntities = FranchiseMenuOptions.ToEntityArray(Allocator.Temp);
            using var franchiseMenuOptionComps = FranchiseMenuOptions.ToComponentDataArray<CAvailableIngredient>(Allocator.Temp);

            GenericMasteryDishCard dishCard = MasteryDishCrossReference.GetMasteryCardById(currentDishID);

            // Go through each contained Mastery dish, making sure to only use each resulting menu item and
            //  option once by checking against its source dish id to ensure it's not still the card's id
            foreach (GenericMasteryDish MasteryDish in dishCard.ContainedMasteryRecipes)
            {
                Mod.Logger.LogInfo($"{LogMsgPrefix} - AddMasteryMenuComponents() - Starting dish {{{MasteryDish.UniqueNameID}}} with " +
                    $"{{ResultingMenuItems = {{{String.Join(", ", MasteryDish.ResultingMenuItems.Select(mi => $"{mi.Item.ID}:\"{mi.Item.name}\""))}}}}} and " +
                    $"{{IngredientsUnlocks = {{{String.Join(", ", MasteryDish.IngredientsUnlocks.Select(iu => $"({iu.MenuItem.ID}:\"{iu.MenuItem.name}\", {iu.Ingredient.ID}:\"{iu.Ingredient.name}\")"))}}}}}");
                int menuItemCount = 0;
                int attempts = 0;
                bool allFound = false;
                while (menuItemCount < MasteryDish.ResultingMenuItems.Count)
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
                            !MasteryDish.ResultingMenuItems.Any(mItem => mItem.Item.ID == cMenuItem.Item))
                        {
                            continue;
                        }

                        int MasteryDishGDOId = MasteryDish.GameDataObject.ID;
                        cMenuItem.SourceDish = MasteryDishGDOId;
                        EntityManager.AddComponentData(entity, cMenuItem);
                        EntityManager.AddComponentData(entity, new CMasteryMenuItem()
                        {
                            SourceMasteryDish = MasteryDishGDOId,
                            Type = MasteryMenuType.Mastery,
                            HasBeenProvided = false
                        });
                        menuItemCount++;
                        if (menuItemCount >= MasteryDish.ResultingMenuItems.Count)
                        {
                            Mod.Logger.LogInfo($"{LogMsgPrefix} - Found all CMenuItems for recipe {{{MasteryDish.UniqueNameID}}}");
                            allFound = true;
                            break;
                        }
                    }
                    if (!allFound && attempts >= 3)
                    {
                        Mod.Logger.LogWarning($"{LogMsgPrefix} - FAILED to find all CMenuItems for recipe {{{MasteryDish.UniqueNameID}}}");
                        break;
                    }
                }
                int menuOptionCount = 0;
                attempts = 0;
                allFound = false;
                while (menuOptionCount < MasteryDish.IngredientsUnlocks.Count)
                {
                    attempts++;
                    //Mod.Logger.LogInfo($"{LogMsgPrefix} franchiseMenuOptionEntities: " +
                    //    $"{{{String.Join(", ", franchiseMenuOptionComps.Select(option => $"[{option.MenuItem}, {option.Ingredient}]"))}}}");
                    for (int i = 0; i < franchiseMenuOptionEntities.Length; i++)
                    {
                        Entity entity = franchiseMenuOptionEntities[i];
                        CAvailableIngredient cAvailableIngredient = franchiseMenuOptionComps[i];

                        // Skip this entity if it isn't a match (Menu Options don't have duplicates)
                        if (!MasteryDish.IngredientsUnlocks.Any(ui => ui.MenuItem.ID == cAvailableIngredient.MenuItem && ui.Ingredient.ID == cAvailableIngredient.Ingredient))
                        {
                            //Mod.Logger.LogInfo($"{LogMsgPrefix} cAvailableIngredient: (MI: {{{cAvailableIngredient.MenuItem}}}, Ing: {{{cAvailableIngredient.Ingredient}}}");
                            continue;
                        }

                        EntityManager.AddComponentData(entity, new CMasteryMenuItem()
                        {
                            SourceMasteryDish = MasteryDish.GameDataObject.ID,
                            Type = MasteryMenuType.Mastery,
                            HasBeenProvided = false
                        });
                        EntityManager.AddComponent<CMasteryMenuItemOption>(entity);
                        menuOptionCount++;
                        if (menuOptionCount >= MasteryDish.IngredientsUnlocks.Count)
                        {
                            Mod.Logger.LogInfo($"{LogMsgPrefix} - Found all CAvailableIngredients for recipe {{{MasteryDish.UniqueNameID}}}");
                            allFound = true;
                            break;
                        }
                    }
                    if (!allFound && attempts >= 3)
                    {
                        Mod.Logger.LogInfo($"{LogMsgPrefix} - FAILED to find all CAvailableIngredients for recipe {{{MasteryDish.UniqueNameID}}}");
                        break;
                    }
                }
            }
        }

        private void UpdateMasteryIngredients()
        {
            Mod.Logger.LogInfo("RebuildMasteryKitchen - It's the Mastery Menu dish!");
            List<int> itemIDs = GetReplacementIngredients();
            int ingredientIndex = 0;
            using var itemProviders = HqItemProviders.ToEntityArray(Allocator.Temp);
            using var existingCItemProviders = HqItemProviders.ToComponentDataArray<CItemProvider>(Allocator.Temp);
            Mod.Logger.LogInfo("RebuildMasteryKitchen - Updating ingredients");
            for (int i = 0; i < itemProviders.Length; i++)
            {
                var entity = itemProviders[i];
                var cItemProvider = existingCItemProviders[i];

                // Plate & Wok Stacks are also Item Providers, so don't change those.
                if (MasteryDishUtils.IsLimitedContainer(cItemProvider.ProvidedItem))
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
