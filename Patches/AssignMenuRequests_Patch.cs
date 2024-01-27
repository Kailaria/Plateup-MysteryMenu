using HarmonyLib;
using Kitchen;
using KitchenLib.Utils;
using KitchenMysteryMenu.Components;
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
            new QueryHelper().All(typeof(CMenuItem))
                .None(typeof(CDisabledMenuItem),typeof(CDisabledMysteryMenu))
        };
        private static object[] IngredientsParameters = new object[]
        {
            new QueryHelper().All(typeof(CAvailableIngredient))
                .None(typeof(CDisabledMysteryOption))
        };
        private static object[] ExtrasParameters = new object[]
        {
            new QueryHelper().All(typeof(CPossibleExtra))
                .None(typeof(CDisabledMysteryExtra))
        };

        [HarmonyPostfix]
        [HarmonyPatch("Initialize")]
        public static void Initialize_Postfix(ref AssignMenuRequests __instance)
        {
            // Add CDisabled components to ensure only truly available mystery .
            MethodInfo getEntityQueryMethod = ReflectionUtils.GetMethod<ComponentSystemBase>("GetEntityQuery");
            
            var MenuItemsQuery = getEntityQueryMethod.Invoke(__instance, MenuItemsParameters);
            ReflectionUtils.GetField<AssignMenuRequests>("MenuItems", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(__instance, MenuItemsQuery);

            var IngredientsQuery = getEntityQueryMethod.Invoke(__instance, IngredientsParameters);
            ReflectionUtils.GetField<AssignMenuRequests>("Ingredients", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(__instance, IngredientsQuery);

            var ExtrasQuery = getEntityQueryMethod.Invoke(__instance, ExtrasParameters);
            ReflectionUtils.GetField<AssignMenuRequests>("Extras", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(__instance, ExtrasQuery);
        }
    }
}
