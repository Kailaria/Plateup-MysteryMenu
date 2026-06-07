using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMasteryMenu;
using KitchenMasteryMenu.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMasteryMenu.Customs.Appliances
{
    public class MasteryIngredientProviderCakes3 : MasteryIngredientProvider
    {
        public override string UniqueNameID => "Mastery Ingredient Provider Cakes 3";

        public override List<(Locale, ApplianceInfo)> InfoList => new List<(Locale, ApplianceInfo)>()
        {
            (Locale.English, new ApplianceInfo()
                {
                    Name = "Mastery Menu - Provider",
                    Description = "Provides ingredients for the Mastery menu, randomized each day."
                })
        };
    }
}
