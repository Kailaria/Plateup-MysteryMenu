using Kitchen;
using KitchenData;
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
using UnityEngine;

namespace KitchenMysteryMenu.Systems
{
    public class SelectMysteryMenuOfDay : StartOfDaySystem
    {
        private string LogMsgPrefix = "[SelectMysteryMenuOfDay]";

        EntityQuery MysteryItemProviders;
        EntityQuery MenuItems;
        EntityQuery MysteryOptions;
        EntityQuery MysteryExtras;
        EntityQuery DisabledMenuItems;
        EntityQuery StaticItemProviders;

        protected override void Initialise()
        {
            base.Initialise();
            MysteryItemProviders = GetEntityQuery(typeof(CItemProvider), typeof(CMysteryMenuProvider));
            StaticItemProviders = GetEntityQuery(new QueryHelper()
                .All(typeof(CItemProvider))
                .None(typeof(CMysteryMenuProvider), typeof(CDynamicMenuProvider)));
            MenuItems = GetEntityQuery(typeof(CMenuItem), typeof(CMysteryMenuItem));
            MysteryOptions = GetEntityQuery(typeof(CAvailableIngredient), typeof(CMysteryMenuItemOption), typeof(CMysteryMenuItem));
            MysteryExtras = GetEntityQuery(typeof(CPossibleExtra), typeof(CMysteryMenuItemOption), typeof(CMysteryMenuItem));
            DisabledMenuItems = GetEntityQuery(typeof(CMysteryMenuItem), typeof(CDisabledMysteryMenu));
            RequireForUpdate(MenuItems);
        }

