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
    public class MasteryIngredientProvider : CustomAppliance
    {
        public override string UniqueNameID => "Mastery Ingredient Provider";
        public override GameObject Prefab => ((Appliance) GDOUtils.GetExistingGDO(ApplianceReferences.SourceFish)).Prefab;
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>()
        {
            new CMasteryMenuProvider() {
                Type = Utils.MasteryMenuType.Mastery
            }
        };
        public override bool IsNonInteractive => false;
        public override OccupancyLayer Layer => OccupancyLayer.Default;
        public override bool IsPurchasable => false;
        public override bool IsPurchasableAsUpgrade => false;
        public override DecorationType ThemeRequired => DecorationType.Null;
        public override ShoppingTags ShoppingTags => ShoppingTags.Basic;
        public override RarityTier RarityTier => RarityTier.Special;
        public override PriceTier PriceTier => PriceTier.Free;
        public override bool StapleWhenMissing => false;
        public override bool SellOnlyAsDuplicate => false;
        public override bool PreventSale => true;
        public override bool IsNonCrated => false;

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
