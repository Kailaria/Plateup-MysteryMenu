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
        public static HashSet<GenericMysteryDish> MysteryDishes = new();
        public static HashSet<GenericMysteryDishCard> MysteryDishCards = new();

        public static void RegisterDish(GenericMysteryDish mysteryDish)
        {
            Mod.Logger.LogInfo($"Registering {{{mysteryDish.UniqueNameID}}} with GDO.ID {{{mysteryDish.GameDataObject.ID}}}");
            MysteryDishes.Add(mysteryDish);
        }

        public static void RegisterDishCard(GenericMysteryDishCard mysteryDishCard)
        {
            Mod.Logger.LogInfo($"Registering {{{mysteryDishCard.UniqueNameID}}} with GDO.ID {{{mysteryDishCard.GameDataObject.ID}}}");
            MysteryDishCards.Add(mysteryDishCard);
        }

        public static GenericMysteryDish GetRelatedMysteryDish(Dish dish)
        {
            return GetRelatedMysteryMainDish(dish.ID);
        }

        public static GenericMysteryDish GetRelatedMysteryMainDish(int id)
        {
            return MysteryDishes.Where(gmd => gmd.OrigDish.ID == id && gmd.ResultingMenuItems.Count > 0).FirstOrDefault();
        }

        public static GenericMysteryDish GetRelatedMysteryOptionDish(int id)
        {
            return MysteryDishes.Where(gmd => gmd.OrigDish.ID == id && gmd.IngredientsUnlocks.Count > 0).FirstOrDefault();
        }

        public static GenericMysteryDish GetMysteryDishById(int id)
        {
            return MysteryDishes.Where(gmd => gmd.GameDataObject.ID == id).FirstOrDefault();
        }

        public static GenericMysteryDishCard GetMysteryCardById(int id)
        {
            return MysteryDishCards.Where(gmdc => gmdc.GameDataObject.ID == id).FirstOrDefault();
        }

        public static GenericMysteryDish GetMysteryDishByMenuItem(int menuItem)
        {
            return MysteryDishes
                .Where(gmd => gmd.ResultingMenuItems.Select(mi => mi.Item.ID).Contains(menuItem))
                .FirstOrDefault();
        }
    }
}