        protected override void OnUpdate()
        {
            Mod.Logger.LogInfo($"{LogMsgPrefix} Begin start-of-day Update");

            // Borrowing a lot of code from SelectFishOfDay, but dish & menu selection will need to be vastly different
            EntityManager.RemoveComponent<CDisabledMysteryMenu>(DisabledMenuItems);
            using var menuItemEntities = MenuItems.ToEntityArray(Allocator.Temp);
            using var menuItemMysteryComps = MenuItems.ToComponentDataArray<CMysteryMenuItem>(Allocator.Temp);
            using var menuItemItemComps = MenuItems.ToComponentDataArray<CMenuItem>(Allocator.Temp);

            using var mysteryProviderEntities = MysteryItemProviders.ToEntityArray(Allocator.Temp);
            using var mysteryProviderComps = MysteryItemProviders.ToComponentDataArray<CMysteryMenuProvider>(Allocator.Temp);

            using var mysteryOptionEntities = MysteryOptions.ToEntityArray(Allocator.Temp);
            using var mysteryOptionRecipes = MysteryOptions.ToComponentDataArray<CMysteryMenuItem>(Allocator.Temp);
            using var mysteryOptionIngredients = MysteryOptions.ToComponentDataArray<CAvailableIngredient>(Allocator.Temp);

            using var mysteryExtraEntities = MysteryExtras.ToEntityArray(Allocator.Temp);
            using var mysteryExtraRecipes = MysteryExtras.ToComponentDataArray<CMysteryMenuItem>(Allocator.Temp);
            using var mysteryExtraExtraComps = MysteryExtras.ToComponentDataArray<CPossibleExtra>(Allocator.Temp);

            // algo 1: Determine existing, permanently available ingredients
            //      (ignore all possible Dynamic dishes; but this parenthetical should be handled already by HandleNewMysteryMenuDish)
            HashSet<Item> availableItemsForRecipes = FillAvailableItemsFromStaticProviders();
            List<MysteryRecipeIngredientCounter> currentRecipes = new List<MysteryRecipeIngredientCounter>();

            // algo 2: Sort MysteryMenuItems by whether they've been provided or not
            // algo 2.5: While sorting, work out which menu phases are possible and already being served.
            Dictionary<MenuPhase, int> minimumIngredientsPerMenuPhase = new Dictionary<MenuPhase, int>();

            Mod.Logger.LogInfo($"{LogMsgPrefix} Sorting CMysteryMenuItem");
            List<MysteryRecipeIngredientCounter> olderMysteryMenuItemEntities = new List<MysteryRecipeIngredientCounter>();
            List<MysteryRecipeIngredientCounter> newerMysteryMenuItemEntities = new List<MysteryRecipeIngredientCounter>();
            for (int i = 0; i < menuItemMysteryComps.Length; i++)
            {
                if (menuItemMysteryComps[i].Type == MysteryMenuType.Mystery)
                {
                    (menuItemMysteryComps[i].HasBeenProvided ? olderMysteryMenuItemEntities : newerMysteryMenuItemEntities)
                        .Add(new MysteryRecipeIngredientCounter(menuItemEntities[i], menuItemItemComps[i], menuItemMysteryComps[i].SourceMysteryDish));
                }
                else
                {
                    // It's not a mystery menu item, so we'll assume its phase is always available since it presumably has static providers.
                    //  and fish will handle its own disabling after mystery stops providing each type fish.
                    currentRecipes.Add(new MysteryRecipeIngredientCounter(menuItemEntities[i], menuItemItemComps[i], menuItemMysteryComps[i].SourceMysteryDish));
                    minimumIngredientsPerMenuPhase[menuItemItemComps[i].Phase] = 0;
                }
            }

            List<MysteryRecipeIngredientCounter> olderMysteryOptionEntities = new List<MysteryRecipeIngredientCounter>();
            List<MysteryRecipeIngredientCounter> newerMysteryOptionEntities = new List<MysteryRecipeIngredientCounter>();
            for (int i = 0; i < mysteryOptionRecipes.Length; i++)
            {
                if (mysteryOptionRecipes[i].Type == MysteryMenuType.Mystery)
                {
                    (mysteryOptionRecipes[i].HasBeenProvided ? olderMysteryOptionEntities : newerMysteryOptionEntities)
                        .Add(new MysteryRecipeIngredientCounter(mysteryOptionEntities[i], mysteryOptionIngredients[i], mysteryOptionRecipes[i].SourceMysteryDish));
                }
            }
            
            List<MysteryRecipeIngredientCounter> olderMysteryExtraEntities = new List<MysteryRecipeIngredientCounter>();
            List<MysteryRecipeIngredientCounter> newerMysteryExtraEntities = new List<MysteryRecipeIngredientCounter>();
            for (int i = 0; i < mysteryExtraRecipes.Length; i++)
            {
                if (mysteryExtraRecipes[i].Type == MysteryMenuType.Mystery)
                {
                    (mysteryExtraRecipes[i].HasBeenProvided ? olderMysteryExtraEntities : newerMysteryExtraEntities)
                        .Add(new MysteryRecipeIngredientCounter(mysteryExtraEntities[i], mysteryExtraExtraComps[i], mysteryExtraRecipes[i].SourceMysteryDish));
                }
            }

            // Combine the entities for ease of access
            List<MysteryRecipeIngredientCounter> olderCombinedEntities = new List<MysteryRecipeIngredientCounter>();
            olderCombinedEntities.AddRange(olderMysteryMenuItemEntities);
            olderCombinedEntities.AddRange(olderMysteryOptionEntities);
            olderCombinedEntities.AddRange(olderMysteryExtraEntities);
            //olderCombinedEntities.ShuffleInPlace(); // No longer shuffling since randomness will be determined by ingredient weight anyway.

            List<MysteryRecipeIngredientCounter> newerCombinedEntities = new List<MysteryRecipeIngredientCounter>();
            newerCombinedEntities.AddRange(newerMysteryMenuItemEntities);
            newerCombinedEntities.AddRange(newerMysteryOptionEntities);
            newerCombinedEntities.AddRange(newerMysteryExtraEntities);
            //newerCombinedEntities.ShuffleInPlace(); // No longer shuffling since randomness will be determined by weight anyway.

            List<Entity> mysteryProviderEntityList = new List<Entity>();
            for (int j = 0; j < mysteryProviderEntities.Length; j++)
            {
                if (mysteryProviderComps[j].Type == MysteryMenuType.Mystery)
                {
                    mysteryProviderEntityList.Add(mysteryProviderEntities[j]);
                }
            }
            mysteryProviderEntityList.ShuffleInPlace();

            // algo 3: Begin selection & randomization loop until all Mystery Providers have been assigned
            int mysteryProviderIndex = 0;
            int failedAttempts = 0;
            int maxFailedAttempts = 3;
            while (mysteryProviderIndex < mysteryProviderEntityList.Count)
            {
                Mod.Logger.LogInfo($"{LogMsgPrefix} Provider Loop - Provider index {mysteryProviderIndex} of {mysteryProviderEntities.Length}");
                // algo 3a: Determine available Mystery MenuItem entities that are satisfied with all available ingredients (including those
                //          provided by Mystery Providers)
                AddRecipesToCurrent(currentRecipes, olderCombinedEntities, newerCombinedEntities, availableItemsForRecipes);

                // algo 3b: Inversely weight each recipe based on how many other validly available recipes would also utilize the same ingredients
                //      * Inverse is so that each *ingredient* has a total contributed weight of 1 across all recipes that utilize it
                //      * Prioritize recipes that have not been provided (since that will generally cover ingredients, too, but not always)
                int numRemainingProviders = mysteryProviderEntityList.Count - mysteryProviderIndex;
                DetermineMenuPhaseIngredientMinimums(minimumIngredientsPerMenuPhase, currentRecipes, newerCombinedEntities, olderCombinedEntities, numRemainingProviders);
                WeightRecipesByIngredientSum(currentRecipes, olderCombinedEntities, newerCombinedEntities, minimumIngredientsPerMenuPhase);

                // algo 3c: Randomly select more recipes, one at a time, until the Mystery Providers have all been given their assignments for the day.
                // algo 3d: Ensure that the only recipes that can be selected are those that require at most the number of available ingredient providers,
                //          AND whose prerequisite dishes (if any) have already been satisfied.
                //      * i.e. 2 available providers during a given randomization => only recipes that need 1-2 more ingredients to be valid
                //      * Need to make sure to account for MenuItemTypes (TODO after the algo works for mains alone)
                //      * Need to make sure to account for RequiresVariant/Parent ***HANDLED***
                var selectedRecipeList = SelectRandomRecipe(currentRecipes, newerCombinedEntities, olderCombinedEntities, minimumIngredientsPerMenuPhase);

                // algo 3e: Given the selected recipe(s), assign the ingredients to Mystery Providers that have not already been provided, whether randomly
                //          or permanently, and add the new ingredients to the availableItemsForRecipes set.
                if (selectedRecipeList == default)
                {
                    failedAttempts++;
                    if (failedAttempts < maxFailedAttempts)
                    {
                        continue;
                    }
                    throw new ArgumentNullException($"{LogMsgPrefix} Failed to properly fill all mystery providers after {failedAttempts} failed attempts.");
                }
                var selectedRecipesIngredients = selectedRecipeList.SelectMany(r => r.Recipe.MinimumRequiredMysteryIngredients).ToHashSet();
                foreach(var ingredient in selectedRecipesIngredients)
                {
                    if (availableItemsForRecipes.Any(item => item.ID == ingredient.ID))
                    {
                        // Don't re-add the ingredient if it's already there. 
                        continue;
                    }
                    var mysteryProviderCItemProvider = EntityManager.GetComponentData<CItemProvider>(mysteryProviderEntityList[mysteryProviderIndex]);
                    mysteryProviderCItemProvider.SetAsItem(ingredient.ID);
                    EntityManager.SetComponentData(mysteryProviderEntityList[mysteryProviderIndex], mysteryProviderCItemProvider);
                    availableItemsForRecipes.Add(ingredient);
                    mysteryProviderIndex++;
                }
            }

            // algo 4: With all Mystery providers assigned, make a final sweep to determine the available recipes, and disable those that cannot be fulfilled
            AddRecipesToCurrent(currentRecipes, olderCombinedEntities, newerCombinedEntities, availableItemsForRecipes);
            DisableUnusedRecipes(newerCombinedEntities);
            DisableUnusedRecipes(olderCombinedEntities);
        }

