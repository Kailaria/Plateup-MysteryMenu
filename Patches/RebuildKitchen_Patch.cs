using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib.Logging;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Components;
using KitchenMysteryMenu.Customs.Dishes;
using KitchenMysteryMenu.Customs.Ingredients;
using KitchenMysteryMenu.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMysteryMenu.Patches
{
    [HarmonyPatch(typeof(RebuildKitchen))]
    public class RebuildKitchen_Patch
    {
        static readonly int MYSTERY_MENU_ID = GDOUtils.GetCastedGDO<Dish, MysteryMenuDish>().ID;
        static KitchenLogger Logger = new KitchenLogger("KitchenMysteryMenu");
        static EntityQuery MysteryProvidersQuery = PatchController.StaticGetEntityQuery(
                    new QueryHelper()
                    .All(typeof(RebuildKitchen.CFranchiseKitchenAppliance),
                        typeof(CItemProvider),
                        typeof(CMysteryMenuProvider))
                );

        [HarmonyPostfix]
        [HarmonyPatch("RecreateAppliances")]
        public static void RecreateAppliances_Postfix(Dish dish)
        {
            Logger.LogInfo($"dish.ID = {dish.ID}; dish.Name = {dish.Name}; MysteryMenuID = {MYSTERY_MENU_ID}");
            if (dish.ID != MYSTERY_MENU_ID)
            {
                return;
            }

            NativeArray<int> itemIDs = new();
            itemIDs.AddItem(ItemReferences.Meat);
            itemIDs.AddItem(ItemReferences.Flour);
            using var mpEntities = MysteryProvidersQuery.ToEntityArray(Allocator.Temp);
            Logger.LogInfo($"mpEntities: {mpEntities}\n" +
                $"mpEntities.Length: {mpEntities.Length}");

            // Compare Minimum Ingredients in the dish to the provided items by the providers. Modify the data
            // of the matching entity to use the original ingredient instead of the Mystery form.
            // TODO?: After MVP release, instead, base it off a SelectMysteryMenu-selected pair of ingredients
            //      that have been implemented, perhaps only if there's a "randomize ingredients" appliance.
            for (int i = 0; i < mpEntities.Length; i++)
            {
                var ent = mpEntities[i];
                var cItemProvider = CItemProvider.InfiniteItemProvider(itemIDs[i % itemIDs.Length]);
                PatchController.StaticRemoveComponentData<CItemProvider>(ent);
                PatchController.StaticAddComponentData(ent, cItemProvider);
            }
        }

        /* TODO: After providing a lobby button to swap available ingredients, need to make sure the cats order
         *      the right menu. Down-the-line feature. */
        //[HarmonyPrefix]
        //[HarmonyPatch("RecreateMenu")]
        //public static bool RecreateMenu_Prefix(Dish dish)
        //{
        //    // Run RecreateMenu normally for all other dishes besides the Mystery Menu dish
        //    var mysteryMenuDish = (MysteryMenuDish) GDOUtils.GetCustomGameDataObject<MysteryMenuDish>();
        //    if (dish.Name != GDOUtils.GetCastedGDO<Dish,MysteryMenuDish>().Name)
        //    {
        //        return true;
        //    }

        //    // Largely recreate the menu in the same way aside from one major difference: ensure that the
        //    //  "available ingredients" are actually the original ingredients, not their Mystery equivalent
        //    foreach (Dish.MenuItem unlocksMenuItem in dish.UnlocksMenuItems)
        //    {
        //        Entity entity = PatchController.StaticCreateEntity(typeof(CMenuItem), 
        //            typeof(RebuildKitchen.CFranchiseKitchenMenuItem),
        //            typeof(CAvailableIngredient));
        //        PatchController.StaticAddComponentData(entity, new CMenuItem
        //        {
        //            Item = unlocksMenuItem.Item.ID,
        //            Weight = 1f,
        //            Phase = unlocksMenuItem.Phase
        //        });
        //        switch (unlocksMenuItem.Phase)
        //        {
        //            case MenuPhase.Starter:
        //                PatchController.StaticAddComponent<CMenuItemStarter>(entity);
        //                break;
        //            case MenuPhase.Main:
        //                PatchController.StaticAddComponent<CMenuItemMain>(entity);
        //                break;
        //            case MenuPhase.Side:
        //                PatchController.StaticAddComponent<CMenuItemSide>(entity);
        //                break;
        //            case MenuPhase.Dessert:
        //                PatchController.StaticAddComponent<CMenuItemDessert>(entity);
        //                break;
        //        }
        //        if (!(unlocksMenuItem.Item is ItemGroup itemGroup))
        //        {
        //            continue;
        //        }
        //        foreach (ItemGroup.ItemSet derivedSet in itemGroup.DerivedSets)
        //        {
        //            foreach (Item item in derivedSet.Items)
        //            {
        //                if (derivedSet.RequiresUnlock)
        //                {
        //                    bool unlockIngredientFlag = false;
        //                    foreach (Dish.IngredientUnlock unlocksIngredient in dish.UnlocksIngredients)
        //                    {
        //                        if (unlocksIngredient.MenuItem == itemGroup && unlocksIngredient.Ingredient == item)
        //                        {
        //                            unlockIngredientFlag = true;
        //                            break;
        //                        }

        //                    }
        //                    if (!unlockIngredientFlag)
        //                    {
        //                        continue;
        //                    }
        //                }
        //                PatchController.StaticUnlockIngredient(unlocksMenuItem.Item.ID, item.ID);
        //            }
        //        }
        //    }
        //    return false;
        //}
        // Need to patch over RebuildKitchen to remove menu item entities that can't be ordered
        // Might not be necessary depending on if AlsoAddRecipes adding the cards also adds the dishes *in the restaurant*,
        //  unlike with the kitchen
    }
}
