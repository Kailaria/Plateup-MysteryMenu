using KitchenData;
using KitchenLib.Utils;
using KitchenMasteryMenu.Customs.Dishes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMasteryMenu.Utils
{
    public static class MasteryDishCrossReference
    {
        public static HashSet<GenericMasteryDish> MasteryDishes = new();
        public static HashSet<GenericMasteryDishCard> MasteryDishCards = new();

        public static void RegisterDish(GenericMasteryDish MasteryDish)
        {
            Mod.Logger.LogInfo($"Registering {{{MasteryDish.UniqueNameID}}} with GDO.ID {{{MasteryDish.GameDataObject.ID}}}");
            MasteryDishes.Add(MasteryDish);
        }

        public static void RegisterDishCard(GenericMasteryDishCard MasteryDishCard)
        {
            Mod.Logger.LogInfo($"Registering {{{MasteryDishCard.UniqueNameID}}} with GDO.ID {{{MasteryDishCard.GameDataObject.ID}}}");
            MasteryDishCards.Add(MasteryDishCard);
        }

        public static GenericMasteryDish GetRelatedMasteryDish(Dish dish)
        {
            return GetRelatedMasteryMainDish(dish.ID);
        }

        public static GenericMasteryDish GetRelatedMasteryMainDish(int id)
        {
            return MasteryDishes.Where(gmd => gmd.OrigDish.ID == id && gmd.ResultingMenuItems.Count > 0).FirstOrDefault();
        }

        public static GenericMasteryDish GetRelatedMasteryOptionDish(int id)
        {
            return MasteryDishes.Where(gmd => gmd.OrigDish.ID == id && gmd.IngredientsUnlocks.Count > 0).FirstOrDefault();
        }

        public static GenericMasteryDish GetMasteryDishById(int id)
        {
            return MasteryDishes.Where(gmd => gmd.GameDataObject.ID == id).FirstOrDefault();
        }

        public static GenericMasteryDishCard GetMasteryCardById(int id)
        {
            return MasteryDishCards.Where(gmdc => gmdc.GameDataObject.ID == id).FirstOrDefault();
        }

        public static GenericMasteryDish GetMasteryDishByMenuItem(int menuItem)
        {
            return MasteryDishes
                .Where(gmd => gmd.ResultingMenuItems.Select(mi => mi.Item.ID).Contains(menuItem))
                .FirstOrDefault();
        }
    }
}
