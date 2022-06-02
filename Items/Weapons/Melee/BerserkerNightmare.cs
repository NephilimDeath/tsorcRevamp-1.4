﻿using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee
{

    public class BerserkerNightmare : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.useAnimation = 44;
            Item.useTime = 44;
            Item.damage = 49;
            Item.knockBack = 8;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.LightPurple;
            Item.shootSpeed = 13;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = PriceByRarity.LightPurple_6;
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ModContent.ProjectileType<Projectiles.BerserkerSphere>();
        }

        public override void AddRecipes()
        {
            Terraria.Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DaoofPow, 2);
            recipe.AddIngredient(Mod.Find<ModItem>("DarkSoul").Type, 70000);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }


    }
}
