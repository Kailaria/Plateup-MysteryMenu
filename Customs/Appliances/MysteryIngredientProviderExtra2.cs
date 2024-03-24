using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu;
using KitchenMysteryMenu.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenMysteryMenu.Customs.Appliances
{
    public class MysteryIngredientProviderExtra2 : MysteryIngredientProvider
    {
        public override string UniqueNameID => "Mystery Ingredient Provider Extra 2";

        public override List<(Locale, ApplianceInfo)> InfoList => new List<(Locale, ApplianceInfo)>()
        {
            (Locale.English, new ApplianceInfo()
                {
                    Name = "Mystery Menu - Provider",
                    Description = "Provides ingredients for the mystery menu, randomized each day."
                })
        };
    }
}
