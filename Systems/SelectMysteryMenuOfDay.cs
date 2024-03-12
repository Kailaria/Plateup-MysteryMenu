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
            MysteryOptions = GetEntityQuery(typeof(CMysteryMenuItemOption), typeof(CMysteryMenuItem));
            MysteryExtras = GetEntityQuery(typeof(CPossibleExtra), typeof(CMysteryMenuItem));
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
            
            //using var mysteryExtraEntities = MysteryExtras.ToEntityArray(Allocator.Temp);
            //using var mysteryExtraRecipes = MysteryExtras.ToComponentDataArray<CMysteryMenuItem>(Allocator.Temp);

            // algo 1: Determine existing, permanently available ingredients
            //      (ignore all possible Dynamic dishes; but this parenthetical should be handled already by HandleNewMysteryMenuDish)
            HashSet<Item> availableItemsForRecipes = FillAvailableItemsFromStaticProviders();

            // algo 2: Sort MysteryMenuItems by whether they've been provided or not
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
            //TODO: also sort Extras separately, but then combine. This way, it's easier to know what to do with each.

            // Combine the entities for ease of access
            List<MysteryRecipeIngredientCounter> olderCombinedEntities = new List<MysteryRecipeIngredientCounter>();
            olderCombinedEntities.AddRange(olderMysteryMenuItemEntities);
            olderCombinedEntities.AddRange(olderMysteryOptionEntities);
            //addrange
            //olderCombinedEntities.ShuffleInPlace(); // No longer shuffling since randomness will be determined by ingredient weight anyway.

            List<MysteryRecipeIngredientCounter> newerCombinedEntities = new List<MysteryRecipeIngredientCounter>();
            newerCombinedEntities.AddRange(newerMysteryMenuItemEntities);
            newerCombinedEntities.AddRange(newerMysteryOptionEntities);
            //addrange
            //newerCombinedEntities.ShuffleInPlace(); // No longer shuffling since randomness will be determined by weight anyway.

            List<Entity> mysteryProviderEntityList = new List<Entity>();
            for (int j = 0; j < mysteryProviderEntities.Length; j++)
            {
                if (mysteryProviderComps[j].Type == MysteryMenuType.Mystery)
                {
                    mysteryProviderEntityList.Add(mysteryProviderEntities[j]);
                }
            }

            // algo 3: Begin selection & randomization loop until all Mystery Providers have been assigned
            int mysteryProviderIndex = 0;
            List<MysteryRecipeIngredientCounter> currentRecipes = new List<MysteryRecipeIngredientCounter>();

            while (mysteryProviderIndex < mysteryProviderEntities.Length)
            {
                Mod.Logger.LogInfo($"{LogMsgPrefix} Provider Loop - Provider index {mysteryProviderIndex} of {mysteryProviderEntities.Length}");
                // algo 3a: Determine available Mystery MenuItem entities that are satisfied with all available ingredients (including those
                //          provided by Mystery Providers)
                AddRecipesToCurrent(currentRecipes, olderCombinedEntities, newerCombinedEntities, availableItemsForRecipes);

                // algo 3b: Inversely weight each recipe based on how many other validly available recipes would also utilize the same ingredients
                //      * Inverse is so that each *ingredient* has a total contributed weight of 1 across all recipes that utilize it
                //      * Prioritize recipes that have not been provided (since that will generally cover ingredients, too, but not always)
                int numRemainingProviders = mysteryProviderEntityList.Count - mysteryProviderIndex;
                WeightRecipesByIngredientSum(currentRecipes, olderCombinedEntities, newerCombinedEntities, availableItemsForRecipes, numRemainingProviders);

                // algo 3c: Randomly select more recipes, one at a time, until the Mystery Providers have all been given their assignments for the day.
                // algo 3d: Ensure that the only recipes that can be selected are those that require at most the number of available ingredient providers,
                //          AND whose prerequisite dishes (if any) have already been satisfied.
                //      * i.e. 2 available providers during a given randomization => only recipes that need 1-2 more ingredients to be valid
                //      * Need to make sure to account for MenuItemTypes (TODO after the algo works for mains alone)
                //      * Need to make sure to account for RequiresVariant/Parent ***HANDLED***
                var selectedRecipeList = SelectRandomRecipe(currentRecipes, newerCombinedEntities, olderCombinedEntities, numRemainingProviders);

                // algo 3e: Given the selected recipe(s), assign the ingredients to Mystery Providers that have not already been provided, whether randomly
                //          or permanently, and add the new ingredients to the availableItemsForRecipes set.
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

        private void AddRecipesToCurrent(
            List<MysteryRecipeIngredientCounter> currentRecipes,
            List<MysteryRecipeIngredientCounter> olderCombinedEntities,
            List<MysteryRecipeIngredientCounter> newerCombinedEntities,
            HashSet<Item> availableItemsForRecipes)
        {
            Mod.Logger.LogInfo($"{LogMsgPrefix} Adding Recipes to 'Current' List that are fully supplied.");
            AddUpdateRecipesToCurrent(currentRecipes, newerCombinedEntities, availableItemsForRecipes, "newer");
            AddUpdateRecipesToCurrent(currentRecipes, olderCombinedEntities, availableItemsForRecipes, "older");
            AddUpdateRequiresVariantMains(currentRecipes, newerCombinedEntities, olderCombinedEntities);
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
                if (recipe.CanBeCooked() && (!recipe.IsMenuItem() || !recipe.RequiresVariant))
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

        private void AddUpdateRequiresVariantMains(List<MysteryRecipeIngredientCounter> currentRecipes,
            List<MysteryRecipeIngredientCounter> newerCombinedRecipes,
            List<MysteryRecipeIngredientCounter> olderCombinedRecipes)
        {
            var allCombinedRecipes = newerCombinedRecipes.Concat(olderCombinedRecipes);
            var requiresVariantMainRecipes = allCombinedRecipes.Where(r => r.IsMenuItem() && r.RequiresVariant).ToList();
            var availableRecipes = currentRecipes.Concat(allCombinedRecipes);
            Mod.Logger.LogInfo($"{LogMsgPrefix} Checking {{count = {requiresVariantMainRecipes.Count}}} Mains that require a variant");
            foreach (var recipe in requiresVariantMainRecipes)
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
            HashSet<Item> availableItemsForRecipes,
            int numRemainingProviders)
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
            var availableMenus = currentRecipes.Select(r => r.Recipe).ToList();
            var allValidEntities = allCombinedEntities
                .Where(e => e.CanBeSelected(numRemainingProviders) && (e.CanBeServed(currentRecipes) || e.CouldBeServed(numRemainingProviders, allCombinedEntities)))
                .ToList();

            // Determine what ingredients there are to choose from (i.e. which ingredients are NOT already available)
            Mod.Logger.LogInfo($"{LogMsgPrefix} — Gathering Missing Ingredients");
            HashSet<Item> missingIngredients = allValidEntities.SelectMany(entity => entity.Recipe.MinimumRequiredMysteryIngredients)
                .Where(item => !availableItemsForRecipes.Contains(item)).ToHashSet();

            // Loop over ingredients
            foreach (Item item in missingIngredients)
            {
                // Add the inverse of the count to the total recipe weight such that each *ingredient* only contributes 1.0f
                //  total weight across all non-current recipes it's in
                List<MysteryRecipeIngredientCounter> recipesMissingThisItem = allValidEntities.Where(e => e.Recipe.MinimumRequiredMysteryIngredients.Contains(item)).ToList();
                Mod.Logger.LogInfo($"{LogMsgPrefix} — Weight Loop — Unavailable Item {{name = {item.name}}} is in {{{recipesMissingThisItem.Count}}} valid, unlocked recipes");
                recipesMissingThisItem.ForEach(r => r.Weight += 1f / recipesMissingThisItem.Count);
            }
            Mod.Logger.LogInfo($"{LogMsgPrefix} Done weighting recipes this loop");
        }

        private List<MysteryRecipeIngredientCounter> SelectRandomRecipe(
            List<MysteryRecipeIngredientCounter> currentRecipes,
            List<MysteryRecipeIngredientCounter> newerCombinedEntities,
            List<MysteryRecipeIngredientCounter> olderCombinedEntities,
            int numRemainingProviders)
        {
            Mod.Logger.LogInfo($"{LogMsgPrefix} Selecting a random recipe");
            var allCombinedEntities = new List<MysteryRecipeIngredientCounter>();
            allCombinedEntities.AddRange(newerCombinedEntities);
            allCombinedEntities.AddRange(olderCombinedEntities);
            float totalNewWeight = newerCombinedEntities
                .Where(e => e.CanBeSelected(numRemainingProviders) && 
                        (e.CanBeServed(currentRecipes) || e.CouldBeServed(numRemainingProviders, allCombinedEntities)))
                .Sum(e => e.Weight);
            float randomNew = UnityEngine.Random.Range(0f, totalNewWeight);
            float currentNew = 0f;

            Mod.Logger.LogInfo($"{LogMsgPrefix} Searching weight range {{0 -> {totalNewWeight}}} for newer recipes. Seeking for {{{randomNew}}}");
            bool found = false;
            var recipeList = new List<MysteryRecipeIngredientCounter>();
            for (int i = 0; i < newerCombinedEntities.Count; i++)
            {
                MysteryRecipeIngredientCounter recipe = newerCombinedEntities[i];
                currentNew += recipe.Weight;
                // If the random number is less than the current, then it's within the range of the previous weight and the current summed weight.
                if (randomNew <= currentNew)
                {
                    Mod.Logger.LogInfo($"{LogMsgPrefix} Found a newer recipe! Name = {{{recipe.Recipe.UniqueNameID}}}; " +
                        $"missingIngredients: {{{recipe.MissingIngredientCount}}}");
                    if (recipe.CanBeSelected(numRemainingProviders))
                    {
                        recipeList.Add(recipe);
                        if (recipe.CanBeServed(currentRecipes))
                        {
                            found = true;
                            break;
                        }
                        if (recipe.CouldBeServed(numRemainingProviders, allCombinedEntities, out var parentRecipe))
                        {
                            recipeList.Add(parentRecipe);
                            found = true;
                            break;
                        }
                    }
                    break;
                }
            }
            if (found)
            {
                HandleSelectedRecipe(currentRecipes, newerCombinedEntities, olderCombinedEntities, recipeList);
                return recipeList;
            }

            float totalOldWeight = olderCombinedEntities
                .Where(e => e.CanBeSelected(numRemainingProviders) &&
                    (e.CanBeServed(currentRecipes) || e.CouldBeServed(numRemainingProviders, allCombinedEntities)))
                .Sum(e => e.Weight);
            float randomOld = UnityEngine.Random.Range(0f, totalOldWeight);
            float currentOld = 0f;

            Mod.Logger.LogInfo($"{LogMsgPrefix} Searching weight range {{0 -> {totalOldWeight}}} for older recipes. Seeking for {{{randomOld}}}");
            for (int j = 0; j < olderCombinedEntities.Count; j++)
            {
                MysteryRecipeIngredientCounter recipe = olderCombinedEntities[j];
                currentOld += recipe.Weight;
                // If the random number is less than the current, then it's within the range of the previous weight and the current summed weight.
                if (randomOld <= currentOld)
                {
                    Mod.Logger.LogInfo($"{LogMsgPrefix} Found an older recipe! Name = {{{recipe.Recipe.UniqueNameID}}}; " +
                        $"missingIngredients: {{{recipe.MissingIngredientCount}}}");
                    if (recipe.CanBeSelected(numRemainingProviders))
                    {
                        recipeList.Add(recipe);
                        if (recipe.CanBeServed(currentRecipes))
                        {
                            found = true;
                            break;
                        }
                        if (recipe.CouldBeServed(numRemainingProviders, allCombinedEntities, out var parentRecipe))
                        {
                            recipeList.Add(parentRecipe);
                            found = true;
                            break;
                        }
                    }
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
            //public CPossibleExtra DishExtra;
            public GenericMysteryDish Recipe;
            public int NumMatchingIngredients;
            public int MissingIngredientCount => Recipe.MinimumRequiredMysteryIngredients.Count() - NumMatchingIngredients;
            public bool RequiresVariant => Recipe.RequiresVariant;
            public float Weight;

            public MysteryRecipeIngredientCounter(Entity entity, int sourceDishID)
            {
                Instantiate(entity, sourceDishID, default, default);
            }

            public MysteryRecipeIngredientCounter(Entity entity, CMenuItem menuItem, int sourceDishID)
            {
                Instantiate(entity, sourceDishID, menuItem, default);
            }

            public MysteryRecipeIngredientCounter(Entity entity, CAvailableIngredient dishOption, int sourceDishID)
            {
                Instantiate(entity, sourceDishID, default, dishOption);
            }

            private void Instantiate(Entity entity, int sourceDishID, CMenuItem menuItem, CAvailableIngredient dishOption)
            {
                Entity = entity;
                Recipe = MysteryDishCrossReference.GetMysteryDishById(sourceDishID);
                NumMatchingIngredients = 0;
                Weight = 0f;
                MenuItem = menuItem;
                DishOption = dishOption;
            }

            public bool CanBeCooked()
            {
                return Recipe.MinimumRequiredMysteryIngredients.Count <= NumMatchingIngredients;
            }

            public bool CanRequiredVariantBeCooked(IEnumerable<MysteryRecipeIngredientCounter> recipes)
            {
                if (!IsMenuItem() || !RequiresVariant)
                {
                    return false;
                }
                return recipes.Where(r => r.DishOption.MenuItem == MenuItem.Item)
                    .Any(r => r.CanBeCooked());
            }

            public bool CanBeSelected(int availableProviderCount)
            {
                return MissingIngredientCount <= availableProviderCount;
            }

            /**
             *  CanBeServed
             *  
             *  Used to determine if it's serveable given the current recipes.
             *  When the entity has a CMenuItem, this will essentially always be true.
             *  When it's a dish option (CAvailableIngredient or CPossibleExtra), then an already serveable recipe needs to be among the current recipes.
             **/
            public bool CanBeServed(IEnumerable<MysteryRecipeIngredientCounter> currentRecipes)
            {
                return (IsMenuItem() && !RequiresVariant)
                    || (IsAvailableIngredient() && currentRecipes.Any(r => r.IsMenuItem() && r.MenuItem.Item == DishOption.MenuItem));
            }

            /**
             * CouldBeServed
             * 
             * When the Recipe can't be served with the current recipes, check to see if it *could* be served if its prerequisite recipe 
             *  can *also* be selected as a pair.
             */
            public bool CouldBeServed(int availableProviderCount, IEnumerable<MysteryRecipeIngredientCounter> nonCurrentRecipes)
            {
                return CouldBeServed(availableProviderCount, nonCurrentRecipes, out _);
            }

            public bool CouldBeServed(int availableProviderCount, IEnumerable<MysteryRecipeIngredientCounter> nonCurrentRecipes, out MysteryRecipeIngredientCounter parentRecipe)
            {
                parentRecipe = GetParentRecipe(nonCurrentRecipes);
                if (IsMenuItem())
                {
                    return CanBeSelected(availableProviderCount);
                }
                if (IsAvailableIngredient())
                {
                    return CanBeSelected(availableProviderCount - parentRecipe.MissingIngredientCount);
                }
                return false;
            }

            public MysteryRecipeIngredientCounter GetParentRecipe(IEnumerable<MysteryRecipeIngredientCounter> availableRecipes)
            {
                if (IsAvailableIngredient())
                {
                    return availableRecipes.Where(r => r.IsMenuItem() && DishOption.MenuItem == r.MenuItem.Item).FirstOrDefault();
                }
                // if (IsPossibleExtra())
                //{
                //}
                return default;
            }

            public void RecalculateMatchingCount(HashSet<Item> availableItems)
            {
                NumMatchingIngredients = Recipe.MinimumRequiredMysteryIngredients.Count(item => availableItems.Any(avaItem => item.ID == avaItem.ID));
            }

            public bool IsMenuItem()
            {
                return MenuItem.Item != 0;
            }

            public bool IsAvailableIngredient()
            {
                return DishOption.MenuItem != 0 && DishOption.Ingredient != 0;
            }
        }
    }
}
