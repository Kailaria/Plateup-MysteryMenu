﻿using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMysteryMenu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMysteryMenu.Customs.Dishes.Pies
{
    public class MysteryPiesBaseDish : GenericMysteryDish
    {
        protected override string NameTag => "Mystery Pies Dish";
        public override Dish OrigDish => (Dish)GDOUtils.GetExistingGDO(DishReferences.PieBase);
        public override DishType Type => DishType.Main;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.None;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.None;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override bool IsUnlockable => false;
        public override Item RequiredDishItem => (Item)GDOUtils.GetExistingGDO(ItemReferences.Plate);
        public override bool RequiredNoDishItem => false;
        public override bool IsAvailableAsLobbyOption => false;
        public override int Difficulty => 2;
        public override Dictionary<Locale, string> Recipe => new()
        {
            {
                Locale.English,
                "<color=yellow>Requires ingredients:</color> Flour + <i>variant filling</i>\n" +
                "Knead flour (or add water) to make dough, then knead into pie crust. Add filling and cook."
            }
        };
        public override List<(Locale, UnlockInfo)> InfoList => new()
        {
            (Locale.English, new UnlockInfo()
            {
                Name = "Mystery - Pies",
                Description = "Adds meat pie as a main when <b>Flour</b> and <b>Meat</b> are present",
                FlavourText = "Are you ready for the best pies in PlateUp!?"
            })
        };

        public override List<Dish.MenuItem> ResultingMenuItems => new()
        {
            new()
            {
                Item = (Item)GDOUtils.GetExistingGDO(ItemReferences.PiePlated),
                Phase = MenuPhase.Main,
                Weight = 1
            }
        };

        public override HashSet<Item> MinimumRequiredMysteryIngredients => new HashSet<Item>();
        public override bool RequiresVariant => true;
        public override List<Unlock> HardcodedRequirements => new()
        {
            GDOUtils.GetCastedGDO<Dish, MysteryMenuBaseMainsDish>()
        };
    }
}
