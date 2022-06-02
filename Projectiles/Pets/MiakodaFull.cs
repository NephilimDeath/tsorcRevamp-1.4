﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Projectiles.Pets
{
    class MiakodaFull : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Full Moon Miakoda");
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyHornet);
            Projectile.width = 18;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            AIType = ProjectileID.BabyHornet;
            Projectile.scale = 1f;
            Projectile.scale = 0.85f;
            DrawOffsetX = -8;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.hornet = false;

            return true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            tsorcRevampPlayer modPlayer = player.GetModPlayer<tsorcRevampPlayer>();
            float MiakodaVol = ModContent.GetInstance<tsorcRevampConfig>().MiakodaVolume / 100f;
            Lighting.AddLight(Projectile.position, .6f, .6f, .4f);

            Vector2 idlePosition = player.Center;
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();

            if (player.dead)
            {
                modPlayer.MiakodaFull = false;
            }
            if (modPlayer.MiakodaFull)
            {
                Projectile.timeLeft = 2;
            }

            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1500f)
            {
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            if ((modPlayer.MiakodaEffectsTimer > 720) && (Main.rand.Next(3) == 0))
            {
                if (Projectile.direction == 1)
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X + 4, Projectile.position.Y), Projectile.width - 6, Projectile.height - 6, 57, Projectile.velocity.X * 0f, Projectile.velocity.Y * 0f, 30, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                }
                if (Projectile.direction == -1)
                {
                    int dust = Dust.NewDust(new Vector2(Projectile.position.X - 4, Projectile.position.Y), Projectile.width - 6, Projectile.height - 6, 57, Projectile.velocity.X * 0f, Projectile.velocity.Y * 0f, 30, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                }
            }

            if (modPlayer.MiakodaEffectsTimer == 720 && MiakodaVol != 0) //sound effect the moment the timer reaches 420, to signal pet ability ready.
            {
                string[] ReadySoundChoices = new string[] { "Sounds/Custom/MiakodaChaaa", "Sounds/Custom/MiakodaChao", "Sounds/Custom/MiakodaDootdoot", "Sounds/Custom/MiakodaHi", "Sounds/Custom/MiakodaOuuee" };
                string ReadySound = Main.rand.Next(ReadySoundChoices);
                if (!Main.dedServ)
                {
                    Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle(ReadySound).WithVolume(.4f * MiakodaVol).WithPitchVariance(.2f), Projectile.Center);
                }
            }

            if (modPlayer.MiakodaFullHeal2) //splash effect and sound once player gets crit+heal.
            {
                if (MiakodaVol != 0)
                {
                    string[] AmgerySoundChoices = new string[] { "Sounds/Custom/MiakodaScream", "Sounds/Custom/MiakodaChaoExcl", "Sounds/Custom/MiakodaUwuu" };
                    string AmgerySound = Main.rand.Next(AmgerySoundChoices);
                    if (!Main.dedServ)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle(AmgerySound).WithVolume(.6f * MiakodaVol).WithPitchVariance(.2f), Projectile.Center);
                    }
                }

                for (int d = 0; d < 90; d++)
                {
                    if (Projectile.direction == 1)
                    {
                        int dust = Dust.NewDust(new Vector2(Projectile.position.X - 4, Projectile.position.Y), Projectile.width - 6, Projectile.height - 6, 57, 0f, 0f, 30, default(Color), 1.5f);
                        Main.dust[dust].velocity *= Main.rand.NextFloat(0.5f, 3.5f);
                        Main.dust[dust].noGravity = true;
                    }
                    if (Projectile.direction == -1)
                    {
                        int dust = Dust.NewDust(new Vector2(Projectile.position.X + 4, Projectile.position.Y), Projectile.width - 6, Projectile.height - 6, 57, 0f, 0f, 30, default(Color), 1.5f);
                        Main.dust[dust].velocity *= Main.rand.NextFloat(0.5f, 3.5f);
                        Main.dust[dust].noGravity = true;
                    }
                }

                if (modPlayer.MiakodaEffectsTimer < 720)
                {
                    player.GetModPlayer<tsorcRevampPlayer>().MiakodaFullHeal2 = false;
                }
            }
        }
    }
}
