using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Projectiles.Magic.Runeterra
{
    public class OrbOfDeceptionFlame : ModProjectile
    {
        public float angularSpeed = 0.05f;
        public float circleRad1 = 50f;

        public float currentAngle;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
			Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 900;
            Projectile.extraUpdates = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            float maxDetectRadius = 500f;
            float projSpeed = 5f;

            NPC closestNPC = FindClosestNPC(maxDetectRadius);
            Visuals();
            if (closestNPC == null || Projectile.timeLeft >= 780)
            {
                currentAngle += (angularSpeed / (50f * 0.001f + 1f));

                Vector2 offset = new Vector2(0, 50f).RotatedBy(-currentAngle);

                Projectile.Center = player.Center + offset;

                Projectile.velocity = Projectile.rotation.ToRotationVector2();

                return;
            }

            Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                if (target.CanBeChasedBy())
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }
            return closestNPC;
        }
        private void Visuals()
        {
            Lighting.AddLight(Projectile.Center, Color.LightSteelBlue.ToVector3() * 0.78f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            player.statMana += player.GetManaCost(player.HeldItem) / 2;
        }
    }
}