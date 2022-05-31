﻿using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace tsorcRevamp.Projectiles {
    class HeavenSword : ModProjectile {
        public override string Texture => "tsorcRevamp/Items/Weapons/Ranged/HeavenSword";
        public override void SetDefaults() { 
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI() {
            if (Main.rand.Next(20) == 0) {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 57, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 200, default, 1f);
            }
        }
    }
}
