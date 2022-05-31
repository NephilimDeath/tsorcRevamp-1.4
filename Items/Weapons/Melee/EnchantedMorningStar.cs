﻿using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {

    public class EnchantedMorningStar : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Enchantment does increased damage against mages and ghosts.");
        }

        public override void SetDefaults() {
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.scale = 0.8f;
            Item.useAnimation = 44;
            Item.useTime = 44;
            Item.damage = 33;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Green;
            Item.shootSpeed = 12;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = PriceByRarity.Green_2;
            Item.melee = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.EnchantedMorningStar>();
        }

        public override void AddRecipes() {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(Mod.GetItem("OldMorningStar"), 1);
            recipe.AddIngredient(Mod.GetItem("EphemeralDust"), 30);
            recipe.AddIngredient(Mod.GetItem("DarkSoul"), 6000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
