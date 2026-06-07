using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu.Components;
using KitchenMasteryMenu.Customs.Dishes;
using KitchenMasteryMenu.Utils;
using MasteryMenu.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMasteryMenu.Systems
{
    public class HandleNewMasteryMenuDish : RestaurantSystem
    {
        private EntityQuery MasteryProviders;
        private EntityQuery NewPendingMasteryDishes;
        private EntityQuery NonHandledMenuItems;
        private EntityQuery NonHandledMenuOptions;
        private EntityQuery NonHandledPossibleExtras;
        private EntityQuery HandledSubstitutionSetsQuery;

        protected override void Initialise()
        {
            base.Initialise();
            MasteryProviders = GetEntityQuery(typeof(CItemProvider), typeof(CMasteryMenuProvider));
            NewPendingMasteryDishes = GetEntityQuery(typeof(CNewMasteryRecipe));
            NonHandledMenuItems = GetEntityQuery(new QueryHelper()
                .All(typeof(CMenuItem))
                .None(typeof(CMasteryMenuItem)));
            // Don't care about entities that are both a CMenuItem & CAvailableIngredient as those are handled above
            NonHandledMenuOptions = GetEntityQuery(new QueryHelper()
                .All(typeof(CAvailableIngredient))
                .None(typeof(CMenuItem), typeof(CMasteryMenuItemOption), typeof(CNonMasteryAvailableIngredient)));
            NonHandledPossibleExtras = GetEntityQuery(new QueryHelper()
                .All(typeof(CPossibleExtra))
                .None(typeof(CMasteryMenuItemOption), typeof(CNonMasteryExtra)));
            HandledSubstitutionSetsQuery = GetEntityQuery(typeof(CMasteryMenuSubstitutionSet));
            RequireForUpdate(MasteryProviders);
            RequireForUpdate(NewPendingMasteryDishes);
        }

        protected override void OnUpdate()
        {
            Mod.Logger.LogInfo("HandleNewMasteryMenuDish updating");
            NativeArray<Entity> newMasteryRecipes = NewPendingMasteryDishes.ToEntityArray(Allocator.Temp);
            if (newMasteryRecipes.Length <= 0)
            {
                return; // This shouldn't happen with RequireForUpdate, but best to be safe.
            }

            CNewMasteryRecipe newMasteryRecipe = GetComponent<CNewMasteryRecipe>(newMasteryRecipes[0]);
            Dish dishData = GameData.Main.Get<Dish>(newMasteryRecipe.DishID);
            GenericMasteryDish genericMasteryDish = MasteryDishCrossReference.GetMasteryDishById(newMasteryRecipe.DishID);
            GenericMasteryDishCard genericMasteryDishCard = MasteryDishCrossReference.GetMasteryCardById(newMasteryRecipe.CardID);
            EntityManager.DestroyEntity(newMasteryRecipes[0]);

            // dishData is guaranteed to be a valid Dish-GDO even if the dish is part of this mod.
            //   genericMasteryDish & -DishCard will only be non-default if it's a Mastery dish, though.
            Mod.Logger.LogInfo($"HandleNewMasteryMenuDish - Processing Dish {{{dishData.Name}}}");
            if (dishData.UnlocksMenuItems.Count > 0)
            {
                HandleNewMenuItems(dishData, genericMasteryDish, genericMasteryDishCard);
            }
            if (dishData.UnlocksIngredients.Count > 0)
            {
                HandleNewMenuOptions(dishData, genericMasteryDish, genericMasteryDishCard);
            }
            if (dishData.ExtraOrderUnlocks.Count > 0)
            {
                HandleNewExtras(dishData, genericMasteryDish, genericMasteryDishCard);
            }
            if (genericMasteryDish != default && genericMasteryDish.SubstitutionIngredientSets.Count > 0)
            {
                HandleNewSubstitutions(dishData, genericMasteryDish, genericMasteryDishCard);
            }
        }

        private void HandleNewMenuItems(Dish dishData, GenericMasteryDish genericMasteryDish, GenericMasteryDishCard MasteryDishCard)
        {
            Mod.Logger.LogInfo("Handling new menu item");
            using var nonHandledMenuItems = NonHandledMenuItems.ToEntityArray(Allocator.Temp);
            using var cMenuItems = NonHandledMenuItems.ToComponentDataArray<CMenuItem>(Allocator.Temp);
            int matchCount = 0;
            int matchMax = dishData.UnlocksMenuItems.Count;
            for (int i = 0; i < nonHandledMenuItems.Length && matchCount < matchMax; i++)
            {
                var entity = nonHandledMenuItems[i];
                var menuItem = cMenuItems[i];

                // Continue until we find the matching non-Mastery Dish or the matching MasteryDishCard
                int relevantSourceDishID = MasteryDishCard == default ? dishData.ID : MasteryDishCard.GameDataObject.ID;
                Mod.Logger.LogInfo($"[HandleNewMenuItems] cMenuItem.SourceDish {{{menuItem.SourceDish}}}; cMenuItem.Item {{{menuItem.Item}}}; dishData.ID {{{dishData.ID}}}; " +
                    $"gMD.GDO.ID {{{genericMasteryDish?.GameDataObject.ID}}}; mDC.GDO.ID {{{MasteryDishCard?.GameDataObject.ID}}}; gmd.UniqueNameID {{{genericMasteryDish?.UniqueNameID}}}");
                if (menuItem.SourceDish != relevantSourceDishID || !genericMasteryDish.ResultingMenuItems.Any(rmi => rmi.Item.ID == menuItem.Item))
                {
                    continue;
                }

                MasteryMenuType type;

                // This should handle both vanilla and modded dishes now since we have a dishData handed in.
                if (genericMasteryDish == default && MasteryDishCard == default)
                {
                    Mod.Logger.LogInfo($"Handling non-Mastery menu item");
                    // Utilize the related Mastery Dish to get the Static Dish and compare
                    GenericMasteryDish relatedMasteryDish = MasteryDishCrossReference.GetRelatedMasteryMainDish(dishData.ID);
                    //TODO: try to get a CDynamicMenuItem for the entity and add ingredients for the fish itself..?
                    //TODO: handle case when Static Dish is a "duplicate" MenuItem of an existing GenericMasteryDish's MenuItem

                    type = MasteryMenuType.Static;
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
                //    type = MasteryMenuType.Fish;
                //}
                else
                {
                    // check if the genericMasteryDish matches
                    if (genericMasteryDish == default)
                    {
                        Mod.Logger.LogWarning($"Mastery Card ({MasteryDishCard.UniqueNameID}) does not seem to contain CMenuItem with ItemID = {menuItem.Item}");
                        continue;
                    }
                    type = MasteryMenuType.Mastery;

                    // TODO: Handle case when Mastery Dish would be a duplicate of an existing Static/Dynamic Dish's CMenuItem (by removing the one with MasteryMenuItem.Type = Mastery).
                    // Point the CMenuItem's SourceDish to the MasteryDish's ID instead of its parent MasteryDishCard's ID
                    menuItem.SourceDish = genericMasteryDish.GameDataObject.ID;

                    // Counteract HandleNewDish to make the weights more balanced when ordering since all menu items are bundled in large cards with this mod.
                    menuItem.Weight = 1;
                    EntityManager.AddComponentData(entity, menuItem);
                }

                // Source Dish by now should either be the original dish ID or the GenericMasteryDish's GDO ID, rather than a GMDC's GDO ID
                Mod.Logger.LogInfo($"Creating CMasteryMenuItem for entity {{Index = {entity.Index}}}");
                EntityManager.AddComponentData(entity, new CMasteryMenuItem()
                {
                    SourceMasteryDish = menuItem.SourceDish,
                    Type = type,
                    HasBeenProvided = false
                });
                matchCount++;
            }
        }

        private void HandleNewMenuOptions(Dish dishData, GenericMasteryDish genericMasteryDish, GenericMasteryDishCard genericMasteryDishCard)
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

                // Continue until we find the matching CAvailableIngredient. The relevant IngredientUnlock set will be a standard dish if non-Mastery
                //  or the MasteryDish's if it is Mastery.
                Mod.Logger.LogInfo($"[HandleNewMenuOptions] cAvailableIngredient {{MenuItem = {availableIngredient.MenuItem}, Ingredient = {availableIngredient.Ingredient}}}");
                HashSet<Dish.IngredientUnlock> relevantUISet = genericMasteryDish == default
                    ? dishData.UnlocksIngredients
                    : genericMasteryDish.IngredientsUnlocks;
                if (!relevantUISet.Any(ui => ui.MenuItem.ID == availableIngredient.MenuItem && ui.Ingredient.ID == availableIngredient.Ingredient))
                {
                    continue;
                }

                if (genericMasteryDish == default)
                {
                    // This is not a Mastery dish, so give the entity the appropriate component to prevent it from showing up here again
                    // TODO: do I want to actually add the recipe here..? Maybe worry about it when testing Autumn/Lake/Variety
                    Mod.Logger.LogInfo("Option is *not* Mastery; Adding CNonMasteryAvailableIngredient");
                    EntityManager.AddComponent<CNonMasteryAvailableIngredient>(entity);
                    break;
                }

                // This *is* a Mastery dish option, so add the identifier component and the CMasteryMenuItem component so that we
                //  know what ingredients will trigger this to be available.

                Mod.Logger.LogInfo($"Creating CMasteryMenuItem and CMasteryMenuItemOption for entity {{Index = {entity.Index}}}");
                EntityManager.AddComponentData(entity, new CMasteryMenuItem()
                {
                    Type = MasteryMenuType.Mastery,
                    SourceMasteryDish = genericMasteryDish.GameDataObject.ID,
                    HasBeenProvided = false
                });
                EntityManager.AddComponent<CMasteryMenuItemOption>(entity);
            }
        }

        private void HandleNewExtras(Dish dishData, GenericMasteryDish genericMasteryDish, GenericMasteryDishCard genericMasteryDishCard)
        {
            string soughtExtrasLog = "";
            foreach (var ingredient in dishData.ExtraOrderUnlocks)
            {
                soughtExtrasLog += $"  {{MenuItem = {ingredient.MenuItem.ID}, Ingredient = {ingredient.Ingredient.ID}}}\n";
            }
            Mod.Logger.LogInfo($"Handling new menu extra; Looking for possible extras objects: [\n{soughtExtrasLog}]");
            using var possibleExtrasEntities = NonHandledPossibleExtras.ToEntityArray(Allocator.Temp);
            using var possibleExtras = NonHandledPossibleExtras.ToComponentDataArray<CPossibleExtra>(Allocator.Temp);
            for (int i = 0; i < possibleExtrasEntities.Length; i++)
            {
                var entity = possibleExtrasEntities[i];
                var possibleExtra = possibleExtras[i];

                // Continue until we find the matching CAvailableIngredient. The relevant IngredientUnlock set will be a standard dish if non-Mastery
                //  or the MasteryDish's if it is Mastery.
                Mod.Logger.LogInfo($"[HandleNewMenuOptions] cPossibleExtra {{MenuItem = {possibleExtra.MenuItem}, Ingredient = {possibleExtra.Ingredient}}}");
                HashSet<Dish.IngredientUnlock> relevantUISet = genericMasteryDish == default
                    ? dishData.ExtraOrderUnlocks
                    : genericMasteryDish.ExtraOrderUnlocks;
                if (!relevantUISet.Any(ui => ui.MenuItem.ID == possibleExtra.MenuItem && ui.Ingredient.ID == possibleExtra.Ingredient))
                {
                    continue;
                }

                if (genericMasteryDish == default)
                {
                    // This is not a Mastery dish, so give the entity the appropriate component to prevent it from showing up here again
                    // TODO: do I want to actually add the recipe here..? Maybe worry about it when testing Autumn/Lake/Variety
                    Mod.Logger.LogInfo("Option is *not* Mastery; Adding CNonMasteryExtra");
                    EntityManager.AddComponent<CNonMasteryExtra>(entity);
                    break;
                }

                // This *is* a Mastery dish option, so add the identifier component and the CMasteryMenuItem component so that we
                //  know what ingredients will trigger this to be available.

                Mod.Logger.LogInfo($"Creating CMasteryMenuItem and CMasteryMenuItemOption for entity {{Index = {entity.Index}}}");
                EntityManager.AddComponentData(entity, new CMasteryMenuItem()
                {
                    Type = MasteryMenuType.Mastery,
                    SourceMasteryDish = genericMasteryDish.GameDataObject.ID,
                    HasBeenProvided = false
                });
                EntityManager.AddComponent<CMasteryMenuItemOption>(entity);
            }
        }

        private void HandleNewSubstitutions(Dish dishData, GenericMasteryDish genericMasteryDish, GenericMasteryDishCard genericMasteryDishCard)
        {
            using var handledSubstitutionSetEntities = HandledSubstitutionSetsQuery.ToEntityArray(Allocator.Temp);
            using var handledSubstitutionSetComps = HandledSubstitutionSetsQuery.ToComponentDataArray<CMasteryMenuSubstitutionSet>(Allocator.Temp);

            // We know that GMD is non-default before coming in here, so we can freely access it w/o safety checks
            Mod.Logger.LogInfo($"Handling new Mastery Dish subsitution ingredient set.");
            bool found = false;
            for (int i = 0; i < handledSubstitutionSetComps.Length; i++)
            {
                var component = handledSubstitutionSetComps[i];
                if (component.SourceMasteryDishID == genericMasteryDish.GameDataObject.ID)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                Mod.Logger.LogWarning($"Found a matching entity, which shouldn't happen...");
                return;
            }

            foreach (var substitutionSet in genericMasteryDish.SubstitutionIngredientSets)
            {
                Mod.Logger.LogInfo($"Creating new entity and component for substitution set " +
                    $"{{GMD = {genericMasteryDish.UniqueNameID}, Subbed Item = {substitutionSet.Item.name}}}");
                var newEntity = EntityManager.CreateEntity();
                EntityManager.AddComponentData(newEntity, new CMasteryMenuSubstitutionSet()
                {
                    SourceMasteryDishID = genericMasteryDish.GameDataObject.ID,
                    SubstitutedItemID = substitutionSet.Item.ID
                });
            }
        }
    }
}
