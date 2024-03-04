using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Components;
using KitchenMysteryMenu.Customs.Dishes;
using KitchenMysteryMenu.Utils;
using MysteryMenu.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMysteryMenu.Systems
{
    public class HandleNewMysteryMenuDish : RestaurantSystem
    {
        private EntityQuery MysteryProviders;
        private EntityQuery NewPendingMysteryDishes;
        private EntityQuery NonHandledMenuItems;
        private EntityQuery NonHandledMenuOptions;
        private EntityQuery NonHandledPossibleExtras;

        protected override void Initialise()
        {
            base.Initialise();
            MysteryProviders = GetEntityQuery(typeof(CItemProvider), typeof(CMysteryMenuProvider));
            NewPendingMysteryDishes = GetEntityQuery(typeof(CNewMysteryRecipe));
            NonHandledMenuItems = GetEntityQuery(new QueryHelper()
                .All(typeof(CMenuItem))
                .None(typeof(CMysteryMenuItem)));
            // Don't care about entities that are both a CMenuItem & CAvailableIngredient as those are handled above
            NonHandledMenuOptions = GetEntityQuery(new QueryHelper()
                .All(typeof(CAvailableIngredient))
                .None(typeof(CMenuItem), typeof(CMysteryMenuItemOption), typeof(CNonMysteryAvailableIngredient)));
            NonHandledPossibleExtras = GetEntityQuery(new QueryHelper()
                .All(typeof(CPossibleExtra))
                .None(typeof(CMysteryMenuItemOption), typeof(CNonMysteryExtra)));
            RequireForUpdate(MysteryProviders);
            RequireForUpdate(NewPendingMysteryDishes);
        }

        protected override void OnUpdate()
        {
            Mod.Logger.LogInfo("HandleNewMysteryMenuDish updating");
            NativeArray<Entity> newMysteryRecipes = NewPendingMysteryDishes.ToEntityArray(Allocator.Temp);
            if (newMysteryRecipes.Length <= 0)
            {
                return; // This shouldn't happen with RequireForUpdate, but best to be safe.
            }

            CNewMysteryRecipe newMysteryRecipe = GetComponent<CNewMysteryRecipe>(newMysteryRecipes[0]);
            Dish dishData = GameData.Main.Get<Dish>(newMysteryRecipe.DishID);
            GenericMysteryDish genericMysteryDish = MysteryDishCrossReference.GetMysteryDishById(newMysteryRecipe.DishID);
            GenericMysteryDishCard genericMysteryDishCard = MysteryDishCrossReference.GetMysteryCardById(newMysteryRecipe.CardID);
            EntityManager.DestroyEntity(newMysteryRecipes[0]);

            // dishData is guaranteed to be a valid Dish-GDO even if the dish is part of this mod.
            //   genericMysteryDish & -DishCard will only be non-default if it's a mystery dish, though.
            Mod.Logger.LogInfo($"HandleNewMysteryMenuDish - Processing Dish {{{dishData.Name}}}");
            if (dishData.UnlocksMenuItems.Count > 0)
            {
                HandleNewMenuItems(dishData, genericMysteryDish, genericMysteryDishCard);
            }
            if (dishData.UnlocksIngredients.Count > 0)
            {
                HandleNewMenuOptions(dishData, genericMysteryDish, genericMysteryDishCard);
            }
            if (dishData.ExtraOrderUnlocks.Count > 0)
            {
                HandleNewExtras(dishData, genericMysteryDish, genericMysteryDishCard);
            }
        }

        private void HandleNewMenuItems(Dish dishData, GenericMysteryDish genericMysteryDish, GenericMysteryDishCard mysteryDishCard)
        {
            Mod.Logger.LogInfo("Handling new menu item");
            using var nonHandledMenuItems = NonHandledMenuItems.ToEntityArray(Allocator.Temp);
            using var cMenuItems = NonHandledMenuItems.ToComponentDataArray<CMenuItem>(Allocator.Temp);
            for (int i = 0; i < nonHandledMenuItems.Length; i++)
            {
                var entity = nonHandledMenuItems[i];
                var menuItem = cMenuItems[i];

                // Continue until we find the matching non-mystery Dish or the matching MysteryDishCard
                Mod.Logger.LogInfo($"[HandleNewMenuItems] cMenuItem.SourceDish {{{menuItem.SourceDish}}}; dishData.ID {{{dishData.ID}}}; " +
                    $"gMD.GDO.ID {{{genericMysteryDish?.GameDataObject.ID}}}; mDC.GDO.ID {{{mysteryDishCard?.GameDataObject.ID}}}");
                int relevantSourceDishID = mysteryDishCard == default ? dishData.ID : mysteryDishCard.GameDataObject.ID;
                if (menuItem.SourceDish != relevantSourceDishID)
                {
                    continue;
                }

                MysteryMenuType type;

                // This should handle both vanilla and modded dishes now since we have a dishData handed in.
                if (genericMysteryDish == default && mysteryDishCard == default)
                {
                    Mod.Logger.LogInfo($"Handling non-mystery menu item");
                    // Utilize the related Mystery Dish to get the Static Dish and compare
                    GenericMysteryDish relatedMysteryDish = MysteryDishCrossReference.GetRelatedMysteryMainDish(dishData.ID);
                    //TODO: try to get a CDynamicMenuItem for the entity and add ingredients for the fish itself..?
                    //TODO: handle case when Static Dish is a "duplicate" MenuItem of an existing GenericMysteryDish's MenuItem

                    type = MysteryMenuType.Static;
                }
                // TODO: Handle Fish in standard Dish-based cards above
                //else if (dynamicMenuItem.Type == DynamicMenuType.Fish)
                //{
                //    ingredients[0] = dynamicMenuItem.Ingredient;
                //    if (dynamicMenuItem.Ingredient == ItemReferences.CrabRaw)
                //    {
                //        ingredients[1] = ItemReferences.Flour;
                //        ingredients[2] = ItemReferences.Egg;
                //    }
                //    type = MysteryMenuType.Fish;
                //}
                else
                {
                    // check if the genericMysteryDish matches
                    if (genericMysteryDish == default)
                    {
                        Mod.Logger.LogWarning($"Mystery Card ({mysteryDishCard.UniqueNameID}) does not seem to contain CMenuItem with ItemID = {menuItem.Item}");
                        continue;
                    }
                    type = MysteryMenuType.Mystery;

                    // TODO: Handle case when Mystery Dish would be a duplicate of an existing Static/Dynamic Dish's CMenuItem (by removing the one with MysteryMenuItem.Type = Mystery).
                    // Point the CMenuItem's SourceDish to the MysteryDish's ID instead of its parent MysteryDishCard's ID
                    menuItem.SourceDish = genericMysteryDish.GameDataObject.ID;
                    EntityManager.AddComponentData(entity, menuItem);
                }

                // Source Dish by now should either be the original dish ID or the GenericMysteryDish's GDO ID, rather than a GMDC's GDO ID
                Mod.Logger.LogInfo($"Creating CMysteryMenuItem for entity {{Index = {entity.Index}}}");
                EntityManager.AddComponentData(entity, new CMysteryMenuItem()
                {
                    SourceMysteryDish = menuItem.SourceDish,
                    Type = type,
                    HasBeenProvided = false
                });
                break;
            }
        }

        private void HandleNewMenuOptions(Dish dishData, GenericMysteryDish genericMysteryDish, GenericMysteryDishCard genericMysteryDishCard)
        {
            string soughtIngredientsLog = "";
            foreach (var ingredient in dishData.UnlocksIngredients)
            {
                soughtIngredientsLog += $"  {{MenuItem = {ingredient.MenuItem.ID}, Ingredient = {ingredient.Ingredient.ID}}}\n";
            }
            Mod.Logger.LogInfo($"Handling new menu option; Looking for available ingredient objects: [\n{soughtIngredientsLog}]");
            using var menuOptionEntities = NonHandledMenuOptions.ToEntityArray(Allocator.Temp);
            using var availableIngredients = NonHandledMenuOptions.ToComponentDataArray<CAvailableIngredient>(Allocator.Temp);
            for (int i = 0; i < menuOptionEntities.Length; i++)
            {
                var entity = menuOptionEntities[i];
                var availableIngredient = availableIngredients[i];

                // Continue until we find the matching CAvailableIngredient. The relevant IngredientUnlock set will be a standard dish if non-mystery
                //  or the MysteryDish's if it is mystery.
                Mod.Logger.LogInfo($"[HandleNewMenuOptions] cAvailableIngredient {{MenuItem = {availableIngredient.MenuItem}, Ingredient = {availableIngredient.Ingredient}}}");
                HashSet<Dish.IngredientUnlock> relevantUISet = genericMysteryDish == default
                    ? dishData.UnlocksIngredients
                    : genericMysteryDish.IngredientsUnlocks;
                if (!relevantUISet.Any(ui => ui.MenuItem.ID == availableIngredient.MenuItem && ui.Ingredient.ID == availableIngredient.Ingredient))
                {
                    continue;
                }

                if (genericMysteryDish == default)
                {
                    // This is not a mystery dish, so give the entity the appropriate component to prevent it from showing up here again
                    // TODO: do I want to actually add the recipe here..? Maybe worry about it when testing Autumn/Lake/Variety
                    Mod.Logger.LogInfo("Option is *not* mystery; Adding CNonMysteryAvailableIngredient");
                    EntityManager.AddComponent<CNonMysteryAvailableIngredient>(entity);
                    break;
                }

                // This *is* a mystery dish option, so add the identifier component and the CMysteryMenuItem component so that we
                //  know what ingredients will trigger this to be available.

                Mod.Logger.LogInfo($"Creating CMysteryMenuItem and CMysteryMenuItemOption for entity {{Index = {entity.Index}}}");
                EntityManager.AddComponentData(entity, new CMysteryMenuItem()
                {
                    Type = MysteryMenuType.Mystery,
                    SourceMysteryDish = genericMysteryDish.GameDataObject.ID,
                    HasBeenProvided = false,
                    RequiresVariant = genericMysteryDish.RequiresVariant
                });
                EntityManager.AddComponent<CMysteryMenuItemOption>(entity);
            }
        }

        private void HandleNewExtras(Dish dishData, GenericMysteryDish genericMysteryDish, GenericMysteryDishCard genericMysteryDishCard)
        {

        }
    }
}