        private HashSet<Item> FillAvailableItemsFromStaticProviders()
        {
            Mod.Logger.LogInfo($"{LogMsgPrefix} Step 1: Fill available items from static providers (if any)");
            HashSet<Item> availableItemsForRecipes = new HashSet<Item>();
            using var staticProviderEntities = StaticItemProviders.ToEntityArray(Allocator.Temp);
            using var staticItemProviderComps = StaticItemProviders.ToComponentDataArray<CItemProvider>(Allocator.Temp);
            for (int i = 0; i < staticProviderEntities.Length; i++)
            {
                int providedItemID = staticItemProviderComps[i].ProvidedItem;
                // Skip plates, pots, woks, sinks, etc.
                if (MysteryDishUtils.IsLimitedContainer(providedItemID))
                {
                    continue;
                }
                var success = GameData.Main.TryGet(providedItemID, out Item item);
                if (success)
                {
                    availableItemsForRecipes.Add(item);
                }
            }
            Mod.Logger.LogInfo($"{LogMsgPrefix} Number of relevant static ingredients = {availableItemsForRecipes.Count}");
            return availableItemsForRecipes;
        }

        private void DetermineMenuPhaseIngredientMinimums(
            Dictionary<MenuPhase, int> minimumIngredientsPerMenuPhase,
            List<MysteryRecipeIngredientCounter> currentRecipes,
            List<MysteryRecipeIngredientCounter> newerCombinedEntities,
            List<MysteryRecipeIngredientCounter> olderCombinedEntities,
            int numRemainingProviders)
        {
            // Use MenuPhase.Complete to hold the amount of floating providers after minimums are accounted for.
            // However, DON'T modify the actual amounts within the dictionary outside of this method. Only add the actual
            //  phase with Complete.

            // Start off by zeroing phases that are currently being provided.
            foreach (var recipe in currentRecipes)
            {
                // Every PossibleExtra and AvailableIngredient in Current will have a MenuItem parent also in Current, so only check MenuItems.
                // Even if the MenuItem requires a variant, at this point, the variant should also be in Current, so
                //  no special cases are required here.
                if (recipe.IsMenuItem())
                {
                    minimumIngredientsPerMenuPhase[recipe.MenuItem.Phase] = 0;
                }
            }

            // Now check the remaining MenuItems (and if necessary, RequiresVariant children) to place/reset MenuPhases
            //  that aren't being accounted for.
            List<MysteryRecipeIngredientCounter> combinedEntities = newerCombinedEntities.Concat(olderCombinedEntities).ToList();
            foreach (var recipe in combinedEntities)
            {
                if (recipe.RequiresVariant || recipe.IsPossibleExtra())
                {
                    // Requires Variant menu items will be accounted for in AvailableIngredient.
                    continue;
                }
                if (recipe.IsMenuItem())
                {
                    if (!minimumIngredientsPerMenuPhase.TryGetValue(recipe.MenuItem.Phase, out int currentMinimum) ||
                         recipe.MissingIngredients.Count < currentMinimum)
                    {
                        minimumIngredientsPerMenuPhase[recipe.MenuItem.Phase] = recipe.MissingIngredients.Count;
                    }
                }
                else if (recipe.IsAvailableIngredient())
                {
                    // Note: We're only checking *non-added* entities for the parent MenuItem, since if another variant of the Menu Item
                    //  is already Current, then the parent MenuItem has already contributed a 0 to its MenuPhase.
                    var parentMenuItem = recipe.GetParentRecipes(combinedEntities).FirstOrDefault(r => r.IsMenuItem());
                    if (parentMenuItem != default && parentMenuItem.RequiresVariant)
                    {
                        if (!minimumIngredientsPerMenuPhase.TryGetValue(parentMenuItem.MenuItem.Phase, out int currentMinimum) ||
                            recipe.MissingIngredients.Count < currentMinimum)
                        {
                            minimumIngredientsPerMenuPhase[parentMenuItem.MenuItem.Phase] = recipe.MissingIngredients.Count;
                        }
                    }
                }
            }

            // Finally, calculate the amount of flexible providers that can be used for any phase and put it into MenuPhase.Complete.
            int minimumSumOfPhases = minimumIngredientsPerMenuPhase.Where(mipmp => mipmp.Key != MenuPhase.Complete).Sum(mipmp => mipmp.Value);
            minimumIngredientsPerMenuPhase[MenuPhase.Complete] = numRemainingProviders - minimumSumOfPhases;

            // Ideally, this shouldn't happen. However, in the off chance it does, we don't want to be breaking the rest of the algo
            //  by having a negative phase "count" for the flexible providers.
            if (minimumIngredientsPerMenuPhase[MenuPhase.Complete] < 0)
            {
                minimumIngredientsPerMenuPhase[MenuPhase.Complete] = 0;
            }
        }

        private void AddRecipesToCurrent(
            List<MysteryRecipeIngredientCounter> currentRecipes,
            List<MysteryRecipeIngredientCounter> olderCombinedEntities,
            List<MysteryRecipeIngredientCounter> newerCombinedEntities,
            HashSet<Item> availableItemsForRecipes)
        {
            Mod.Logger.LogInfo($"{LogMsgPrefix} Adding Recipes to 'Current' List that are fully supplied.");
            AddUpdateRecipesToCurrent(currentRecipes, newerCombinedEntities, availableItemsForRecipes, "newer");
            AddUpdateRecipesToCurrent(currentRecipes, olderCombinedEntities, availableItemsForRecipes, "older");
            AddUpdateRequiresVariantRecipes(currentRecipes, newerCombinedEntities, olderCombinedEntities);
        }

