using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Logging;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Components;
using KitchenMasteryMenu.Customs.Dishes;
using KitchenMasteryMenu.Customs.Ingredients;
using KitchenMasteryMenu.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMasteryMenu.Patches
{
    [HarmonyPatch(typeof(RebuildKitchen))]
    public class RebuildKitchen_Patch
    {
        static readonly int MASTERY_MENU_ID = GDOUtils.GetCastedGDO<Dish, MasteryMenuBaseMainsDish>().ID;
        static readonly string LogMsgPrefix = "RebuildKitchen_Patch";
        private static object[] MasteryProvidersParams = new object[]
        {
            new [] { new QueryHelper()
                    .All(typeof(RebuildKitchen.CFranchiseKitchenAppliance),
                        typeof(CItemProvider)/*,
                        typeof(CMasteryMenuProvider)*/).Build() }
        };
        static EntityQuery MasteryProvidersQuery = default;

        [HarmonyPostfix]
        [HarmonyPatch("RecreateAppliances")]
        public static void RecreateAppliances_Postfix(ref RebuildKitchen __instance, Dish dish)
        {
            Mod.Logger.LogInfo($"{LogMsgPrefix} dish.ID = {dish.ID}; dish.Name = {dish.Name}; MasteryMenuID = {MASTERY_MENU_ID}");
            if (dish.ID != MASTERY_MENU_ID)
            {
                return;
            }

            int[] itemIDs = new int[2];
            itemIDs.AddItem(ItemReferences.Meat);
            itemIDs.AddItem(ItemReferences.Flour);

            if (MasteryProvidersQuery == default)
            {
                InitMasteryProvidersQuery(ref __instance);
            }
            using var mpEntities = MasteryProvidersQuery.ToEntityArray(Allocator.Temp);
            Mod.Logger.LogInfo($"{LogMsgPrefix} mpEntities: {mpEntities}\n" +
                $"\tmpEntities.Length: {mpEntities.Length}\n" +
                $"\titemIds.Length: {itemIDs.Length}");

            // Compare Minimum Ingredients in the dish to the provided items by the providers. Modify the data
            // of the matching entity to use the original ingredient instead of the Mastery form.
            // TODO?: After MVP release, instead, base it off a SelectMasteryMenu-selected pair of ingredients
            //      that have been implemented, perhaps only if there's a "randomize ingredients" appliance.
            for (int i = 0; i < mpEntities.Length; i++)
            {
                var ent = mpEntities[i];
                var cItemProvider = CItemProvider.InfiniteItemProvider(itemIDs[i % itemIDs.Length]);
                __instance.EntityManager.RemoveComponent<CItemProvider>(ent);
                __instance.EntityManager.AddComponentData(ent, cItemProvider);
                __instance.EntityManager.AddComponentData(ent, new CMasteryMenuProvider()
                {
                    Type = Utils.MasteryMenuType.Mastery
                });
            }
        }

        /* TODO: After providing a lobby button to swap available ingredients, need to make sure the cats order
         *      the right menu. Down-the-line feature. */
        [HarmonyPrefix]
        [HarmonyPatch("RecreateMenu")]
        public static bool RecreateMenu_Prefix(ref RebuildKitchen __instance, Dish dish)
        {
            // Run RecreateMenu normally for all other dishes besides the Mastery Menu dish
            var MasteryDishCard = (MasteryMenuBaseMainsDish)GDOUtils.GetCustomGameDataObject<MasteryMenuBaseMainsDish>();
            if (dish.ID != GDOUtils.GetCastedGDO<Dish, MasteryMenuBaseMainsDish>().ID)
            {
                return true;
            }

            // Largely recreate the menu in the same way aside from one major difference: ensure that the
            //  "available ingredients" are actually the original ingredients, not their Mastery equivalent
            foreach (Dish.MenuItem unlocksMenuItem in dish.UnlocksMenuItems)
            {
                Entity entity = __instance.EntityManager.CreateEntity(typeof(CMenuItem),
                    typeof(RebuildKitchen.CFranchiseKitchenMenuItem),
                    typeof(CAvailableIngredient));
                __instance.EntityManager.AddComponentData(entity, new CMenuItem
                {
                    Item = unlocksMenuItem.Item.ID,
                    Weight = 1f,
                    Phase = unlocksMenuItem.Phase
                });
                switch (unlocksMenuItem.Phase)
                {
                    case MenuPhase.Starter:
                        __instance.EntityManager.AddComponent<CMenuItemStarter>(entity);
                        break;
                    case MenuPhase.Main:
                        __instance.EntityManager.AddComponent<CMenuItemMain>(entity);
                        break;
                    case MenuPhase.Side:
                        __instance.EntityManager.AddComponent<CMenuItemSide>(entity);
                        break;
                    case MenuPhase.Dessert:
                        __instance.EntityManager.AddComponent<CMenuItemDessert>(entity);
                        break;
                }
            }
            foreach (Dish.IngredientUnlock unlocksIngredient in MasteryDishCard.IngredientsUnlocks)
            {
                Entity entity = __instance.EntityManager.CreateEntity(typeof(CAvailableIngredient));
                __instance.EntityManager.AddComponentData(entity, new CAvailableIngredient
                {
                    MenuItem = unlocksIngredient.MenuItem.ID,
                    Ingredient = unlocksIngredient.Ingredient.ID
                });
            }
            return false;
        }

        private static void InitMasteryProvidersQuery(ref RebuildKitchen __instance)
        {
            Mod.Logger.LogInfo("[RebuildKitchen_Patch.RecreateAppliances] - Initializing MasteryProvidersQuery");
            Type t_CSB = typeof(ComponentSystemBase);
            MethodInfo m_getEntityQuery = t_CSB.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(mi => mi.Name.Equals("GetEntityQuery") && mi.GetParameters().Any(p => p.ParameterType == typeof(EntityQueryDesc[])))
                .FirstOrDefault();
            MasteryProvidersQuery = (EntityQuery)m_getEntityQuery.Invoke(__instance, MasteryProvidersParams);
        }
        //Need to patch over RebuildKitchen to remove menu item entities that can't be ordered
        // Might not be necessary depending on if AlsoAddRecipes adding the cards also adds the dishes *in the restaurant*,
        //  unlike with the kitchen
    }
}
