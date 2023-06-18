using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;

namespace tsorcRevamp.Projectiles.Ranged.Runeterra
{
	public class NuclearMushroom: ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
            Main.projFrames[Projectile.type] = 3;
        }

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;

			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 100 * 60;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.knockBack = 0f;
            Projectile.ContinuouslyUpdateDamageStats = true;
        }
        public int BuiltSoundStyle;
        public override void OnSpawn(IEntitySource source)
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.damage = 0;
            BuiltSoundStyle = Main.rand.Next(1, 4);
            if (BuiltSoundStyle == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Ranged/OmegaSquadRifle/ShroomBuilt1") with { Volume = 1f }, Projectile.Center);
            }
            else if (BuiltSoundStyle == 2)
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Ranged/OmegaSquadRifle/ShroomBuilt2") with { Volume = 1f }, Projectile.Center);
            }
            else
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Ranged/OmegaSquadRifle/ShroomBuilt3") with { Volume = 1f }, Projectile.Center);
            }
        }
        public override void AI()
        {
            int frameSpeed = 15;

            Projectile.frameCounter++;

            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            // Some visuals here
            Lighting.AddLight(Projectile.Center, Color.GreenYellow.ToVector3() * 1f);
        }
        public override bool? CanDamage()
        {
            if (Projectile.timeLeft > 100 * 60 - 3 * 60)
            {
                return false;
            }
            return null;
        }
        public int BoomSoundStyle;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player owner = Main.player[Projectile.owner];
            BoomSoundStyle = Main.rand.Next(1, 4);
            if (BoomSoundStyle == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Ranged/OmegaSquadRifle/ShroomBoom1") with { Volume = 1f }, target.Center);
            } else if (BoomSoundStyle == 2)
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Ranged/OmegaSquadRifle/ShroomBoom2") with { Volume = 1f }, target.Center);
            } else
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Ranged/OmegaSquadRifle/ShroomBoom3") with { Volume = 1f }, target.Center);
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<NuclearMushroomExplosion>(), Projectile.damage / 2, Projectile.knockBack * 10, Projectile.owner);
        }
    }
}