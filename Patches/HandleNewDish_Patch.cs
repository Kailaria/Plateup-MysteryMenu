using HarmonyLib;
using Kitchen;
using KitchenLib.Utils;
using KitchenMasteryMenu.Components;
using KitchenMasteryMenu.Customs.Dishes;
using KitchenMasteryMenu.Utils;
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
    [HarmonyPatch(typeof(HandleNewDish))]
    public class HandleNewDish_Patch
    {
        private static object[] MasteryProvidersParams = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMasteryMenuProvider)).Build() }
        };
        private static EntityQuery MasteryProvidersQuery = default;

        [HarmonyPrefix]
        [HarmonyPatch("OnUpdate")]
        public static bool OnUpdate_Prefix(ref HandleNewDish __instance)
        {
            Mod.Logger.LogInfo("[HandleNewDish_Patch] - OnUpdate_Prefix entered");
            try
            {
                if (MasteryProvidersQuery == default)
                {
                    Mod.Logger.LogInfo("[HandleNewDish_Patch] Initializing MasteryProvidersQuery");
                    Type t_CSB = typeof(ComponentSystemBase);
                    MethodInfo m_GetEntityQuery = t_CSB.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(mi => mi.Name.Equals("GetEntityQuery") && mi.GetParameters().Any(p => p.ParameterType == typeof(EntityQueryDesc[])))
                        .FirstOrDefault();
                    MasteryProvidersQuery = (EntityQuery)m_GetEntityQuery.Invoke(__instance, MasteryProvidersParams);
                }

                EntityQuery NewPendingDishes = (EntityQuery)ReflectionUtils.GetField<HandleNewDish>("NewPendingDishes", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(__instance);
                using NativeArray<CNewDish> newDishes = NewPendingDishes.ToComponentDataArray<CNewDish>(Allocator.Temp);

                // The original method does this, so might as well handle it as well to be safe.
                if (newDishes.Length <= 0)
                {
                    Mod.Logger.LogInfo($"[HandleNewDish_Patch] No new dishes found.");
                    return true;
                }

                // Just make new entities indiscriminately since the original method will always run and will destroy the first CNewDish each update.
                CNewDish newDish = newDishes[0];
                Mod.Logger.LogInfo($"[HandleNewDish_Patch] Handling new dish {{ID = {newDish.ID}}}");

                var MasteryDishCard = MasteryDishCrossReference.GetMasteryCardById(newDish.ID);
                using var MasteryProviderEntities = MasteryProvidersQuery.ToEntityArray(Allocator.Temp);
                if (MasteryDishCard == default)
                {
                    // Require there to be Mastery Providers to continue adding a NewMasteryDishEntity for a non-Mastery dish.
                    if (MasteryProviderEntities.Length <= 0)
                    {
                        Mod.Logger.LogInfo($"[HandleNewDish_Patch] No Mastery providers found, so no need to handle the dish.");
                        return true;
                    }
                    // This is either a one-off Mastery Dish or it's a non-Mastery-related Dish, so just create a single new entity.
                    CreateNewMasteryDishEntity(__instance, newDish.ID);
                    return true;
                }

                // This is a GenericMasteryDishCard, so it likely has multiple new Dishes and/or IngredientsUnlocks that each need to be handled.
                HandleMasteryDishCard(__instance, MasteryDishCard);
            }
            catch (Exception e)
            {
                Mod.Logger.LogException(e);
            }
            Mod.Logger.LogInfo("[HandleNewDish_Patch] Exiting OnUpdate_Prefix end.");
            return true;
        }

        private static void HandleMasteryDishCard(HandleNewDish __instance, GenericMasteryDishCard MasteryDishCard)
        {
            // Create new CNewMasteryDish entities for each contained recipe
            Mod.Logger.LogInfo($"[HandleNewDish_Patch] Creating {{{MasteryDishCard.ContainedMasteryRecipes.Count}}} CNewMasteryRecipes for card {{{MasteryDishCard.UniqueNameID}}}");
            foreach (GenericMasteryDish genericMasteryDish in MasteryDishCard.ContainedMasteryRecipes)
            {
                CreateNewMasteryDishEntity(__instance, genericMasteryDish.GameDataObject.ID, MasteryDishCard.GameDataObject.ID);
            }
        }

        private static void CreateNewMasteryDishEntity(HandleNewDish __instance, int newDishID, int newCardID = -1)
        {
            var entity = __instance.EntityManager.CreateEntity(typeof(CNewMasteryRecipe));
            __instance.EntityManager.AddComponentData(entity, new CNewMasteryRecipe
            {
                DishID = newDishID,
                CardID = newCardID
            });
        }

        // [2024-02-03] Don't need to patch Initialise since we're making new entities, not adding component data to existing ones that will just be
        //          deleted in OnUpdate.
        // Need to patch over HandleNewDish to add a "new Mastery Dish" component to hand off to HandleNewMasteryDish
        //private static object[] NewDishParameters = new object[]
        //{
        //    new object[] { 
        //        new QueryHelper().All(typeof(CNewDish)).None(typeof(CNewMasteryDish)).Build() 
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
