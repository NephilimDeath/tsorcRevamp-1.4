﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Magic
{
    class FarronDart : ModItem
    {
        public override string Texture => "tsorcRevamp/Items/Weapons/Magic/SoulArrowStaff";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Farron Dart");
            Tooltip.SetDefault("Rapidly shoots controllable farron darts");
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.damage = 13;
            Item.knockBack = 1.5f;
            Item.mana = 2;
            Item.crit = 4;
            Item.shootSpeed = 7;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item8;
            Item.value = 3000;
            Item.DamageType = DamageClass.Magic;
            Item.shoot = ModContent.ProjectileType<Projectiles.FarronDart>();
            Item.rare = ItemRarityID.Blue;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = Mod.GetTexture("Items/Weapons/Magic/FarronDart_Scroll");
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), drawColor, 0f, origin, scale, SpriteEffects.None, 0.1f);

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = Mod.GetTexture("Items/Weapons/Magic/FarronDart_Scroll");
            spriteBatch.Draw(texture, Item.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), lightColor, 0f, new Vector2(19, 20), Item.scale, SpriteEffects.None, 0.1f);

            return false;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 speed, int type, int damage, float knockBack)
        {

            Vector2 muzzleOffset = Vector2.Normalize(speed) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
                position.Y += -14;
            }
            float mySpeedX = Main.mouseX + Main.screenPosition.X - position.X;
            float mySpeedY = Main.mouseY + Main.screenPosition.Y - position.Y;
            float speedAbs = (float)Math.Sqrt((mySpeedX * mySpeedX) + (mySpeedY * mySpeedY));
            speedAbs = 7f / speedAbs; // for speed consistency
            mySpeedX *= speedAbs;
            mySpeedY *= speedAbs;
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), new Vector2(position.X, position.Y), new Vector2(mySpeedX, mySpeedY), ModContent.ProjectileType<Projectiles.FarronDart>(), damage, knockBack, player.whoAmI);

            return false;
        }



        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(Mod.Find<ModItem>("WoodenWand").Type, 1);
            recipe.AddIngredient(Mod.Find<ModItem>("DarkSoul").Type, 200);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }
    }
}