        private void AddUpdateRecipesToCurrent(List<MysteryRecipeIngredientCounter> currentRecipes, List<MysteryRecipeIngredientCounter> combinedEntities, HashSet<Item> availableItemsForRecipes, string listName)
        {
            int count = 0;
            foreach (var recipe in combinedEntities)
            {
                // Recalculate the count of matching ingredients
                recipe.RecalculateMatchingCount(availableItemsForRecipes);

                // If a recipe can be cooked or one of its required variants can be cooked, then add it.
                // AvailableIngredients should always be added if they can be cooked, since they still require a MenuItem to be plated.
                // PossibleExtras should always be added since, if they can be cooked, they were already selected for a provider slot by another option.
                //      The extra still won't be requested unless its related MenuItem is already there.
                if (recipe.CanBeCooked() && !recipe.RequiresVariant)
                {
                    count++;
                    UpdateRecipeEntity(currentRecipes, recipe);
                }
            }
            combinedEntities.RemoveAll(r => currentRecipes.Contains(r));
            Mod.Logger.LogInfo($"{LogMsgPrefix} Moved {{count = {count}}} from {{{listName}}} to current.");
        }

        private void UpdateRecipeEntity(List<MysteryRecipeIngredientCounter> currentRecipes, MysteryRecipeIngredientCounter recipe)
        {
            currentRecipes.Add(recipe);
            var mysteryMenuComp = EntityManager.GetComponentData<CMysteryMenuItem>(recipe.Entity);
            mysteryMenuComp.HasBeenProvided = true;
            EntityManager.SetComponentData(recipe.Entity, mysteryMenuComp);
        }

        private void AddUpdateRequiresVariantRecipes(List<MysteryRecipeIngredientCounter> currentRecipes,
            List<MysteryRecipeIngredientCounter> newerCombinedRecipes,
            List<MysteryRecipeIngredientCounter> olderCombinedRecipes)
        {
            var allCombinedRecipes = newerCombinedRecipes.Concat(olderCombinedRecipes);
            var requiresVariantRecipes = allCombinedRecipes.Where(r => r.RequiresVariant).ToList();
            var availableRecipes = currentRecipes.Concat(allCombinedRecipes);
            Mod.Logger.LogInfo($"{LogMsgPrefix} Checking {{count = {requiresVariantRecipes.Count}}} Mains that require a variant");
            foreach (var recipe in requiresVariantRecipes)
            {
                if (recipe.CanBeCooked() && recipe.CanRequiredVariantBeCooked(availableRecipes))
                {
                    UpdateRecipeEntity(currentRecipes, recipe);
                }
            }
            var count = newerCombinedRecipes.RemoveAll(r => currentRecipes.Contains(r));
            Mod.Logger.LogInfo($"{LogMsgPrefix} Moved {{count = {count}}} RequiresVariant Mains from newer to current.");
            count = olderCombinedRecipes.RemoveAll(r => currentRecipes.Contains(r));
            Mod.Logger.LogInfo($"{LogMsgPrefix} Moved {{count = {count}}} RequiresVariant Mains from older to current.");
        }

        private void WeightRecipesByIngredientSum(
            List<MysteryRecipeIngredientCounter> currentRecipes,
            List<MysteryRecipeIngredientCounter> olderCombinedEntities,
            List<MysteryRecipeIngredientCounter> newerCombinedEntities,
            Dictionary<MenuPhase, int> minimumIngredientsPerMenuPhase)
        {
            Mod.Logger.LogInfo($"{LogMsgPrefix} Weighting Recipes by Ingredient-Sum");

            // We aren't manipulating the lists themselves, so we can combine them here for easier management
            //  and to account for how many total recipes might be added if a given ingredient is selected.
            var allCombinedEntities = new List<MysteryRecipeIngredientCounter>();
            allCombinedEntities.AddRange(olderCombinedEntities);
            allCombinedEntities.AddRange(newerCombinedEntities);

            // Zero out the weights for everything
            Mod.Logger.LogInfo($"{LogMsgPrefix} — Zeroing weights");
            allCombinedEntities.ForEach(entity => entity.Weight = 0);

            // Only allow recipes that can be fulfilled with the remaining provider count AND either have their prerequisite met
            //  or have the ability to be selected if their prereq dish could be also selected.
            //  If these conditions are met, then consider the entity valid for weighting purposes.
            var allValidEntities = allCombinedEntities
                .Where(e => e.CanBeSelected(e.GetUsableProviderCount(minimumIngredientsPerMenuPhase)) && 
                    (e.CanBeServed(currentRecipes) || 
                        e.CouldBeServed(e.GetUsableProviderCount(minimumIngredientsPerMenuPhase), allCombinedEntities, currentRecipes)))
                .ToList();

            // Get the phases with the maximal amount of minimum ingredients to increase the odds of pulling in more menu phases than just
            //  the one.
            HashSet<MenuPhase> menuPhases = minimumIngredientsPerMenuPhase
                .Where(kvp => kvp.Key != MenuPhase.Complete && kvp.Value > 0
                    && kvp.Value == minimumIngredientsPerMenuPhase.Max(kvp => kvp.Value))
                .Select(kvp => kvp.Key)
                .ToHashSet();
            List<MysteryRecipeIngredientCounter> idealPhaseValidEntities;
            if (menuPhases.Count == 0)
            {
                idealPhaseValidEntities = allValidEntities;
            }
            else
            {
                idealPhaseValidEntities = allValidEntities
                    .Where(e => menuPhases.Contains(e.GetMenuPhase()))
                    .ToList();
            }

            // [2024-03-27] Doing the ingredient-based weights ends up putting too much emphasis on large recipes like cheesy spaghetti.
            //      Gonna try just leaving it at recipe-based weighting, completely even, but maintain existing code
            foreach (var validEntity in idealPhaseValidEntities)
            {
                validEntity.Weight = 1;
            }

            //// Determine what ingredients there are to choose from (i.e. which ingredients are NOT already available)
            //Mod.Logger.LogInfo($"{LogMsgPrefix} — Gathering Missing Ingredients");
            //HashSet<Item> missingIngredients = allValidEntities.SelectMany(entity => entity.MissingIngredients).ToHashSet();

            //// Loop over ingredients
            //foreach (Item item in missingIngredients)
            //{
            //    // Add the inverse of the count to the total recipe weight such that each *ingredient* only contributes 1.0f
            //    //  total weight across all non-current recipes it's in
            //    List<MysteryRecipeIngredientCounter> recipesMissingThisItem = allValidEntities.Where(e => e.MissingIngredients.Contains(item)).ToList();
            //    Mod.Logger.LogInfo($"{LogMsgPrefix} — Weight Loop — Unavailable Item {{name = {item.name}}} is in {{{recipesMissingThisItem.Count}}} valid, unlocked recipes");
            //    recipesMissingThisItem.ForEach(r => r.Weight += 1f / recipesMissingThisItem.Count);
            //}
            Mod.Logger.LogInfo($"{LogMsgPrefix} Done weighting recipes this loop");
        }

