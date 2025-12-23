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
    [HarmonyPatch(typeof(GroupHandleChoosingOrder))]
    public class GroupHandleChoosingOrder_Patch
    {
        private static object[] StarterPhaseParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItem),typeof(CMenuItemStarter))
                .None(typeof(CDisabledMysteryMenu)).Build() }
        };
        private static object[] MainPhaseParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItem),typeof(CMenuItemMain))
                .None(typeof(CDisabledMysteryMenu)).Build() }
        };
        private static object[] DessertsPhaseParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItem),typeof(CMenuItemDessert))
                .None(typeof(CDisabledMysteryMenu)).Build() }
        };

        [HarmonyPostfix]
        [HarmonyPatch("Initialise")]
        public static void Initialise_Postfix(ref GroupHandleChoosingOrder __instance)
        {
            // Add CDisabled components to ensure only truly available mystery dishes count towards determining
            //  if the Desserts phase happens.
            Type t_CSB = typeof(ComponentSystemBase);
            MethodInfo m_GetEntityQuery = t_CSB.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(mi => mi.Name.Equals("GetEntityQuery") && mi.GetParameters().Any(p => p.ParameterType == typeof(EntityQueryDesc[])))
                .FirstOrDefault();
            var methods = t_CSB.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(mi => mi.Name.Equals("GetEntityQuery")).ToList();
            Mod.Logger.LogInfo($"Method match count: {methods.Count}");
            foreach ( MethodInfo mi in methods ) { Mod.Logger.LogInfo($"t_CSB method: \"{mi}\""); }

            try
            {
                var StartersQuery = (EntityQuery) m_GetEntityQuery.Invoke(__instance, StarterPhaseParameters);

                var MainsQuery = (EntityQuery) m_GetEntityQuery.Invoke(__instance, MainPhaseParameters);

                var DessertsQuery = (EntityQuery) m_GetEntityQuery.Invoke(__instance, DessertsPhaseParameters);

                var MenuItems = new Dictionary<MenuPhase, EntityQuery>()
                {
                    { MenuPhase.Starter, StartersQuery },
                    { MenuPhase.Main, MainsQuery },
                    { MenuPhase.Dessert, DessertsQuery }
                };
                ReflectionUtils.GetField<GroupHandleChoosingOrder>("MenuItems", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(__instance, MenuItems);


            } catch (Exception e)
            {
                Mod.Logger.LogError("GroupHandleChoosingOrder_Initialise_Postfix failed");
                Mod.Logger.LogError($"m_GetEntityQuery = {m_GetEntityQuery}");
                Mod.Logger.LogException(e);
                throw e;
            }
        }
    }
}
