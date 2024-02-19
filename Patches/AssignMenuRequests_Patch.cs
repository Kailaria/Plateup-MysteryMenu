using HarmonyLib;
using Kitchen;
using KitchenLib.Utils;
using KitchenMysteryMenu.Components;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenMysteryMenu.Patches
{
    [HarmonyPatch(typeof(AssignMenuRequests))]
    public class AssignMenuRequests_Patch
    {
        private static object[] MenuItemsParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItem))
                .None(typeof(CDisabledMenuItem),typeof(CDisabledMysteryMenu)).Build() }
        };
        private static object[] IngredientsParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CAvailableIngredient))
                .None(typeof(CDisabledMysteryOption)).Build() }
        };
        private static object[] ExtrasParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CPossibleExtra))
                .None(typeof(CDisabledMysteryExtra)).Build() }
        };

        [HarmonyPostfix]
        [HarmonyPatch("Initialise")]
        public static void Initialise_Postfix(ref AssignMenuRequests __instance)
        {
            // Add CDisabled components to ensure only truly available mystery .
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
    }
}
