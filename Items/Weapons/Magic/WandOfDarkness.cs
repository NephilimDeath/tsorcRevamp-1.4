﻿using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Magic {
    class WandOfDarkness : ModItem {

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Wand of Darkness");
            Tooltip.SetDefault("Shoots a piercing bolt of darkness");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults() {
            Item.autoReuse = true;
            Item.width = 12;
            Item.height = 17;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.damage = 11;
            Item.knockBack = 1f;
            Item.mana = 2;
            Item.UseSound = SoundID.Item8;
            Item.shootSpeed = 6;
            Item.noMelee = true;
            Item.value = PriceByRarity.Blue_1;
            Item.rare = ItemRarityID.Blue;
            Item.magic = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.ShadowBall>();
        }
        public override void AddRecipes() {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(Mod.GetItem("WoodenWand"), 1);
            recipe.AddIngredient(Mod.GetItem("DarkSoul"), 150);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
