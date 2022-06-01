﻿using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Ranged {
    class Crossbow : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("High crit rate" +
                                "\nUses Bolts as ammo. 10 are crafted with" +
                                "\none wood and two Dark Souls at a Demon Altar" +
                                 "\nBolts pierce once");
        }
        public override void SetDefaults() {
            Item.damage = 16;
            Item.height = 28;
            Item.knockBack = 4;
            Item.crit = 16;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = Mod.Find<ModItem>("Bolt").Type;
            Item.shootSpeed = 10;
            Item.useAnimation = 45;
            Item.UseSound = SoundID.Item5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 45;
            Item.value = 1400;
            Item.width = 12;
        }

        public override void AddRecipes() {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddIngredient(Mod.GetItem("DarkSoul"), 150);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
