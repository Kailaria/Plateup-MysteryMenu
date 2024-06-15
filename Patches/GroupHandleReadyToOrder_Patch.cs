using HarmonyLib;
using Kitchen;
using KitchenData;
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
    [HarmonyPatch(typeof(GroupHandleReadyToOrder))]
    public class GroupHandleReadyToOrder_Patch
    {
        private static object[] MenuItemsStarterParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItemStarter))
                .None(typeof(CDisabledMenuItem),typeof(CDisabledMysteryMenu)).Build() }
        };
        private static object[] MenuItemsSideParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItemSide))
                .None(typeof(CDisabledMenuItem),typeof(CDisabledMysteryMenu)).Build() }
        };

        [HarmonyPostfix]
        [HarmonyPatch("Initialise")]
        public static void Initialise_Postfix(ref GroupHandleReadyToOrder __instance)
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
                var MenuItemsStarterQuery = (EntityQuery) m_GetEntityQuery.Invoke(__instance, MenuItemsStarterParameters);
                ReflectionUtils.GetField<GroupHandleReadyToOrder>("Starters", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(__instance, MenuItemsStarterQuery);

                var MenuItemsSideQuery = (EntityQuery) m_GetEntityQuery.Invoke(__instance, MenuItemsSideParameters);
                ReflectionUtils.GetField<GroupHandleReadyToOrder>("Sides", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(__instance, MenuItemsSideQuery);
            } catch (Exception e)
            {
                Mod.Logger.LogError("GroupHandleReadyToOrder_Initialise_Postfix failed");
                Mod.Logger.LogError($"m_GetEntityQuery = {m_GetEntityQuery}");
                Mod.Logger.LogException(e);
                throw e;
            }
        }
    }
}
