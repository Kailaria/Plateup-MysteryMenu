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
        private static object[] MenuItemsStarterParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItem), typeof(CMenuItemStarter))
                .None(typeof(CDisabledMenuItem),typeof(CDisabledMysteryMenu)).Build() }
        };
        private static object[] MenuItemsMainParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItem), typeof(CMenuItemMain))
                .None(typeof(CDisabledMenuItem),typeof(CDisabledMysteryMenu)).Build() }
        };
        private static object[] MenuItemsDessertParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItem), typeof(CMenuItemDessert))
                .None(typeof(CDisabledMenuItem),typeof(CDisabledMysteryMenu)).Build() }
        };

        [HarmonyPostfix]
        [HarmonyPatch("Initialise")]
        public static void Initialise_Postfix(ref GroupHandleChoosingOrder __instance)
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
                var MenuItems = new Dictionary<MenuPhase, EntityQuery>();
                var MenuItemsStarterQuery = (EntityQuery) m_GetEntityQuery.Invoke(__instance, MenuItemsStarterParameters);
                var MenuItemsMainQuery = (EntityQuery) m_GetEntityQuery.Invoke(__instance, MenuItemsMainParameters);
                var MenuItemsDessertQuery = (EntityQuery) m_GetEntityQuery.Invoke(__instance, MenuItemsDessertParameters);
                MenuItems.Add(MenuPhase.Starter, MenuItemsStarterQuery);
                MenuItems.Add(MenuPhase.Main, MenuItemsMainQuery);
                MenuItems.Add(MenuPhase.Dessert, MenuItemsDessertQuery);
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
