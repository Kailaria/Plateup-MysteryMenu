using Kitchen;
using KitchenData;
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
    public class HandleNewMysteryMenuDish : RestaurantSystem
    {
        private EntityQuery MysteryProviders;
        private EntityQuery NonHandledMenuItems;

        protected override void Initialise()
        {
            base.Initialise();
            MysteryProviders = GetEntityQuery(typeof(CItemProvider), typeof(CMysteryMenuProvider));
            NonHandledMenuItems = GetEntityQuery(new QueryHelper()
                .All(typeof(CMenuItem), typeof(CDynamicMenuItem))
                .None(typeof(CMysteryMenuItem)));
            RequireForUpdate(MysteryProviders);
            RequireForUpdate(NonHandledMenuItems);
        }

        protected override void OnUpdate()
        {
            Mod.Logger.LogInfo("HandleNewMysteryMenuDish updating");
            // TODO: Refer to HandleNewDish system
            // Check for DynamicMenuType = References.DynamicMenuTypeMystery
            // Add CMysteryMenuItem to whatever has CDynamicMenuItem and is actually of type Mystery
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
                    type = MysteryMenuType.Fish;
                }
                else if (dynamicMenuItem.Type == References.DynamicMenuTypeMystery)
                {
                    // Need to gather all Menu Items and their corresponding minimum ingredients
                    GenericMysteryDish mysteryDish = MysteryDishCrossReference.GetMysteryDishById(menuItem.SourceDish);
                    int iIndex = 0;
                    foreach(Item ingredient in mysteryDish.MinimumRequiredMysteryIngredients)
                    {
                        ingredients[iIndex] = ingredient.ID;
                        iIndex++;
                        if (iIndex >= References.MaxIngredientCountForMinimumRecipe)
                        {
                            continue;
                        }
                    }
                    if (iIndex >= References.MaxIngredientCountForMinimumRecipe)
                    {
                        Mod.Logger.LogError($"Mystery Dish ({mysteryDish.UniqueNameID}) has more minimum ingredients (count = {iIndex}) than expected. " +
                            $"Expected max ingredient count = {References.MaxIngredientCountForMinimumRecipe}");
                        continue;
                    }
                    type = MysteryMenuType.Mystery;
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
                    HasBeenProvided = false
                };
                EntityManager.AddComponentData(entity, cMysteryMenuItem);
            }
        }
    }
}
