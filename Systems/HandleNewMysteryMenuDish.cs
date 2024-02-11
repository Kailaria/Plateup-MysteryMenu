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
        private EntityQuery NewPendingMysteryDishes;
        private EntityQuery NonHandledMenuItems;
        private EntityQuery NonHandledMenuOptions;
        private EntityQuery NonHandledPossibleExtras;

        protected override void Initialise()
        {
            base.Initialise();
            MysteryProviders = GetEntityQuery(typeof(CItemProvider), typeof(CMysteryMenuProvider));
            NewPendingMysteryDishes = GetEntityQuery(typeof(CNewMysteryRecipe));
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
            RequireForUpdate(MysteryProviders);
            RequireForUpdate(NewPendingMysteryDishes);
        }

        protected override void OnUpdate()
        {
            Mod.Logger.LogInfo("HandleNewMysteryMenuDish updating");
            NativeArray<Entity> newMysteryRecipes = NewPendingMysteryDishes.ToEntityArray(Allocator.Temp);
            if (newMysteryRecipes.Length <= 0)
            {
                return; // This shouldn't happen with RequireForUpdate, but best to be safe.
            }

            CNewMysteryRecipe newMysteryRecipe = GetComponent<CNewMysteryRecipe>(newMysteryRecipes[0]);
            Dish dishData = GameData.Main.Get<Dish>(newMysteryRecipe.DishID);
            GenericMysteryDish genericMysteryDish = MysteryDishCrossReference.GetMysteryDishById(newMysteryRecipe.DishID);
            GenericMysteryDishCard genericMysteryDishCard = MysteryDishCrossReference.GetMysteryCardById(newMysteryRecipe.CardID);
            EntityManager.DestroyEntity(newMysteryRecipes[0]);

            // TODO: Refer to HandleNewDish system
            // Check for DynamicMenuType = References.DynamicMenuTypeMystery
            // Add CMysteryMenuItem to whatever has CDynamicMenuItem and is actually of type Mystery
            if (dishData.UnlocksMenuItems.Count > 0)
            {
                HandleNewMenuItems(dishData, genericMysteryDish, genericMysteryDishCard);
            }
            if (dishData.UnlocksIngredients.Count > 0)
            {
                HandleNewMenuOptions(dishData, genericMysteryDish, genericMysteryDishCard);
            }
            if (dishData.ExtraOrderUnlocks.Count > 0)
            {
                HandleNewExtras(dishData, genericMysteryDish, genericMysteryDishCard);
            }
        }

        private void HandleNewMenuItems(Dish dishData, GenericMysteryDish genericMysteryDish, GenericMysteryDishCard mysteryDishCard)
        {
            using var nonHandledMenuItems = NonHandledMenuItems.ToEntityArray(Allocator.Temp);
            using var cMenuItems = NonHandledMenuItems.ToComponentDataArray<CMenuItem>(Allocator.Temp);
            using var cDynamicMenuItems = NonHandledMenuItems.ToComponentDataArray<CDynamicMenuItem>(Allocator.Temp);
            for (int i = 0; i < nonHandledMenuItems.Length; i++)
            {
                var entity = nonHandledMenuItems[i];
                var menuItem = cMenuItems[i];
                var dynamicMenuItem = cDynamicMenuItems[i];

                if (menuItem.SourceDish != dishData.ID)
                {
                    continue;
                }

                NativeArray<int> ingredients = new NativeArray<int>(References.MaxIngredientCountForMinimumRecipe, Allocator.Temp);
                MysteryMenuType type;
                bool requiresVariant = false;
                // TODO: Instead, check for mystery dish & card defaultness..?
                if (dynamicMenuItem.Type == DynamicMenuType.Static)
                {
                    // Utilize the related Mystery Dish to get the Static Dish and compare
                    Dish staticDish = MysteryDishCrossReference.GetRelatedMysteryMainDish(menuItem.SourceDish).OrigDish;
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
                    // check if the genericMysteryDish matches
                    if (genericMysteryDish == default)
                    {
                        Mod.Logger.LogWarning($"Mystery Card ({mysteryDishCard.UniqueNameID}) does not seem to contain CMenuItem with ItemID = {menuItem.Item}");
                        continue;
                    }
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
                    type = MysteryMenuType.Mystery;
                    requiresVariant = genericMysteryDish.RequiresVariant;
                    
                    // We'll handle provider and menu item stuff in SelectMysteryMenuOfDay, and don't want CDisabledMenuItem being
                    //  placed on Mystery Menu Items.
                    EntityManager.RemoveComponent<CDynamicMenuItem>(entity);

                    // Point the CMenuItem's SourceDish to the MysteryDish's ID instead of its parent MysteryDishCard's ID
                    menuItem.SourceDish = genericMysteryDish.BaseGameDataObjectID;
                    EntityManager.RemoveComponent<CMenuItem>(entity);
                    EntityManager.AddComponentData(entity, menuItem);
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

        private void HandleNewMenuOptions(Dish dishData, GenericMysteryDish genericMysteryDish, GenericMysteryDishCard genericMysteryDishCard)
        {
            using var menuOptionEntities = NonHandledMenuOptions.ToEntityArray(Allocator.Temp);
            using var availableIngredients = NonHandledMenuOptions.ToComponentDataArray<CAvailableIngredient>(Allocator.Temp);
            for (int i = 0; i < menuOptionEntities.Length; i++)
            {
                var entity = menuOptionEntities[i];
                var availableIngredient = availableIngredients[i];

                var mysteryDish = MysteryDishCrossReference.GetMysteryDishByMenuItem(availableIngredient.MenuItem);
                if (mysteryDish == default)
                {
                    // This is not a mystery dish, so give the entity the appropriate component to prevent it from showing up here again
                    EntityManager.AddComponent<CNonMysteryAvailableIngredient>(entity);
                    continue;
                }

                // This *is* a mystery dish option, so add the identifier component and the CMysteryMenuItem component so that we
                //  know what ingredients will trigger this to be available.
                NativeArray<int> ingredients = new NativeArray<int>(References.MaxIngredientCountForMinimumRecipe, Allocator.Temp);
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

                CMysteryMenuItem mysteryOptionRecipe = new CMysteryMenuItem()
                {
                    Type = MysteryMenuType.Mystery,
                    Ingredients = ingredients,
                    HasBeenProvided = false,
                    RequiresVariant = mysteryDish.RequiresVariant
                };
                EntityManager.AddComponentData(entity, mysteryOptionRecipe);
                EntityManager.AddComponent<CMysteryMenuItemOption>(entity);
            }
        }

        private void HandleNewExtras(Dish dishData, GenericMysteryDish genericMysteryDish, GenericMysteryDishCard genericMysteryDishCard)
        {

        }
    }
}
