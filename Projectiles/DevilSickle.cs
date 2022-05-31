﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace tsorcRevamp.Projectiles {
    public class DevilSickle : ModProjectile {
        public override void SetDefaults() {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.scale = 0.9f;
            Projectile.alpha = 100;
            Projectile.timeLeft = 130;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void AI() {
            int dust = Dust.NewDust(
            Projectile.position,
            Projectile.width,
            Projectile.height,
            6,
            Projectile.velocity.X,
            Projectile.velocity.Y,
            90,
            default,
            1.8f
            );
            Main.dust[dust].noGravity = true;
            Projectile.rotation += Projectile.direction * 0.8f;

            if (Projectile.velocity.X <= 7f && Projectile.velocity.Y <= 7f && Projectile.velocity.X >= -7f && Projectile.velocity.Y >= -7f) {
                Projectile.velocity.X *= 1.06f;
                Projectile.velocity.Y *= 1.06f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if (Main.rand.Next(5) == 0) {
                target.AddBuff(BuffID.OnFire, 420); //blaze it 
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit) {
            if (Main.rand.Next(5) == 0) {
                target.AddBuff(BuffID.OnFire, 420); //blaze it 
            }
        }
        public override void Kill(int timeLeft) {
            Main.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y, 9);
        }
    }
}
