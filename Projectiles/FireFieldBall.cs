﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace tsorcRevamp.Projectiles {
    class FireFieldBall : ModProjectile {

        public override string Texture => "tsorcRevamp/Projectiles/GreatFireballBall";
        public override void SetDefaults() {
            Projectile.aiStyle = 9;
            Projectile.friendly = true;
            Projectile.height = 16;
            Projectile.width = 16;
            Projectile.light = 0.8f;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            if (Projectile.soundDelay == 0 && Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) > 2f)
            {
                Projectile.soundDelay = 10;
                Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 9);
            }

            int thisDust = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X, Projectile.position.Y - Projectile.velocity.Y), Projectile.width, Projectile.height, 15, 0, 0, 100, default, 2f);
            Main.dust[thisDust].noGravity = true;

            Projectile.rotation += 0.3f;
        }

        public override void Kill(int timeLeft) {            
            Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
            Projectile.NewProjectile(Projectile.position.X + (float)(Projectile.width), Projectile.position.Y + (float)(Projectile.height), 0, 0, ModContent.ProjectileType<FireField>(), Projectile.damage, 3f, Projectile.owner);

            for (int i = 0; i < 20; i++) {
                int thisDust = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X, Projectile.position.Y - Projectile.velocity.Y), Projectile.width, Projectile.height, 15, 0, 0, 100, default, 2f);
                Main.dust[thisDust].noGravity = true;
                thisDust = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X, Projectile.position.Y - Projectile.velocity.Y), Projectile.width, Projectile.height, 15, 0, 0, 100, default, 2f);
                Main.dust[thisDust].noGravity = true;
            }            
        }
    }
}