        private List<MysteryRecipeIngredientCounter> SelectRandomRecipe(
            List<MysteryRecipeIngredientCounter> currentRecipes,
            List<MysteryRecipeIngredientCounter> newerCombinedEntities,
            List<MysteryRecipeIngredientCounter> olderCombinedEntities,
            Dictionary<MenuPhase, int> minimumIngredientsPerMenuPhase)
        {
            var methodPrefix = "SelectRandomRecipe()";
            Mod.Logger.LogInfo($"{LogMsgPrefix} Selecting a random recipe");
            var allCombinedEntities = new List<MysteryRecipeIngredientCounter>();
            allCombinedEntities.AddRange(newerCombinedEntities);
            allCombinedEntities.AddRange(olderCombinedEntities);
            float totalNewWeight = newerCombinedEntities
                .Where(e => e.CanBeSelected(e.GetUsableProviderCount(minimumIngredientsPerMenuPhase)) && 
                        (e.CanBeServed(currentRecipes) || 
                            e.CouldBeServed(e.GetUsableProviderCount(minimumIngredientsPerMenuPhase), allCombinedEntities, currentRecipes)))
                .Sum(e => e.Weight);
            float randomNew = UnityEngine.Random.Range(0f, totalNewWeight);
            float currentNew = 0f;

            Mod.Logger.LogInfo($"{LogMsgPrefix} {methodPrefix} Searching weight range {{0 -> {totalNewWeight}}} for newer recipes. Seeking for {{{randomNew}}}");
            bool found = false;
            var recipeList = new List<MysteryRecipeIngredientCounter>();
            for (int i = 0; i < newerCombinedEntities.Count; i++)
            {
                MysteryRecipeIngredientCounter recipe = newerCombinedEntities[i];
                int numRemainingProviders = recipe.GetUsableProviderCount(minimumIngredientsPerMenuPhase);
                currentNew += recipe.Weight;
                // If the random number is less than the current, then it's within the range of the previous weight and the current summed weight.
                if (randomNew <= currentNew)
                {
                    Mod.Logger.LogInfo($"{LogMsgPrefix} Found a newer recipe! Name = {{{recipe.Recipe.UniqueNameID}}}; " +
                        $"missingIngredients: {{{recipe.MissingIngredients.Count}}}");
                    if (recipe.CanBeSelected(numRemainingProviders))
                    {
                        if (recipe.CanBeServed(currentRecipes))
                        {
                            Mod.Logger.LogInfo($"{LogMsgPrefix} {methodPrefix} - It can be selected and served alone!");
                            recipeList.Add(recipe);
                            found = true;
                            break;
                        }
                        if (recipe.CouldBeServed(numRemainingProviders, allCombinedEntities, currentRecipes, out var parentRecipes))
                        {
                            Mod.Logger.LogInfo($"{LogMsgPrefix} {methodPrefix} - It can be selected and served with {{count = {parentRecipes.Count()}}} parent recipes!");
                            recipeList.Add(recipe);
                            recipeList.AddRange(parentRecipes);
                            found = true;
                            break;
                        }
                    }
                    Mod.Logger.LogInfo($"{LogMsgPrefix} {methodPrefix} - It cannot be selected or served..?");
                    break;
                }
                else
                {
                    Mod.Logger.LogInfo($"{LogMsgPrefix} {methodPrefix} - Skipping Name = {{{recipe.Recipe.UniqueNameID}}}; " +
                        $"missingIngredients: {{{recipe.MissingIngredients.Count}}}");
                }
            }
            if (found)
            {
                HandleSelectedRecipe(currentRecipes, newerCombinedEntities, olderCombinedEntities, recipeList);
                return recipeList;
            }

            float totalOldWeight = olderCombinedEntities
                .Where(e => e.CanBeSelected(e.GetUsableProviderCount(minimumIngredientsPerMenuPhase)) &&
                    (e.CanBeServed(currentRecipes) || 
                        e.CouldBeServed(e.GetUsableProviderCount(minimumIngredientsPerMenuPhase), allCombinedEntities, currentRecipes)))
                .Sum(e => e.Weight);
            float randomOld = UnityEngine.Random.Range(0f, totalOldWeight);
            float currentOld = 0f;
            recipeList = new List<MysteryRecipeIngredientCounter>();

            Mod.Logger.LogInfo($"{LogMsgPrefix} Searching weight range {{0 -> {totalOldWeight}}} for older recipes. Seeking for {{{randomOld}}}");
            for (int j = 0; j < olderCombinedEntities.Count; j++)
            {
                MysteryRecipeIngredientCounter recipe = olderCombinedEntities[j];
                int numRemainingProviders = recipe.GetUsableProviderCount(minimumIngredientsPerMenuPhase);
                currentOld += recipe.Weight;
                // If the random number is less than the current, then it's within the range of the previous weight and the current summed weight.
                if (randomOld <= currentOld)
                {
                    Mod.Logger.LogInfo($"{LogMsgPrefix} Found an older recipe! Name = {{{recipe.Recipe.UniqueNameID}}}; " +
                        $"missingIngredients: {{{recipe.MissingIngredients.Count}}}");
                    if (recipe.CanBeSelected(numRemainingProviders))
                    {
                        if (recipe.CanBeServed(currentRecipes))
                        {
                            Mod.Logger.LogInfo($"{LogMsgPrefix} {methodPrefix} - It can be selected and served alone!");
                            recipeList.Add(recipe);
                            found = true;
                            break;
                        }
                        if (recipe.CouldBeServed(numRemainingProviders, allCombinedEntities, currentRecipes, out var parentRecipes))
                        {
                            Mod.Logger.LogInfo($"{LogMsgPrefix} {methodPrefix} - It can be selected and served with {{count = {parentRecipes.Count()}}} parent recipes!");
                            recipeList.Add(recipe);
                            recipeList.AddRange(parentRecipes);
                            found = true;
                            break;
                        }
                    }
                    Mod.Logger.LogInfo($"{LogMsgPrefix} {methodPrefix} - It cannot be selected or served..?");
                    break;
                }
            }
            if (found)
            {
                HandleSelectedRecipe(currentRecipes, newerCombinedEntities, olderCombinedEntities, recipeList);
                return recipeList;
            }
            Mod.Logger.LogWarning($"{LogMsgPrefix} Somehow failed to find a valid (Selectable, Servable) recipe.");
            return default;
        }

