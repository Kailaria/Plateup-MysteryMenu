using Kitchen;
using KitchenMysteryMenu.Components;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;

namespace KitchenMysteryMenu.Systems
{
    public class SelectMysteryMenuOfDay : StartOfDaySystem
    {
        EntityQuery MysteryItemProviders;
        EntityQuery MenuItems;
        EntityQuery DisabledMenuItems;
        EntityQuery NonMysteryProviders;

        protected override void Initialise()
        {
            base.Initialise();
            MysteryItemProviders = GetEntityQuery(typeof(CItemProvider), typeof(CMysteryMenuProvider));
            NonMysteryProviders = GetEntityQuery(new QueryHelper()
                .All(typeof(CItemProvider))
                .None(typeof(CMysteryMenuProvider)));
            MenuItems = GetEntityQuery(typeof(CMenuItem), typeof(CMysteryMenuItem));
            DisabledMenuItems = GetEntityQuery(typeof(CMysteryMenuItem), typeof(CDisabledMysteryMenu));
            RequireForUpdate(MenuItems);
        }

        protected override void OnUpdate()
        {
            // Borrowing a lot of code from SelectFishOfDay, but dish & menu selection will need to be vastly different
            EntityManager.RemoveComponent<CDisabledMysteryMenu>(DisabledMenuItems);
            using var menuItemEntities = MenuItems.ToEntityArray(Allocator.Temp);
            using var menuItemMysteryComps = MenuItems.ToComponentDataArray<CMysteryMenuItem>(Allocator.Temp);
            using var providerEntities = MysteryItemProviders.ToEntityArray(Allocator.Temp);
            using var providerMysteryComps = MysteryItemProviders.ToComponentDataArray<CMysteryMenuProvider>(Allocator.Temp);

            // algo 1: Determine existing, permanently available ingredients
            //      (ignore all possible Dynamic dishes; but this parenthetical should be handled already by HandleNewMysteryMenuDish)

            // algo 2: Begin selection & randomization loop until all Mystery Providers have been assigned
                // algo 2a: Determine available Mystery MenuItem entities that are satisfied with the all available ingredients (including those
                //          provided by Mystery Providers)

                // algo 2b: Randomly select more recipes, one at a time, until the Mystery Providers have all been given their assignments for the day.

                // algo 2c: Ensure that the only recipes that can be selected are those that require at most the number of available ingredient providers,
                //          AND whose prerequisite dishes (if any) have already been satisfied.
                //      * i.e. 2 available providers during a given randomization => only recipes that need 1-2 more ingredients to be valid
                //      * Need to make sure 

                // algo 2d: Inversely weight each recipe based on how many other validly available recipes would also utilize the same ingredients
                //      * Inverse is so that each *ingredient* has a total contributed weight of 1 across all recipes that utilize it
                //      * Prioritize recipes that have not been provided (since that will generally cover ingredients, too, but not always)

                // algo 2e: Given the selected recipe, assign the ingredients to Mystery Providers that have not already been provided, whether randomly
                //          or permanently.

            // algo 3: With all Mystery providers assigned, make a final sweep to determine the available recipes, and disable those that cannot be fulfilled

            //foreach (MysteryMenuType type in Enum.GetValues(typeof(MysteryMenuType)))
            //{
            //    // Sort each menu entity of the given type into those that have been provided and those that haven't
            //    List<Entity> olderTypeMatchedMenuItemEntities = new List<Entity>();
            //    List<Entity> newerTypeMatchedMenuItemEntities = new List<Entity>();
            //    for (int i = 0; i < menuItemMysteryComps.Length; i++)
            //    {
            //        if (menuItemMysteryComps[i].Type == type)
            //        {
            //            (menuItemMysteryComps[i].HasBeenProvided ? olderTypeMatchedMenuItemEntities : newerTypeMatchedMenuItemEntities).Add(menuItemEntities[i]);
            //        }
            //    }

            //    List<Entity> typeMatchingProviderEntities = new List<Entity>();
            //    for (int j = 0; j < providerEntities.Length; j++)
            //    {
            //        if (providerMysteryComps)
            //    }
            //}
        }
    }
}
