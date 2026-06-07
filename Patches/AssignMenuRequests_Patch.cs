using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using KitchenMasteryMenu.Components;
using KitchenMasteryMenu.Utils;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenMasteryMenu.Patches
{
    [HarmonyPatch(typeof(AssignMenuRequests))]
    public class AssignMenuRequests_Patch
    {
        private static object[] MenuItemsParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItem))
                .None(typeof(CDisabledMenuItem),typeof(CDisabledMasteryMenu)).Build() }
        };
        private static object[] IngredientsParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CAvailableIngredient))
                .None(typeof(CDisabledMasteryMenu)).Build() }
        };
        private static object[] ExtrasParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CPossibleExtra))
                .None(typeof(CDisabledMasteryMenu)).Build() }
        };

        [HarmonyPostfix]
        [HarmonyPatch("Initialise")]
        public static void Initialise_Postfix(ref AssignMenuRequests __instance)
        {
            // Add CDisabled components to ensure only truly available Mastery dishes will be ordered.
            Type t_CSB = typeof(ComponentSystemBase);
            MethodInfo m_GetEntityQuery = t_CSB.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(mi => mi.Name.Equals("GetEntityQuery") && mi.GetParameters().Any(p => p.ParameterType == typeof(EntityQueryDesc[])))
                .FirstOrDefault();
            var methods = t_CSB.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(mi => mi.Name.Equals("GetEntityQuery")).ToList();
            Mod.Logger.LogInfo($"Method match count: {methods.Count}");
            foreach ( MethodInfo mi in methods ) { Mod.Logger.LogInfo($"t_CSB method: \"{mi}\""); }

            try
            {
                var MenuItemsQuery = m_GetEntityQuery.Invoke(__instance, MenuItemsParameters);
                ReflectionUtils.GetField<AssignMenuRequests>("MenuItems", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(__instance, MenuItemsQuery);

                var IngredientsQuery = m_GetEntityQuery.Invoke(__instance, IngredientsParameters);
                ReflectionUtils.GetField<AssignMenuRequests>("Ingredients", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(__instance, IngredientsQuery);

                var ExtrasQuery = m_GetEntityQuery.Invoke(__instance, ExtrasParameters);
                ReflectionUtils.GetField<AssignMenuRequests>("Extras", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(__instance, ExtrasQuery);
            } catch (Exception e)
            {
                Mod.Logger.LogError("AssignMenuRequests_Initialise_Postfix failed");
                Mod.Logger.LogError($"m_GetEntityQuery = {m_GetEntityQuery}");
                Mod.Logger.LogException(e);
                throw e;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("OrderItem")]
        public static void OrderItem_Postfix(AssignMenuRequests __instance, Item item_data, EntityContext ctx, ItemList item_components, Entity group, ref float bonus_time, int member_index, MenuPhase phase, int source_menu_item)
        {
            var MasteryDish = MasteryDishCrossReference.GetMasteryDishById(source_menu_item);
            Dish normalDish = null;
            if (MasteryDish == default)
            {
                normalDish = (Dish)GDOUtils.GetExistingGDO(source_menu_item);
            }
            string sourceMenuItemName = "";
            try
            {
                sourceMenuItemName = normalDish != default ? normalDish.name : MasteryDish.UniqueNameID;
            }
            catch (NullReferenceException e)
            {
                sourceMenuItemName = "ERROR: NAME FAILED";
                Mod.Logger.LogError(e.StackTrace);
            }
            Mod.Logger.LogInfo($"AssignMenuRequests - OrderItem Postfix\n" +
                $"|\titem_data {{id: {item_data.ID}, name: {item_data.name}}}\n" +
                $"|\titem_components {{names: {String.Join(", ", item_components.AsArray().Select(i => ((Item)GDOUtils.GetExistingGDO(i)).name))}}}\n" +
                $"|\tsource_menu_item {{id: {source_menu_item}, name?: {sourceMenuItemName}}}");
        }
    }
}