        private void HandleSelectedRecipe(List<MysteryRecipeIngredientCounter> currentRecipes, List<MysteryRecipeIngredientCounter> newerCombinedEntities, List<MysteryRecipeIngredientCounter> olderCombinedEntities, List<MysteryRecipeIngredientCounter> selectedRecipes)
        {
            currentRecipes.AddRange(selectedRecipes);
            foreach (var recipe in selectedRecipes)
            {
                var mysteryComp = EntityManager.GetComponentData<CMysteryMenuItem>(recipe.Entity);
                mysteryComp.HasBeenProvided = true;
                EntityManager.SetComponentData(recipe.Entity, mysteryComp);
            }
            newerCombinedEntities.RemoveAll(r => selectedRecipes.Contains(r));
            olderCombinedEntities.RemoveAll(r => selectedRecipes.Contains(r));
        }

        private void DisableUnusedRecipes(List<MysteryRecipeIngredientCounter> combinedEntities)
        {
            foreach (var recipe in combinedEntities)
            {
                EntityManager.AddComponent<CDisabledMysteryMenu>(recipe.Entity);
            }
        }

        internal class MysteryRecipeIngredientCounter
        {
            public Entity Entity;
            public CMenuItem MenuItem;
            public CAvailableIngredient DishOption;
            public CPossibleExtra DishExtra;
            public GenericMysteryDish Recipe;
            public HashSet<Item> MissingIngredients;
            public bool RequiresVariant => Recipe.RequiresVariant;
            public float Weight;

            public MysteryRecipeIngredientCounter(Entity entity, int sourceDishID)
            {
                Instantiate(entity, sourceDishID, default, default, default);
            }

            public MysteryRecipeIngredientCounter(Entity entity, CMenuItem menuItem, int sourceDishID)
            {
                Instantiate(entity, sourceDishID, menuItem, default, default);
            }

            public MysteryRecipeIngredientCounter(Entity entity, CAvailableIngredient dishOption, int sourceDishID)
            {
                Instantiate(entity, sourceDishID, default, dishOption, default);
            }

            public MysteryRecipeIngredientCounter(Entity entity, CPossibleExtra dishExtra, int sourceDishID)
            {
                Instantiate(entity, sourceDishID, default, default, dishExtra);
            }

            private void Instantiate(Entity entity, int sourceDishID, CMenuItem menuItem, CAvailableIngredient dishOption, CPossibleExtra dishExtra)
            {
                Entity = entity;
                Recipe = MysteryDishCrossReference.GetMysteryDishById(sourceDishID);
                MissingIngredients = Recipe.MinimumRequiredMysteryIngredients.Select(i => i).ToHashSet();
                Weight = 0f;
                MenuItem = menuItem;
                DishOption = dishOption;
                DishExtra = dishExtra;
            }

            public bool CanBeCooked()
            {
                return MissingIngredients.Count == 0;
            }

            public bool CanRequiredVariantBeCooked(IEnumerable<MysteryRecipeIngredientCounter> recipes)
            {
                var logID = "[MRIC.CanRequiredVariantBeCooked()]";
                if (!RequiresVariant)
                {
                    // All Extras won't require a variant. Most Options won't require a variant
                    Mod.Logger.LogInfo($"{logID} - Recipe {{{Recipe.UniqueNameID}}} doesn't require variant, so returning false.");
                    return false;
                }
                if (IsMenuItem())
                {
                    IEnumerable<MysteryRecipeIngredientCounter> dishOptions = recipes.Where(r => IsParentOf(r) && r.IsAvailableIngredient());
                    bool anyIndependentChildCanBeCooked = dishOptions.Where(r => !r.RequiresVariant).Any(r => r.CanBeCooked());
                    bool allDependentChildrenCanBeCooked = dishOptions.Where(r => r.RequiresVariant).All(r => r.CanBeCooked() && r.CanRequiredVariantBeCooked(recipes));
                    Mod.Logger.LogInfo($"{logID} - Recipe {{{Recipe.UniqueNameID}}} is MenuItem. Independent child recipe can be cooked = {{{anyIndependentChildCanBeCooked}}}. " +
                        $"All dependent children recipes can be cooked = {{{allDependentChildrenCanBeCooked}}}");
                    return anyIndependentChildCanBeCooked && allDependentChildrenCanBeCooked;
                }
                // This has a valid DishOption/Extra, so check if other Options that aren't the same Ingredient can be cooked.
                //  (Should hopefully address Rice-only Stir Fry and null reference)
                bool anySiblingCanBeCooked = recipes.Where(r => IsSiblingOf(r) && r.IsAvailableIngredient())
                                    .Any(r => r.CanBeCooked());
                Mod.Logger.LogInfo($"{logID} - Recipe {{{Recipe.UniqueNameID}}} is DishOption. Any one sibling recipe can be cooked = {{{anySiblingCanBeCooked}}}");
                return anySiblingCanBeCooked;
            }

