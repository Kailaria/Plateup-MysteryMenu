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
    public class MasteryTrayProviderCakes : CustomAppliance
    {
        public override string UniqueNameID => "Mastery Tray Provider";
        public override GameObject Prefab => ((Appliance) GDOUtils.GetExistingGDO(ApplianceReferences.SourceCookieTray)).Prefab;
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>()
        {
            new CMasteryMenuProvider() {
                Type = Utils.MasteryMenuType.MasteryTray
            },
            new CItemProvider() {
                ProvidedItem = 0,
                Available = 0,
                Maximum = 1,
                PreventReturns = true
            },
            new CItemHolder()
        };
        public override List<Appliance.ApplianceProcesses> Processes => 
            ((Appliance) GDOUtils.GetExistingGDO(ApplianceReferences.SourceCookieTray)).Processes.ToList();
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
                    Name = "Mastery Menu - Mastery Tray",
                    Description = "Provides a random tray/tin/pan for the Mastery menu each day."
                })
        };
    }
}
