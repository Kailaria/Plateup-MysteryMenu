using HarmonyLib;
using Kitchen;
using KitchenLib.Utils;
using KitchenMasteryMenu.Components;
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
    [HarmonyPatch(typeof(GroupHandleReadyToOrder))]
    public class GroupHandleReadyToOrder_Patch
    {
        private static object[] SidesParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItemSide))
                .None(typeof(CDisabledMasteryMenu)).Build() }
        };
        private static object[] StartersParameters = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMenuItemStarter))
                .None(typeof(CDisabledMasteryMenu)).Build() }
        };

        [HarmonyPostfix]
        [HarmonyPatch("Initialise")]
        public static void Initialise_Postfix(ref GroupHandleReadyToOrder __instance)
        {
            // Add CDisabled components to ensure only truly available, non-disabled Mastery dishes are accounted for
            //  when determining if a customer will order a side or starter.
            Type t_CSB = typeof(ComponentSystemBase);
            MethodInfo m_GetEntityQuery = t_CSB.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(mi => mi.Name.Equals("GetEntityQuery") && mi.GetParameters().Any(p => p.ParameterType == typeof(EntityQueryDesc[])))
                .FirstOrDefault();
            var methods = t_CSB.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(mi => mi.Name.Equals("GetEntityQuery")).ToList();
            Mod.Logger.LogInfo($"Method match count: {methods.Count}");
            foreach ( MethodInfo mi in methods ) { Mod.Logger.LogInfo($"t_CSB method: \"{mi}\""); }

            try
            {
                var SidesQuery = m_GetEntityQuery.Invoke(__instance, SidesParameters);
                ReflectionUtils.GetField<GroupHandleReadyToOrder>("Sides", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(__instance, SidesQuery);

                var StartersQuery = m_GetEntityQuery.Invoke(__instance, StartersParameters);
                ReflectionUtils.GetField<GroupHandleReadyToOrder>("Starters", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(__instance, StartersQuery);
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
