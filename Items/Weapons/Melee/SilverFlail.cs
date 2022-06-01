﻿using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {
    class SilverFlail : ModItem {

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.damage = 9;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.White;
            Item.shootSpeed = 14;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = 5000;
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ModContent.ProjectileType<Projectiles.SilverBall>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(ItemID.SilverBar, 1);
            recipe.AddIngredient(ItemID.Chain, 2);
            recipe.AddIngredient(ModContent.ItemType<Items.DarkSoul>(), 100);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
