using HarmonyLib;
using Kitchen;
using KitchenLib.Utils;
using KitchenMysteryMenu.Components;
using KitchenMysteryMenu.Customs.Dishes;
using KitchenMysteryMenu.Utils;
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
        private static object[] MysteryProvidersParams = new object[]
        {
            new [] { new QueryHelper().All(typeof(CMysteryMenuProvider)).Build() }
        };
        private static EntityQuery MysteryProvidersQuery = default;

        [HarmonyPrefix]
        [HarmonyPatch("OnUpdate")]
        public static bool OnUpdate_Prefix(ref HandleNewDish __instance)
        {
            Mod.Logger.LogInfo("[HandleNewDish_Patch] - OnUpdate_Prefix entered");
            try
            {
                if (MysteryProvidersQuery == default)
                {
                    Mod.Logger.LogInfo("[HandleNewDish_Patch] Initializing MysteryProvidersQuery");
                    Type t_CSB = typeof(ComponentSystemBase);
                    MethodInfo m_GetEntityQuery = t_CSB.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(mi => mi.Name.Equals("GetEntityQuery") && mi.GetParameters().Any(p => p.ParameterType == typeof(EntityQueryDesc[])))
                        .FirstOrDefault();
                    MysteryProvidersQuery = (EntityQuery)m_GetEntityQuery.Invoke(__instance, MysteryProvidersParams);
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

                var mysteryDishCard = MysteryDishCrossReference.GetMysteryCardById(newDish.ID);
                using var mysteryProviderEntities = MysteryProvidersQuery.ToEntityArray(Allocator.Temp);
                if (mysteryDishCard == default)
                {
                    // Require there to be Mystery Providers to continue adding a NewMysteryDishEntity for a non-mystery dish.
                    if (mysteryProviderEntities.Length <= 0)
                    {
                        Mod.Logger.LogInfo($"[HandleNewDish_Patch] No mystery providers found, so no need to handle the dish.");
                        return true;
                    }
                    // This is either a one-off Mystery Dish or it's a non-Mystery-related Dish, so just create a single new entity.
                    CreateNewMysteryDishEntity(__instance, newDish.ID);
                    return true;
                }

                // This is a GenericMysteryDishCard, so it likely has multiple new Dishes and/or IngredientsUnlocks that each need to be handled.
                HandleMysteryDishCard(__instance, mysteryDishCard);
            }
            catch (Exception e)
            {
                Mod.Logger.LogException(e);
            }
            Mod.Logger.LogInfo("[HandleNewDish_Patch] Exiting OnUpdate_Prefix end.");
            return true;
        }

        private static void HandleMysteryDishCard(HandleNewDish __instance, GenericMysteryDishCard mysteryDishCard)
        {
            // Create new CNewMysteryDish entities for each contained recipe
            Mod.Logger.LogInfo($"[HandleNewDish_Patch] Creating {{{mysteryDishCard.ContainedMysteryRecipes.Count}}} CNewMysteryRecipes for card {{{mysteryDishCard.UniqueNameID}}}");
            foreach (GenericMysteryDish genericMysteryDish in mysteryDishCard.ContainedMysteryRecipes)
            {
                CreateNewMysteryDishEntity(__instance, genericMysteryDish.GameDataObject.ID, mysteryDishCard.GameDataObject.ID);
            }
        }

        private static void CreateNewMysteryDishEntity(HandleNewDish __instance, int newDishID, int newCardID = -1)
        {
            var entity = __instance.EntityManager.CreateEntity(typeof(CNewMysteryRecipe));
            __instance.EntityManager.AddComponentData(entity, new CNewMysteryRecipe
            {
                DishID = newDishID,
                CardID = newCardID
            });
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
