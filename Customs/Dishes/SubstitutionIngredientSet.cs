using KitchenData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasteryMenu.Customs.Dishes
{
    public class SubstitutionIngredientSet
    {
        public Item Item;
        public HashSet<Item> SubstitutionItems;

        public SubstitutionIngredientSet(Item item, HashSet<Item> substitutionItems)
        {
            Item = item;
            SubstitutionItems = substitutionItems;
        }
    }
}
