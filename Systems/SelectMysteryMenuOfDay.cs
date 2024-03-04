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

namespace KitchenMysteryMenu.Systems
{
    public class SelectMysteryMenuOfDay : StartOfDaySystem
    {
        private string LogMsgPrefix = "[SelecteMysteryMenuOfDay]";

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
            using var mysteryProviderEntities = MysteryItemProviders.ToEntityArray(Allocator.Temp);
            using var mysteryProviderComps = MysteryItemProviders.ToComponentDataArray<CMysteryMenuProvider>(Allocator.Temp);
            using var mysteryOptionEntities = MysteryOptions.ToEntityArray(Allocator.Temp);
            using var mysteryOptionRecipes = MysteryOptions.ToComponentDataArray<CMysteryMenuItem>(Allocator.Temp);
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
                        .Add(new MysteryRecipeIngredientCounter(menuItemEntities[i], menuItemMysteryComps[i].SourceMysteryDish));
                }
            }

            List<MysteryRecipeIngredientCounter> olderMysteryOptionEntities = new List<MysteryRecipeIngredientCounter>();
            List<MysteryRecipeIngredientCounter> newerMysteryOptionEntities = new List<MysteryRecipeIngredientCounter>();
            for (int i = 0; i < mysteryOptionRecipes.Length; i++)
            {
                if (mysteryOptionRecipes[i].Type == MysteryMenuType.Mystery)
                {
                    (mysteryOptionRecipes[i].HasBeenProvided ? olderMysteryOptionEntities : newerMysteryOptionEntities)
                        .Add(new MysteryRecipeIngredientCounter(mysteryOptionEntities[i], mysteryOptionRecipes[i].SourceMysteryDish));
                }
            }
            //TODO: also sort Options and Extras separately, but then combine. This way, it's easier to know what to do with each.

            // Combine the entities for ease of access
            List<MysteryRecipeIngredientCounter> olderCombinedEntities = new List<MysteryRecipeIngredientCounter>();
            olderCombinedEntities.AddRange(olderMysteryMenuItemEntities);
            olderCombinedEntities.AddRange(olderMysteryOptionEntities);
            //addrange
            //olderCombinedEntities.ShuffleInPlace(); // No longer shuffling since randomness will be determined by weight anyway.

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
                WeightRecipesByIngredientSum(currentRecipes, olderCombinedEntities, newerCombinedEntities, availableItemsForRecipes, mysteryProviderEntityList.Count - mysteryProviderIndex);

                // algo 3c: Randomly select more recipes, one at a time, until the Mystery Providers have all been given their assignments for the day.

                // algo 3d: Ensure that the only recipes that can be selected are those that require at most the number of available ingredient providers,
                //          AND whose prerequisite dishes (if any) have already been satisfied.
                //      * i.e. 2 available providers during a given randomization => only recipes that need 1-2 more ingredients to be valid
                //      * Need to make sure 

                // algo 3e: Given the selected recipe, assign the ingredients to Mystery Providers that have not already been provided, whether randomly
                //          or permanently, and add the new ingredients to the availableItemsForRecipes set.
                break;
            }

            // algo 4: With all Mystery providers assigned, make a final sweep to determine the available recipes, and disable those that cannot be fulfilled
            AddRecipesToCurrent(currentRecipes, olderCombinedEntities, newerCombinedEntities, availableItemsForRecipes);
            // DisableUnusedRecipes(olderCombinedEntities, newerCombinedEntities)
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
            AddUpdateRecipesToCurrent(currentRecipes, olderCombinedEntities, availableItemsForRecipes, "older");
            AddUpdateRecipesToCurrent(currentRecipes, newerCombinedEntities, availableItemsForRecipes, "newer");
        }

        private void AddUpdateRecipesToCurrent(List<MysteryRecipeIngredientCounter> currentRecipes, List<MysteryRecipeIngredientCounter> combinedEntities, HashSet<Item> availableItemsForRecipes, string listName)
        {
            int count = 0;
            foreach (var recipe in combinedEntities)
            {
                // Recalculate the count of matching ingredients
                recipe.RecalculateMatchingCount(availableItemsForRecipes);
                if (recipe.CanBeCooked())
                {
                    count++;
                    currentRecipes.Add(recipe);
                }
            }
            combinedEntities.RemoveAll(r => currentRecipes.Contains(r));
            Mod.Logger.LogInfo($"{LogMsgPrefix} Moved {{count = {count}}} from {{{listName}}} to current.");
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

            // Only allow recipes that can be fulfilled with the remaining provider count AND have their prerequisite met
            //  to be valid for weighting purposes.
            var availableMenus = currentRecipes.Select(r => r.Recipe).ToList();
            var allValidEntities = allCombinedEntities.Where(e => e.CanBeSelected(numRemainingProviders) && 
                    (e.Recipe.BaseMysteryDish == default || availableMenus.Contains(e.Recipe.BaseMysteryDish))).ToList();

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

        internal class MysteryRecipeIngredientCounter
        {
            public Entity Entity;
            public GenericMysteryDish Recipe;
            public int NumMatchingIngredients;
            public float Weight;

            public MysteryRecipeIngredientCounter(Entity entity, int sourceDishID)
            {
                Entity = entity;
                Recipe = MysteryDishCrossReference.GetMysteryDishById(sourceDishID);
                NumMatchingIngredients = 0;
                Weight = 0f;
            }

            public bool CanBeCooked()
            {
                return Recipe.MinimumRequiredMysteryIngredients.Count <= NumMatchingIngredients;
            }

            public bool CanBeSelected(int availableProviderCount)
            {
                return Recipe.MinimumRequiredMysteryIngredients.Count <= (NumMatchingIngredients + availableProviderCount);
            }

            public void RecalculateMatchingCount(HashSet<Item> availableItems)
            {
                NumMatchingIngredients = Recipe.MinimumRequiredMysteryIngredients.Count(item => availableItems.Contains(item));
            }
        }
    }
}
