using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Customs.Appliances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMysteryMenu.Customs.Ingredients
{
    public class MysteryLasagnePastaSheet : GenericMysteryItem
    {
        public override Item ExistingGDO => (Item)GDOUtils.GetExistingGDO(ItemReferences.LasagnePastaSheet);
        protected override string NameTag => "Mystery Lasagne Pasta Sheet";
        public override Appliance DedicatedProvider => GDOUtils.GetCastedGDO<Appliance, MysteryIngredientProviderComplexities>();
    }
}
