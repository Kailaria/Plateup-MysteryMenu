using KitchenData;
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
        public static Dictionary<int, GenericMysteryDish> DishDictionary = new Dictionary<int, GenericMysteryDish>();

        public static void RegisterDish(Dish dish, GenericMysteryDish mysteryDish)
        {
            DishDictionary.Add(dish.ID, mysteryDish);
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
    }
}
