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
using Unity.Collections;
using Unity.Entities;

namespace KitchenMysteryMenu.Patches
{
    [HarmonyPatch(typeof(HandleNewDish))]
    public class HandleNewDish_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnUpdate")]
        public static bool OnUpdate_Prefix(ref HandleNewDish __instance)
        {
            try
            {
                EntityQuery NewPendingDishes = (EntityQuery) ReflectionUtils.GetField<HandleNewDish>("NewPendingDishes", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(__instance);
                using NativeArray<CNewDish> newDishes = NewPendingDishes.ToComponentDataArray<CNewDish>(Allocator.Temp);

                // The original method does this, so might as well handle it as well to be safe.
                if (newDishes.Length <= 0)
                {
                    return true;
                }

                // Just make new entities indiscriminately since the original method will always run and will destroy the first CNewDish each update.
                CNewDish newDish = newDishes[0];
                var entity = __instance.EntityManager.CreateEntity(typeof(CNewMysteryDish));
                __instance.EntityManager.AddComponentData(entity, new CNewMysteryDish
                {
                    ID = newDish.ID,
                    ShowRecipe = newDish.ShowRecipe
                });
            }
            catch (Exception e)
            {
                Mod.Logger.LogException(e);
            }
            return true;
        }

        // [2024-02-03] Don't need to patch Initialise since we're making new entities, not adding component data to existing ones that will just be
        //          deleted in OnUpdate.
        // Need to patch over HandleNewDish to add a "new Mystery Dish" component to hand off to HandleNewMysteryDish
        //private static object[] NewDishParameters = new object[]
        //{
        //    new object[] { 
        //        new QueryHelper().All(typeof(CNewDish)).None(typeof(CNewMysteryDish)).Build() 
        //    }
        //};

        //[HarmonyPostfix]
        //[HarmonyPatch("Initialise")]
        //public static void Initialise_Postfix(ref HandleNewDish __instance)
        //{
        //    MethodInfo m_GetEntityQuery = typeof(ComponentSystemBase).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
        //        .Where(mi => mi.Name.Equals("GetEntityQuery") && mi.GetParameters().Any(p => p.ParameterType == typeof(EntityQueryDesc[])))
        //        .FirstOrDefault();

        //    try
        //    {
        //        var NewPendingDishQuery = m_GetEntityQuery.Invoke(__instance, NewDishParameters);
        //        ReflectionUtils.GetField<HandleNewDish>("NewPendingDishes", BindingFlags.NonPublic | BindingFlags.Instance)
        //            .SetValue(__instance, NewPendingDishQuery);
        //    } catch (Exception e)
        //    {
        //        Mod.Logger.LogError("HandleNewDish_Initialise_Postfix failed");
        //        Mod.Logger.LogException(e);
        //        throw e;
        //    }
        //}
    }
}