            public int GetUsableProviderCount(Dictionary<MenuPhase, int> availableProvidersPerPhase)
            {
                int availableProviderCount = 0;
                MenuPhase phase = GetMenuPhase();
                if (availableProvidersPerPhase.Where(appp => appp.Key != MenuPhase.Complete).Sum(appp => appp.Value) == 0)
                {
                    availableProviderCount = availableProvidersPerPhase[MenuPhase.Complete];
                }
                else if (availableProvidersPerPhase[phase] > 0)
                {
                    availableProviderCount = availableProvidersPerPhase[phase] + availableProvidersPerPhase[MenuPhase.Complete];
                }
                return availableProviderCount;
            }

            public bool CanBeSelected(int availableProviderCount)
            {
                Mod.Logger.LogInfo($"[MRIC.CanBeSelected()] Recipe = {{{Recipe.UniqueNameID}}}, " +
                    $"MissingIngredientCount = {{{MissingIngredients.Count}}}, Available Providers = {{{availableProviderCount}}}");
                return MissingIngredients.Count <= availableProviderCount;
            }

            /**
             *  CanBeServed
             *  
             *  Used to determine if it's serveable given the current recipes. Should be true whenever the prereqs are in current, or the dish
             *      has no prereqs.
             *  When the entity has a CMenuItem, this will essentially always be true except if it *requires* a variant.
             *  When it's a dish option (CAvailableIngredient or CPossibleExtra), then an already serveable recipe needs to be among the current recipes.
             **/
            public bool CanBeServed(IEnumerable<MysteryRecipeIngredientCounter> currentRecipes)
            {
                var logKey = "[MRIC.CanBeServed()]";
                var currentRequiresVariantRecipes = currentRecipes.Where(r => r.RequiresVariant);
                var currentTerminusRecipes = currentRecipes.Where(r => !r.RequiresVariant);
                if (IsMenuItem())
                {
                    bool canMenuItemBeServed = !RequiresVariant ||
                        currentRecipes.Any(r => r.IsAvailableIngredient() && IsParentOf(r) && !r.RequiresVariant);
                    Mod.Logger.LogInfo($"{logKey} - MenuItem {{name = {Recipe.UniqueNameID}, requiresVariant = {RequiresVariant}, canBeServed = {canMenuItemBeServed}}}");
                    return canMenuItemBeServed;
                }
                if (IsAvailableIngredient())
                {
                    bool canOptionBeServed = currentRecipes.Any(r => r.IsMenuItem() && IsChildOf(r)) &&
                                        (!RequiresVariant || currentRecipes.Where(r => r.IsAvailableIngredient() && IsSiblingOf(r)).Any(r => !r.RequiresVariant));
                    Mod.Logger.LogInfo($"{logKey} - DishOption {{name = {Recipe.UniqueNameID}, requiresVariant = {RequiresVariant}, canBeServed = {canOptionBeServed}");
                    return canOptionBeServed;
                }

                bool canExtraBeServed = IsPossibleExtra() && currentRecipes.Any(r => r.IsMenuItem() && IsChildOf(r)) &&
                                    currentRecipes.Any(r => !r.RequiresVariant && IsSiblingOf(r));
                Mod.Logger.LogInfo($"{logKey} - DishOption {{name = {Recipe.UniqueNameID}, requiresVariant = {RequiresVariant}, canBeServed = {canExtraBeServed}");
                return canExtraBeServed;
            }

            /**
             * CouldBeServed
             * 
             * When the Recipe can't be served with *only* the current recipes, check to see if it *could* be served if non-current prerequisite recipes 
             *  can *also* be selected as a group.
             */
            public bool CouldBeServed(int availableProviderCount, IEnumerable<MysteryRecipeIngredientCounter> nonCurrentRecipes, IEnumerable<MysteryRecipeIngredientCounter> currentRecipes)
            {
                return CouldBeServed(availableProviderCount, nonCurrentRecipes, currentRecipes, out _);
            }

            public bool CouldBeServed(int availableProviderCount, IEnumerable<MysteryRecipeIngredientCounter> nonCurrentRecipes, IEnumerable<MysteryRecipeIngredientCounter> currentRecipes, out List<MysteryRecipeIngredientCounter> parentRecipes)
            {
                var logKey = "[MRIC.CouldBeServed()]";
                var allUnlockedRecipes = nonCurrentRecipes.Concat(currentRecipes);
                parentRecipes = GetParentRecipes(allUnlockedRecipes);
                // If this recipe requires a child variant in order to be served, let the child pull it in instead.
                if (RequiresVariant)
                {
                    Mod.Logger.LogInfo($"{logKey} Recipe {{{Recipe.UniqueNameID}}} requires a variant. " +
                        $"Type = {{{TypeName}}}");
                    return false;
                }
                // If this recipe has no parent recipes and isn't a MenuItem entity, then the parent's card hasn't been selected and isn't available
                //  for this option/extra.
                if (parentRecipes.Count() == 0 && !IsMenuItem())
                {
                    Mod.Logger.LogInfo($"{logKey} Recipe {{{Recipe.UniqueNameID}}} has no valid parents. " +
                        $"Type = {{{TypeName}}}");
                    return false;
                }
                // TODO: This might need to handle Side-type MenuItems and ensuring there's a Main already to attach it to, but perhaps
                //  that'll be handled already by a different process that allots the minimum amount of ingredients applied to each course, thus
                //  making this block redundant with the above two blocks (RequiresVariant & 0 parents as of 2024-03-19).
                if (IsMenuItem())
                {
                    Mod.Logger.LogInfo($"{logKey} MenuItem {{{Recipe.UniqueNameID}}} - ");
                    return CanBeSelected(availableProviderCount);
                }
                // This is where we ensure that the parent recipes get added and accounted for
                if (IsAvailableIngredient())
                {
                    // Only count ingredients that aren't in this recipe's missing ingredients to not double count, say, rice.
                    HashSet<Item> parentMissingIngredients = parentRecipes.SelectMany(r => r.MissingIngredients)
                        .Where(item => !MissingIngredients.Contains(item))
                        .ToHashSet();
                    int parentMissingIngredientSum = parentMissingIngredients.Count();
                    Mod.Logger.LogInfo($"{logKey} AvailableIngredient {{Recipe = {Recipe.UniqueNameID}, MenuItem = " +
                        $"{Recipe.IngredientsUnlocks.First(iu => iu.Ingredient.ID == DishOption.Ingredient && iu.MenuItem.ID == DishOption.MenuItem).MenuItem.name}," +
                        $" ParentRecipes = {parentRecipes}, ParentDistinctMissingIngredientSum = {parentMissingIngredientSum}}}");
                    return CanBeSelected(availableProviderCount - parentMissingIngredientSum);
                }
                // Possible Extras always need their parent recipe to be "Could Be Served", and as such
                if (IsPossibleExtra())
                {
                    HashSet<Item> parentMissingIngredients = parentRecipes.SelectMany(r => r.MissingIngredients)
                        .Where(item => !MissingIngredients.Contains(item))
                        .ToHashSet();
                    int parentMissingIngredientSum = parentMissingIngredients.Count();
                    Mod.Logger.LogInfo($"{logKey} PossibleExtra {{Recipe = {Recipe.UniqueNameID}, MenuItem = " +
                        $"{Recipe.ExtraOrderUnlocks.First(iu => iu.Ingredient.ID == DishExtra.Ingredient && iu.MenuItem.ID == DishExtra.MenuItem).MenuItem.name}," +
                        $" ParentRecipes = {parentRecipes}, ParentDistinctMissingIngredientSum = {parentMissingIngredientSum}}}");
                    return CanBeSelected(availableProviderCount - parentMissingIngredientSum);
                }
                Mod.Logger.LogInfo($"{logKey} Recipe {{{Recipe.UniqueNameID}}} could not be served. It's not a menu item or available ingredient.");
                return false;
            }

