using Terraria;
using Terraria.ModLoader;

namespace tsorcRevamp.Projectiles.Ranged
{
    class BarrageBlast : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.light = 0.3f;
            Projectile.knockBack = 0f;
        }
        public override void AI()
        {

        }
        public override void OnKill(int timeLeft)
        {

        }
    }

}
