using KitchenData;
using KitchenLib.Utils;
using KitchenMysteryMenu.Customs.Dishes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Utils
{
    public static class MysteryDishCrossReference
    {
        public static Dictionary<int, GenericMysteryDish> DishDictionary = new();
        public static Dictionary<int, GenericMysteryDishCard> DishCardDictionary = new();

        public static void RegisterDish(Dish dish, GenericMysteryDish mysteryDish)
        {
            DishDictionary.Add(dish.ID, mysteryDish);
        }

        public static void RegisterDishCard(Dish dish, GenericMysteryDishCard mysteryDishCard)
        {
            DishCardDictionary.Add(dish.ID, mysteryDishCard);
        }

        public static GenericMysteryDish GetRelatedMysteryDish(Dish dish)
        {
            return GetRelatedMysteryDish(dish.ID);
        }

        public static GenericMysteryDish GetRelatedMysteryDish(int id)
        {
            return DishDictionary[id];
        }

        public static GenericMysteryDish GetMysteryDishById(int id)
        {
            return DishDictionary.Values.Where(gmd => gmd.BaseGameDataObjectID == id).FirstOrDefault();
        }

        public static GenericMysteryDishCard GetMysteryCardById(int id)
        {
            return DishCardDictionary.Values.Where(gmdc => gmdc.BaseGameDataObjectID == id).FirstOrDefault();
        }

        public static GenericMysteryDish GetMysteryDishByMenuItem(int menuItem)
        {
            return DishDictionary.Values
                .Where(gmd => gmd.ResultingMenuItems.Select(mi => mi.Item.ID).Contains(menuItem))
                .FirstOrDefault();
        }
    }
}
