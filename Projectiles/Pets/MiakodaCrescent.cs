﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using tsorcRevamp;

namespace tsorcRevamp.Projectiles.Pets
{
    class MiakodaCrescent : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crescent Moon Miakoda");
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
            aiType = ProjectileID.BabyHornet;
            Projectile.scale = 0.85f;
            drawOffsetX = -8;
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
            Lighting.AddLight(Projectile.position, .6f, .45f, .6f);


            Vector2 idlePosition = player.Center;
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();


            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 1500f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            if (player.dead)
            {
                modPlayer.MiakodaCrescent = false;
            }
            if (modPlayer.MiakodaCrescent)
            {
                Projectile.timeLeft = 2;
            }

            if (modPlayer.MiakodaEffectsTimer > 720)
            {
                Lighting.AddLight(Projectile.position, .8f, .65f, .8f);

                if (Main.rand.Next(3) == 0)
                {
                    if (Projectile.direction == 1)
                    {
                        int dust = Dust.NewDust(new Vector2(Projectile.position.X + 6, Projectile.position.Y), Projectile.width - 6, Projectile.height - 6, 234, Projectile.velocity.X * 0f, Projectile.velocity.Y * 0f, 30, default(Color), 1f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].noLight = true;
                    }
                    if (Projectile.direction == -1)
                    {
                        int dust = Dust.NewDust(new Vector2(Projectile.position.X - 6, Projectile.position.Y), Projectile.width - 6, Projectile.height - 6, 234, Projectile.velocity.X * 0f, Projectile.velocity.Y * 0f, 30, default(Color), 1f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].noLight = true;
                    }
                }
            }

            if (modPlayer.MiakodaEffectsTimer == 720 && MiakodaVol != 0) //sound effect the moment the timer reaches 420, to signal pet ability ready.
            {
                string[] ReadySoundChoices = new string[] { "Sounds/Custom/MiakodaChaaa", "Sounds/Custom/MiakodaChao", "Sounds/Custom/MiakodaDootdoot", "Sounds/Custom/MiakodaHi", "Sounds/Custom/MiakodaOuuee" };
                string ReadySound = Main.rand.Next(ReadySoundChoices);
                Main.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, ReadySound).WithVolume(.4f * MiakodaVol).WithPitchVariance(.2f), Projectile.Center);
            }

            if (modPlayer.MiakodaCrescentDust2) //splash effect and sound once player gets crit+heal.
            {
                if (MiakodaVol != 0)
                {
                    string[] AmgerySoundChoices = new string[] { "Sounds/Custom/MiakodaScream", "Sounds/Custom/MiakodaChaoExcl", "Sounds/Custom/MiakodaUwuu" };
                    string AmgerySound = Main.rand.Next(AmgerySoundChoices);
                    Main.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, AmgerySound).WithVolume(.6f * MiakodaVol).WithPitchVariance(.2f), Projectile.Center);
                }

                for (int d = 0; d < 90; d++)
                {
                    if (Projectile.direction == 1)
                    {
                        int dust = Dust.NewDust(new Vector2(Projectile.position.X - 4, Projectile.position.Y + 2), Projectile.width - 6, Projectile.height - 6, 164, 0f, 0f, 30, default(Color), 1.2f);
                        Main.dust[dust].velocity *= Main.rand.NextFloat(1f, 4f);
                        Main.dust[dust].noGravity = true;
                    }
                    if (Projectile.direction == -1)
                    {
                        int dust = Dust.NewDust(new Vector2(Projectile.position.X + 4, Projectile.position.Y + 2), Projectile.width - 6, Projectile.height - 6, 164, 0f, 0f, 30, default(Color), 1.2f);
                        Main.dust[dust].velocity *= Main.rand.NextFloat(1f, 4f);
                        Main.dust[dust].noGravity = true;
                    }
                }

                for (int d = 0; d < 30; d++)
                {
                    if (Projectile.direction == 1)
                    {
                        int dust = Dust.NewDust(new Vector2(Projectile.position.X - 4, Projectile.position.Y + 2), Projectile.width - 6, Projectile.height - 6, 164, 0f, 0f, 30, default(Color), 1.2f);
                        Main.dust[dust].velocity *= Main.rand.NextFloat(0.5f, 3.5f);
                        Main.dust[dust].noGravity = false;
                    }
                    if (Projectile.direction == -1)
                    {
                        int dust = Dust.NewDust(new Vector2(Projectile.position.X + 4, Projectile.position.Y + 2), Projectile.width - 6, Projectile.height - 6, 164, 0f, 0f, 30, default(Color), 1.2f);
                        Main.dust[dust].velocity *= Main.rand.NextFloat(0.5f, 3.5f);
                        Main.dust[dust].noGravity = false;

                    }
                }
            }

            if (modPlayer.MiakodaEffectsTimer < 720)
            {
                player.GetModPlayer<tsorcRevampPlayer>().MiakodaCrescentDust2 = false;
            }
        }
    }
}