            public List<MysteryRecipeIngredientCounter> GetParentRecipes(IEnumerable<MysteryRecipeIngredientCounter> availableRecipes)
            {
                if (IsAvailableIngredient())
                {
                    // Menu Item only cares about child relationship, especially once toppings/sauces/extras get involved.
                    // Siblings are only "parents" if the sibling of this recipe Requires Variant.
                    var parents = availableRecipes
                        .Where(r => (r.IsMenuItem() && IsChildOf(r)) || (r.RequiresVariant && r.IsAvailableIngredient() && IsSiblingOf(r)))
                        .ToList();
                    return parents;
                }
                if (IsPossibleExtra())
                {
                    // Extras need a completely servable recipe. So, get all true parents, and then a sibling (if relevant).
                    var parents = availableRecipes
                        .Where(r => (r.IsMenuItem() && IsChildOf(r)) || (r.IsAvailableIngredient() && r.RequiresVariant && IsSiblingOf(r)))
                        .ToList();
                    var completingSiblings = availableRecipes
                        .Where(r => !r.RequiresVariant && r.IsAvailableIngredient() && IsSiblingOf(r))
                        .ToList();
                    // Only add a sibling if any parents require a variant
                    if (parents.Any(r => r.RequiresVariant) && completingSiblings.Count > 0)
                    {
                        var minimumMissingIngredients = completingSiblings.Min(r => r.MissingIngredients.Count);
                        completingSiblings.ShuffleInPlace();
                        var completingSibling = completingSiblings.Where(r => r.MissingIngredients.Count == minimumMissingIngredients).First();
                        parents.Add(completingSibling);
                    }
                    return parents;
                }
                return default;
            }

            public void RecalculateMatchingCount(HashSet<Item> availableItems)
            {
                MissingIngredients.RemoveWhere(i => availableItems.Contains(i));
            }

            public bool IsMenuItem()
            {
                return MenuItem.Item != 0;
            }

            public bool IsAvailableIngredient()
            {
                return DishOption.MenuItem != 0 && DishOption.Ingredient != 0;
            }

            public bool IsPossibleExtra()
            {
                return DishExtra.MenuItem != 0 && DishExtra.Ingredient != 0;
            }

            public bool IsParentOf(MysteryRecipeIngredientCounter other)
            {
                if (other.IsAvailableIngredient())
                {
                    return MenuItem.Item == other.DishOption.MenuItem;
                }
                if (other.IsPossibleExtra())
                {
                    return MenuItem.Item == other.DishExtra.MenuItem;
                }

                return false;
            }

            public bool IsChildOf(MysteryRecipeIngredientCounter other)
            {
                if (IsAvailableIngredient())
                {
                    return DishOption.MenuItem == other.MenuItem.Item;
                }
                if (IsPossibleExtra())
                {
                    return DishExtra.MenuItem == other.MenuItem.Item;
                }
                return false;
            }

            public bool IsSiblingOf(MysteryRecipeIngredientCounter other)
            {
                // If they're the same type, make sure they aren't the same ingredient
                if (IsAvailableIngredient() && other.IsAvailableIngredient())
                {
                    return DishOption.MenuItem == other.DishOption.MenuItem && DishOption.Ingredient != other.DishOption.Ingredient;
                }
                if (IsPossibleExtra() && other.IsPossibleExtra())
                {
                    return DishExtra.MenuItem == other.DishExtra.MenuItem && DishExtra.Ingredient != other.DishExtra.Ingredient;
                }

                // If they're different types, just ensure they are for the same menu item.
                if (IsAvailableIngredient() && other.IsPossibleExtra())
                {
                    return DishOption.MenuItem == other.DishExtra.MenuItem;
                }
                if (IsPossibleExtra() && other.IsAvailableIngredient())
                {
                    return DishExtra.MenuItem == other.DishOption.MenuItem;
                }

                // Any other combo isn't a sibling relationship.
                return false;
            }

            public MenuPhase GetMenuPhase()
            {
                if (IsMenuItem())
                {
                    return MenuItem.Phase;
                }
                return Recipe.MenuPhase;
            }

            public string TypeName => IsMenuItem() ? "MenuItem" 
                : (IsAvailableIngredient() ? "AvailableIngredient" 
                : (IsPossibleExtra() ? "PossibleExtra" 
                : "type not found"));
        }
    }
}
