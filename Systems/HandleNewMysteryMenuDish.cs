using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Components;
using KitchenMysteryMenu.Customs.Dishes;
using KitchenMysteryMenu.Utils;
using MysteryMenu.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMysteryMenu.Systems
{
    public class HandleNewMysteryMenuDish : RestaurantSystem
    {
        private EntityQuery MysteryProviders;
        private EntityQuery NonHandledMenuItems;
        private EntityQuery NonHandledMenuOptions;
        private EntityQuery NonHandledPossibleExtras;
        private EntityQuery AnythingNotHandled;

        protected override void Initialise()
        {
            base.Initialise();
            MysteryProviders = GetEntityQuery(typeof(CItemProvider), typeof(CMysteryMenuProvider));
            NonHandledMenuItems = GetEntityQuery(new QueryHelper()
                .All(typeof(CMenuItem), typeof(CDynamicMenuItem))
                .None(typeof(CMysteryMenuItem)));
            // Don't care about entities that are both a CMenuItem & CAvailableIngredient as those are handled above
            NonHandledMenuOptions = GetEntityQuery(new QueryHelper()
                .All(typeof(CAvailableIngredient))
                .None(typeof(CMenuItem), typeof(CMysteryMenuItemOption), typeof(CNonMysteryAvailableIngredient)));
            NonHandledPossibleExtras = GetEntityQuery(new QueryHelper()
                .All(typeof(CPossibleExtra))
                .None(typeof(CMysteryMenuItemOption), typeof(CNonMysteryExtra)));
            AnythingNotHandled = GetEntityQuery(new QueryHelper()
                .Any(typeof(CMenuItem), typeof(CAvailableIngredient), typeof(CPossibleExtra))
                .None(typeof(CMysteryMenuItem), typeof(CNonMysteryAvailableIngredient), 
                    typeof(CMysteryMenuItemOption), typeof(CNonMysteryExtra)));
            RequireForUpdate(MysteryProviders);
            RequireForUpdate(AnythingNotHandled);
        }

        protected override void OnUpdate()
        {
            Mod.Logger.LogInfo("HandleNewMysteryMenuDish updating");
            // TODO: Refer to HandleNewDish system
            // Check for DynamicMenuType = References.DynamicMenuTypeMystery
            // Add CMysteryMenuItem to whatever has CDynamicMenuItem and is actually of type Mystery
            HandleNewMenuItems();
            HandleNewMenuOptions();
            HandleNewExtras();
        }

        private void HandleNewMenuItems()
        {
            using var newMenuEntities = NonHandledMenuItems.ToEntityArray(Allocator.Temp);
            using var cMenuItems = NonHandledMenuItems.ToComponentDataArray<CMenuItem>(Allocator.Temp);
            using var cDynamicMenuItems = NonHandledMenuItems.ToComponentDataArray<CDynamicMenuItem>(Allocator.Temp);
            for (int i = 0; i < newMenuEntities.Length; i++)
            {
                var entity = newMenuEntities[i];
                var menuItem = cMenuItems[i];
                var dynamicMenuItem = cDynamicMenuItems[i];

                NativeArray<int> ingredients = new NativeArray<int>(References.MaxIngredientCountForMinimumRecipe, Allocator.Temp);
                MysteryMenuType type;
                bool requiresVariant = false;
                if (dynamicMenuItem.Type == DynamicMenuType.Static)
                {
                    // Utilize the related Mystery Dish to get the Static Dish and compare
                    Dish staticDish = MysteryDishCrossReference.GetRelatedMysteryDish(menuItem.SourceDish).OrigDish;
                    int iIndex = 0;
                    foreach (Item ingredient in staticDish.MinimumIngredients)
                    {
                        if (ingredient.ID == ItemReferences.Plate || ingredient.ID == ItemReferences.Water ||
                            ingredient.ID == ItemReferences.Wok || ingredient.ID == ItemReferences.MixingBowlEmpty ||
                            ingredient.ID == ItemReferences.Pot)
                        {
                            // Containers and water don't count. Baking tins will, but will also be a unique Mystery Appliance.
                            continue;
                        }
                        ingredients[iIndex] = ingredient.ID;
                        iIndex++;
                        if (iIndex >= References.MaxIngredientCountForMinimumRecipe)
                        {
                            continue;
                        }
                    }
                    if (iIndex >= References.MaxIngredientCountForMinimumRecipe)
                    {
                        Mod.Logger.LogError($"Static Dish (id = {staticDish.ID}) has more minimum ingredients (count = {iIndex}) than expected. " +
                            $"Expected max ingredient count = {References.MaxIngredientCountForMinimumRecipe}");
                        continue;
                    }
                    type = MysteryMenuType.Static;
                }
                else if (dynamicMenuItem.Type == DynamicMenuType.Fish)
                {
                    ingredients[0] = dynamicMenuItem.Ingredient;
                    if (dynamicMenuItem.Ingredient == ItemReferences.CrabRaw)
                    {
                        ingredients[1] = ItemReferences.Flour;
                        ingredients[2] = ItemReferences.Egg;
                    }
                    type = MysteryMenuType.Fish;
                }
                else if (dynamicMenuItem.Type == References.DynamicMenuTypeMystery)
                {
                    // Most dishes coming here will be GenericMysteryDishCards, which comprise multiple GenericMysteryDishes.
                    GenericMysteryDishCard mysteryCard = MysteryDishCrossReference.GetMysteryCardById(menuItem.SourceDish);
                    GenericMysteryDish mysteryDish = mysteryCard.ContainedMysteryRecipes
                        .Where(cmr => cmr.ResultingMenuItems.Any(cmrrmi => cmrrmi.Item.ID == menuItem.Item))
                        .FirstOrDefault();
                    if (mysteryDish == default)
                    {
                        Mod.Logger.LogWarning($"Mystery Card ({mysteryCard.UniqueNameID}) does not seem to contain CMenuItem with ItemID = {menuItem.Item}");
                        continue;
                    }
                    int iIndex = 0;
                    foreach (Item ingredient in mysteryDish.MinimumRequiredMysteryIngredients)
                    {
                        if (iIndex < References.MaxIngredientCountForMinimumRecipe)
                        {
                            ingredients[iIndex] = ingredient.ID;
                        }
                        iIndex++;
                    }
                    if (iIndex >= References.MaxIngredientCountForMinimumRecipe)
                    {
                        Mod.Logger.LogError($"Mystery Dish ({mysteryDish.UniqueNameID}) has more minimum ingredients (count = {iIndex}) than expected. " +
                            $"Expected max ingredient count = {References.MaxIngredientCountForMinimumRecipe}");
                    }
                    type = MysteryMenuType.Mystery;
                    requiresVariant = mysteryDish.RequiresVariant;
                    // We'll handle provider and menu item stuff in SelectMysteryMenuOfDay, and don't want CDisabledMenuItem being
                    //  placed on Mystery Menu Items.
                    EntityManager.RemoveChunkComponent<CDynamicMenuItem>(entity);
                }
                else
                {
                    // It's a different custom dish with a dynamic type, so ignore it.
                    Mod.Logger.LogInfo($"Menu Item (source dish ID = {menuItem.SourceDish}) is an unknown/custom Dynamic type.");
                    continue;
                }

                CMysteryMenuItem cMysteryMenuItem = new()
                {
                    Ingredients = ingredients,
                    Type = type,
                    HasBeenProvided = false,
                    RequiresVariant = requiresVariant
                };
                EntityManager.AddComponentData(entity, cMysteryMenuItem);
            }
        }

        private void HandleNewMenuOptions()
        {
            using var menuOptionEntities = NonHandledMenuOptions.ToEntityArray(Allocator.Temp);
            using var availableIngredients = NonHandledMenuOptions.ToComponentDataArray<CAvailableIngredient>(Allocator.Temp);
            for (int i = 0; i < menuOptionEntities.Length; i++)
            {
                var entity = menuOptionEntities[i];
                var availableIngredient = availableIngredients[i];

                var genericMysteryDish = MysteryDishCrossReference.GetMysteryDishByMenuItem(availableIngredient.MenuItem);
                if (genericMysteryDish == default)
                {
                    // This is not a mystery dish, so give the entity the appropriate component to prevent it from showing up here again
                    EntityManager.AddComponent<CNonMysteryAvailableIngredient>(entity);
                    continue;
                }

                // This *is* a mystery dish option, so add the identifier component and the CMysteryMenuItem component so that we
                //  know what ingredients will trigger this to be available.
                NativeArray<int> ingredients = new NativeArray<int>(References.MaxIngredientCountForMinimumRecipe, Allocator.Temp);
                int iIndex = 0;
                foreach (Item ingredient in genericMysteryDish.MinimumRequiredMysteryIngredients)
                {
                    if (iIndex < References.MaxIngredientCountForMinimumRecipe)
                    {
                        ingredients[iIndex] = ingredient.ID;
                    }
                    iIndex++;
                }
                if (iIndex >= References.MaxIngredientCountForMinimumRecipe)
                {
                    Mod.Logger.LogError($"Mystery Dish ({genericMysteryDish.UniqueNameID}) has more minimum ingredients (count = {iIndex}) than expected. " +
                        $"Expected max ingredient count = {References.MaxIngredientCountForMinimumRecipe}");
                }

                CMysteryMenuItem mysteryOptionRecipe = new CMysteryMenuItem()
                {
                    Type = MysteryMenuType.Mystery,
                    Ingredients = ingredients,
                    HasBeenProvided = false,
                    RequiresVariant = genericMysteryDish.RequiresVariant
                };
                EntityManager.AddComponentData(entity, mysteryOptionRecipe);
                EntityManager.AddComponent<CMysteryMenuItemOption>(entity);
            }
        }

        private void HandleNewExtras()
        {

        }
    }
}
