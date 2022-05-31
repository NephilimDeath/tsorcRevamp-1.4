﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tsorcRevamp.Projectiles.Enemy.Okiku;

namespace tsorcRevamp.NPCs.Bosses.Okiku.FinalForm {
    [AutoloadBossHead]
    class Attraidies : ModNPC {
        public override string Texture => "tsorcRevamp/NPCs/Bosses/Okiku/FirstForm/DarkShogunMask";
        public override void SetDefaults() {
            NPC.npcSlots = 10;
            NPC.damage = 70;
            NPC.defense = 25;
            NPC.height = 44;
            NPC.width = 28;
            NPC.timeLeft = 22500;
            music = 12;
            NPC.lifeMax = 80000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.boss = true;
            NPC.value = 500000;
            NPC.lavaImmune = true;
            NPC.knockBackResist = 0;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Confused] = true;
            Main.npcFrameCount[NPC.type] = 3;
            bossBag = ModContent.ItemType<Items.BossBags.AttraidiesBag>();
            despawnHandler = new NPCDespawnHandler("With your death, a dark shadow falls over the world...", Color.DarkMagenta, DustID.PurpleCrystalShard);            
        }

        public int ShadowOrbDamage = 80;
        public int CrystalShardsDamage = 90;
        public int DeathBallDamage = 120;
        public int BlackFireDamage = 80;
        public int StardustLaserDamage = 100;
        public int AntiMatterBlastDamage = 90;
        public int SolarDetonationDamage = 110;
        public int LightningStrikeDamage = 130;
        public int DarkLaserDamage = 100;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            ShadowOrbDamage = ShadowOrbDamage / 2;
            CrystalShardsDamage = CrystalShardsDamage / 2;
            DeathBallDamage = DeathBallDamage / 2;
            BlackFireDamage = BlackFireDamage / 2;
            StardustLaserDamage = StardustLaserDamage / 2;
            AntiMatterBlastDamage = AntiMatterBlastDamage / 2;
            SolarDetonationDamage = SolarDetonationDamage / 2;
            LightningStrikeDamage = LightningStrikeDamage / 2;
            DarkLaserDamage = DarkLaserDamage / 2;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if (NPC.AnyNPCs(ModContent.NPCType<Attraidies>())) {
                return 0;
            }

            float chance = 0;
            var p = spawnInfo.Player;
            bool nospecialbiome = !p.ZoneJungle && !p.ZoneCorrupt && !p.ZoneCrimson && !p.ZoneHallow && !p.ZoneMeteor && !p.ZoneDungeon;
            bool surface = nospecialbiome && !p.ZoneSkyHeight && (spawnInfo.SpawnTileY <= Main.worldSurface);

            if (Main.hardMode && Main.bloodMoon && !tsorcRevampWorld.SuperHardMode && surface) {
                chance = (1f / 38500f);
            }
            
            return chance;
        }

        public float AttackMode
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float AttackModeCounter
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float TeleportTimer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float DragonSpawned
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        

        public float ShadowShotCount = 0;
        public float CrystalShardsTimer = 0;
        public float NPCSummonCooldown = 0;
        public bool SetVelocity = false;
        public Vector2 NewVelocity = Vector2.Zero;
        int TravelDir = 0;
        Vector2 OrbitOffset = new Vector2(300, 0);
        float RotationProgress = 0;
        //The first entry is true here because he always starts with attack 0, and the rest are only set upon changing attacks
        bool[] UsedAttacks = new bool[7] { true, false, false, false, false, false, false };

        #region AI
        NPCDespawnHandler despawnHandler;
        public override void AI()
        {
           
            
            //if(Main.netMode == NetmodeID.Server)
            //{
            //     NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, this.npc.whoAmI);
            //}

            despawnHandler.TargetAndDespawn(NPC.whoAmI);
            TeleportTimer++;
            //npc.ai[2] = 
            CrystalShardsTimer++; //Crystal Shards attack

            //if (Main.netMode != NetmodeID.Server && !Filters.Scene["tsorcRevamp:AttraidiesShader"].IsActive())
            //{
            //    Filters.Scene.Activate("tsorcRevamp:AttraidiesShader");
            //}

            //Attacks:
            //Mode 0, Teleporting Shots: Teleports above the player, dropping down on them while firing an unbreaking stream of Shadow Shots, casting Massive Crystal Shards, and rarely firing Sudden Death Balls. Classic.
            //Mode 1, Black Fire: Similar to his Phase 2 Obscure Drop attack, except the projectiles are Black Fireballs and there are (thankfully) fewer of them
            //Maybe the fireballs should fall slower, but there should be more of them? Gotta find a balance. A bit janky to dodge right now even with supersonic wings and dragoon armor movespeed boosts due to acceleration/deceleration still being super slow
            //Mode 2, Stardust Lasers: Spawns sets of 4 blue spheres, which then fire lasers at the player that persist and slowly move toward them over a few seconds
            //(Lerps the laser's target Vector2 with the player's right now, meaning tracking slows as it gets closer to give them more of a shot at escaping. If this needs to be made harder to dodge it could just move at them with a set velocity instead)
            //Maybe instead these should totally aim at unmoving spots randomly all around the player instead of homing on them?
            //Mode 3, Homing Spheres: Flies past the player firing a large array of homing projectiles.
            //Maybe he should rotate around the player in a quarter-circle instead if this needs to be made harder? Maybe just if he's low on health
            //Mode 4, Solar Eruptions: Rapidly spawns floating solar charges all over the battlefield, which detonate a few seconds later scattering projectiles and firing a laser in 5 directions
            //[TODO]Mode 5: Chain Lightning. Starts at Attraidies and zig-zag's across the screen, telegraphing its path a moment in advance.
            //Should maybe trap the player in a large ball of lightning too so they can't just run? Like an inverse of Mode 6. May have to use lasers, normal projectiles that big are really janky...
            //Should also probably be predictive
            //Mode 6: Fires dark lasers in 5 directions which rotate around him slowly, while he also spams Obscure Shot's in all directions.

            #region Dusts
            //Spawn dusts depending on his max life
            if (NPC.life > (NPC.lifeMax / 2)) {
                int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 6, NPC.velocity.X, NPC.velocity.Y, 200, Color.Red, 1f);
                Main.dust[dust].noGravity = true;
            }
            else
            {
                int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 54, NPC.velocity.X, NPC.velocity.Y, 140, Color.Red, 2f);
                Main.dust[dust].noGravity = true;
            }
            #endregion

            #region Movement
            if (AttackMode == 0)
            {
                Lighting.AddLight(NPC.position, Color.DarkOrange.ToVector3());
                if ((TeleportTimer >= 190 && NPC.life > 12000) || (TeleportTimer >= 110 && NPC.life <= 12000))
                {
                    AttraidiesTeleport();
                    NPC.noGravity = false;
                    TeleportTimer = 0;
                    CrystalShardsTimer = 0;
                    ShadowShotCount = 0;
                    AttackModeCounter++;
                    if (AttackModeCounter == 5)
                    {
                        ChangeAttackModes();
                    }
                }

                #region Slow down after teleporting
                //Shortly after teleporting, slow down to a stop.
                if (TeleportTimer >= 30)
                {
                    NPC.velocity.X *= 0.17f;
                    NPC.velocity.Y *= 0.17f;
                }
                #endregion
            }

            if (AttackMode == 1)
            {
                Lighting.AddLight(NPC.position, Color.Purple.ToVector3());
                if (!SetVelocity)
                {
                    NPC.noGravity = true;
                    NPC.noTileCollide = true;
                    if (Math.Abs((int)Main.player[NPC.target].position.X - (int)NPC.position.X) > 100)
                    {
                        if (NPC.Center.X - Main.player[NPC.target].Center.X < 0)
                        {
                            NPC.velocity = new Vector2(3, 0);
                        }
                        else
                        {
                            NPC.velocity = new Vector2(-3, 0);
                        }
                    }
                    NPC.velocity.Y = -0.75f;
                }

                AttackModeCounter++;
                if (AttackModeCounter > 600)
                {
                    ChangeAttackModes();
                }
            }

            if (AttackMode == 2)
            {
                Lighting.AddLight(NPC.position, Color.Cyan.ToVector3());
                NPC.noGravity = true;
                NPC.noTileCollide = true;

                //To prevent rapid diretion changes
                if (Math.Abs((int)Main.player[NPC.target].position.X - (int)NPC.position.X) < 100)
                {
                    if (Main.player[NPC.target].position.X > NPC.position.X)
                    {
                        NPC.velocity.X = 4;
                    }
                    else
                    {
                        NPC.velocity.X = -4;
                    }
                }

                if (Math.Abs((int)Main.player[NPC.target].position.Y - (int)NPC.position.Y) < 100)
                {
                    if (Main.player[NPC.target].position.Y > NPC.position.Y)
                    {
                        NPC.velocity.Y = 4;
                    }
                    else
                    {
                        NPC.velocity.Y = -4;
                    }
                }


                if (TeleportTimer >= 450)
                {
                    AttraidiesTeleport();
                    TeleportTimer = 0;
                    AttackModeCounter++;
                    if (AttackModeCounter == 3)
                    {
                        ChangeAttackModes();
                    }
                }
            }

            if (AttackMode == 3)
            {
                Lighting.AddLight(NPC.position, Color.HotPink.ToVector3());
                NPC.noGravity = true;
                NPC.noTileCollide = true;

                float speed = 9;

                if (TravelDir == 0)
                {
                    TravelDir = 1;
                    NPC.position = Main.player[NPC.target].position + new Vector2(-1000, -400);
                    AttackModeCounter = 0;
                }

                if (AttackModeCounter % 2 == 0)
                {
                    NPC.position.X += speed * TravelDir;
                    NPC.position.Y = Main.player[NPC.target].position.Y - 400;
                }
                NPC.velocity = Vector2.Zero;

                if (TeleportTimer >= 300)
                {
                    if (AttackModeCounter == 1)
                    {
                        TravelDir *= -1;
                        NPC.position = Main.player[NPC.target].position + new Vector2(-1000 * TravelDir, -400);
                    }
                    else
                    {
                        AttraidiesTeleport();
                    }
                    TeleportTimer = 0;
                    AttackModeCounter++;
                    if (AttackModeCounter == 4)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCKilled, (int)NPC.position.X, (int)NPC.position.Y, 43);
                        ChangeAttackModes();
                    }
                }
            }

            if (AttackMode == 4)
            {
                Lighting.AddLight(NPC.position, Color.OrangeRed.ToVector3());               
                NPC.noGravity = true;
                NPC.noTileCollide = true;
                NewVelocity = new Vector2(0, 2f).RotatedByRandom(MathHelper.TwoPi);
                SetVelocity = true;

                NPC.position = Main.player[NPC.target].position + OrbitOffset.RotatedBy(RotationProgress);
                RotationProgress += 0.01f; AttackModeCounter++;
                if (AttackModeCounter > 830)
                {
                    ChangeAttackModes();
                }
            }

            if (AttackMode == 5)
            {
                //Unfinished, just skip to the next one
                ChangeAttackModes();
            }

            if(AttackMode == 6)
            {
                NPC.noTileCollide = true;
                if (AttackModeCounter == 5)
                {
                    NPC.position.X = Main.player[NPC.target].position.X;
                    NPC.position.Y -= 800;

                    //Gives the player infinite flight for the duration of the attack. Sticks around for a bit afterward as a bonus.                
                    UsefulFunctions.BroadcastText("You suddenly feel weightless...", Color.DeepSkyBlue);
                    Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<Buffs.EarthAlignment>(), 1600);                    
                }

                NPC.velocity = Vector2.Zero;
                for (int i = 0; i < 10; i++)
                {
                    Vector2 dustOffset = Main.rand.NextVector2Circular(40, 40);
                    Vector2 dustPos = dustOffset + NPC.Center;
                    Dust.NewDustPerfect(dustPos, 54, Vector2.Zero, 250, Color.White, 2.0f).noGravity = true;
                }

                AttackModeCounter++;
                if (AttackModeCounter > 1200)
                {
                    ChangeAttackModes();
                }
                
            }
            #endregion            

            #region Attacks
            //These should only ever run on either a single player client or the multiplayer server!
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                #region Mode 0: Teleport and Fire Waves
                if (AttackMode == 0) {
                    #region Shadow Orb Attack
                    if (ShadowShotCount < 60 && (TeleportTimer % 2 == 0))
                    {
                        float num48 = 0.5f;
                        Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                        int type = ModContent.ProjectileType<ShadowOrb>();
                        float rotation = (float)Math.Atan2(vector8.Y - (Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)), vector8.X - (Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)));
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * num48) * -1), (float)((Math.Sin(rotation) * num48) * -1), type, ShadowOrbDamage, 0f, Main.myPlayer);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 20);
                        ShadowShotCount++;
                        NPC.netUpdate = true; //new
                    }
                    #endregion

                    #region Crystal Shard Attack
                    if (CrystalShardsTimer >= 45) //how often the crystal attack can happen in frames per second
                    {
                        float num48 = 8f;
                        Vector2 startPos = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y - 520 + (NPC.height / 2));
                        float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - startPos.X) + Main.rand.Next(-20, 20);
                        float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - startPos.Y) + Main.rand.Next(-20, 20);
                        float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                        num51 = num48 / num51;
                        speedX *= num51;
                        speedY *= num51;
                        int type = ModContent.ProjectileType<MassiveCrystalShardsSpell>();//44;//0x37; //14;
                        int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), startPos.X, startPos.Y, speedX, speedY, type, CrystalShardsDamage, 0f, Main.myPlayer);
                        Main.projectile[num54].timeLeft = 80;
                        Main.projectile[num54].aiStyle = 0;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 25);
                        //So he can only cast it once per-teleport
                        CrystalShardsTimer = -2000;
                    }
                    if (CrystalShardsTimer >= 0)
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 234, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 100, Color.White, 2.0f);
                            Main.dust[dust].noGravity = true;
                        }
                    }
                    #endregion Crystal Shard Attack

                    #region Sudden Death Ball Attack
                    if (Main.rand.Next(100) == 1)
                    {
                        float num48 = 10f;
                        Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height / 2));
                        float speedX = ((Main.player[NPC.target].position.X + (Main.player[NPC.target].width * 0.5f)) - vector8.X) + Main.rand.Next(-20, 0x15);
                        float speedY = ((Main.player[NPC.target].position.Y + (Main.player[NPC.target].height * 0.5f)) - vector8.Y) + Main.rand.Next(-20, 0x15);
                        if (((speedX < 0f) && (NPC.velocity.X < 0f)) || ((speedX > 0f) && (NPC.velocity.X > 0f)))
                        {
                            float num51 = (float)Math.Sqrt((double)((speedX * speedX) + (speedY * speedY)));
                            num51 = num48 / num51;
                            speedX *= num51;
                            speedY *= num51;
                            int type = ModContent.ProjectileType<EnemySuddenDeathBall>();//44;//0x37; //14;
                            int num54 = Projectile.NewProjectile(NPC.GetSource_FromThis(), vector8.X, vector8.Y, speedX, speedY, type, DeathBallDamage, 0f, Main.myPlayer);
                            Main.projectile[num54].timeLeft = 10;
                            Main.projectile[num54].aiStyle = 1;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 0x11);
                        }
                        NPC.netUpdate = true;
                    }
                    #endregion
                }
                #endregion

                #region Mode 1: Black Fire Rain
                if (AttackMode == 1)
                {
                    if (AttackModeCounter > 15 && (Main.GameUpdateCount % 45 == 0))
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            //The first projectile, which he fires into the sky in clumps and is mostly for visual effect (still does damage, though)
                            Vector2 position = NPC.position + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20));
                            Vector2 velocity = new Vector2(Main.rand.Next(-2, 2), -50);
                            Projectile blackFire = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<EnemyBlackFireVisual>(), BlackFireDamage, .5f, Main.myPlayer);
                            blackFire.timeLeft = 20;
                        }
                    }

                    if (AttackModeCounter > 75 && (Main.GameUpdateCount % 5 == 0))
                    {
                        //The second projectile, which comes raining down a second later and means business
                        Vector2 position = Main.player[NPC.target].position + new Vector2(Main.rand.Next(-1400, 1400), -700);
                        //Very similar to normal Black Fire, but phases through blocks until it reaches the player's height.
                        //Also the explosion doesn't do damage (for obvious reasons)
                        Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), position, new Vector2(0, 5), ModContent.ProjectileType<EnemyAttraidiesBlackFire>(), BlackFireDamage, .5f, Main.myPlayer, NPC.target);
                    }
                }
                #endregion

                #region Mode 2: Stardust Lasers
                if (AttackMode == 2)
                {
                    if (TeleportTimer % 90 == 0 && TeleportTimer < 300)
                    {
                        int speed = 35;
                        Vector2 vagueVelocity = new Vector2(Main.rand.Next(-speed, speed), Main.rand.Next(-speed, speed));
                        for (int i = 0; i < 4; i++)
                        {
                            //The first projectile, which he fires into the sky in clumps and is mostly for visual effect (still does damage, though)
                            Vector2 position = NPC.position;
                            Vector2 velocity = vagueVelocity + new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));
                            int firingDelay = (int)(329 - TeleportTimer);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<StardustShot>(), StardustLaserDamage, .5f, Main.myPlayer, NPC.target, firingDelay);
                        }
                    }
                }
                #endregion

                #region Mode 3: Homing Spheres
                if (AttackMode == 3)
                {
                    if (AttackModeCounter % 2 == 0)
                    {
                        if (TeleportTimer % 60 == 0)
                        {
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position, Vector2.Zero, ModContent.ProjectileType<PhasedMatterBlast>(), AntiMatterBlastDamage, .5f, Main.myPlayer).timeLeft = (int)((300 * (4 - AttackModeCounter)) - TeleportTimer);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position + new Vector2(10, 0), Vector2.Zero, ModContent.ProjectileType<PhasedMatterBlast>(), AntiMatterBlastDamage, .5f, Main.myPlayer).timeLeft = (int)((300 * (4 - AttackModeCounter)) - TeleportTimer);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position + new Vector2(-10, 0), Vector2.Zero, ModContent.ProjectileType<PhasedMatterBlast>(), AntiMatterBlastDamage, .5f, Main.myPlayer).timeLeft = (int)((300 * (4 - AttackModeCounter)) - TeleportTimer);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position + new Vector2(0, 10), Vector2.Zero, ModContent.ProjectileType<PhasedMatterBlast>(), AntiMatterBlastDamage, .5f, Main.myPlayer).timeLeft = (int)((300 * (4 - AttackModeCounter)) - TeleportTimer);
                            Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position + new Vector2(0, -10), Vector2.Zero, ModContent.ProjectileType<PhasedMatterBlast>(), AntiMatterBlastDamage, .5f, Main.myPlayer).timeLeft = (int)((300 * (4 - AttackModeCounter)) - TeleportTimer);
                        }
                    }
                    else
                    {
                        if (TeleportTimer == 90)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 8);
                                Vector2 velocity = Main.rand.NextVector2Circular(10, 10);
                                Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position, velocity, ModContent.ProjectileType<PhasedMatterBlast>(), AntiMatterBlastDamage, .5f, Main.myPlayer).timeLeft = (int)((300 * (4 - AttackModeCounter)) - TeleportTimer);
                                Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position + new Vector2(10, 0), velocity, ModContent.ProjectileType<PhasedMatterBlast>(), AntiMatterBlastDamage, .5f, Main.myPlayer).timeLeft = (int)((300 * (4 - AttackModeCounter)) - TeleportTimer);
                                Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position + new Vector2(-10, 0), velocity, ModContent.ProjectileType<PhasedMatterBlast>(), AntiMatterBlastDamage, .5f, Main.myPlayer).timeLeft = (int)((300 * (4 - AttackModeCounter)) - TeleportTimer);
                                Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position + new Vector2(0, 10), velocity, ModContent.ProjectileType<PhasedMatterBlast>(), AntiMatterBlastDamage, .5f, Main.myPlayer).timeLeft = (int)((300 * (4 - AttackModeCounter)) - TeleportTimer);
                                Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position + new Vector2(0, -10), velocity, ModContent.ProjectileType<PhasedMatterBlast>(), AntiMatterBlastDamage, .5f, Main.myPlayer).timeLeft = (int)((300 * (4 - AttackModeCounter)) - TeleportTimer);
                            }
                        }
                    }
                }
                #endregion

                #region Mode 4: Solar Detonators
                if (AttackMode == 4)
                {                    
                    if (AttackModeCounter % 45 == 0 && AttackModeCounter < 730)
                    {
                        Vector2 position = Main.player[NPC.target].position + Main.rand.NextVector2Square(-600, 600);
                        Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<SolarDetonator>(), SolarDetonationDamage, .5f, Main.myPlayer, NPC.target);
                    }                    
                }
                #endregion

                #region Mode 5: Chain Lightning Strikes
                if (AttackMode == 5)
                {

                }
                #endregion

                #region Mode 6: Dark Lasers
                if (AttackMode == 6)
                {
                    if (AttackModeCounter % 2 == 0 && AttackModeCounter > 120)
                    {
                        Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2CircularEdge(8, 8), ModContent.ProjectileType<ObscureShot>(), DarkLaserDamage, .5f, Main.myPlayer);
                    }
                    if (AttackModeCounter == 5)
                    {
                       Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<DarkLaserHost>(), DarkLaserDamage, .5f, Main.myPlayer, NPC.whoAmI);
                    }                   
                }
                #endregion

                #region NPC Spawning
                NPCSummonCooldown++; 
                if (NPCSummonCooldown >= 3600) //Can summon extra enemies once a minute
                {
                    if (NPC.life > (NPC.lifeMax / 2))
                    {


                        int SpawnSelection = Main.rand.Next(4);
                        if (SpawnSelection == 0)
                        {
                            NPC.NewNPC((int)Main.player[this.NPC.target].position.X - 406 - this.NPC.width / 2, (int)Main.player[this.NPC.target].position.Y - 16 - this.NPC.width / 2, NPCID.ChaosElemental, 0);
                            NPC.NewNPC((int)Main.player[this.NPC.target].position.X + 406 - this.NPC.width / 2, (int)Main.player[this.NPC.target].position.Y - 16 - this.NPC.width / 2, NPCID.ChaosElemental, 0);
                        }
                        if (SpawnSelection == 1)
                        {
                            NPC.NewNPC((int)Main.player[this.NPC.target].position.X + 800 - this.NPC.width / 2, (int)Main.player[this.NPC.target].position.Y - 500 - this.NPC.width / 2, ModContent.NPCType<Enemies.MindflayerIllusion>());
                        }
                        if (SpawnSelection == 2)
                        {
                            NPC.NewNPC((int)Main.player[this.NPC.target].position.X + 900 - this.NPC.width / 2, (int)Main.player[this.NPC.target].position.Y - 500 - this.NPC.width / 2, ModContent.NPCType<AttraidiesMimic>());
                        }
                        if (SpawnSelection == 3)
                        {
                            NPC.NewNPC((int)Main.player[this.NPC.target].position.X + 900 - this.NPC.width / 2, (int)Main.player[this.NPC.target].position.Y - 500 - this.NPC.width / 2, ModContent.NPCType<Enemies.DiscipleOfAttraidies>());
                        }
                        NPCSummonCooldown = 0;
                    }
                    else
                    {
                        #region Dragon Spawning
                        if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Okiku.SecondForm.ShadowDragonHead>()))
                        {
                            DragonSpawned = 1;
                        }
                        if (DragonSpawned == 0)
                        {
                            int OptionId = NPC.NewNPC((int)NPC.position.X + (NPC.width / 2), (int)NPC.position.Y + (NPC.height / 2), ModContent.NPCType<SecondForm.ShadowDragonHead>(), NPC.whoAmI);
                            Main.npc[OptionId].velocity.Y = -10;
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, OptionId, 0f, 0f, 0f, 0);
                            }

                            NPCSummonCooldown = 0;
                            DragonSpawned = 1;
                        }
                        #endregion
                    }                    
                }
                #endregion
            }
            #endregion
        }
        #endregion

        void AttraidiesTeleport()
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 8);
            for (int num36 = 0; num36 < 10; num36++)
            {
                int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 54, NPC.velocity.X + Main.rand.Next(-10, 10), NPC.velocity.Y + Main.rand.Next(-10, 10), 200, Color.Red, 4f);
                Main.dust[dust].noGravity = false;
            }

            float teleportAngle = (float)(Main.rand.Next(360) * (Math.PI / 180));

            Player Pt = Main.player[NPC.target];
            
            Vector2 PtC = Pt.position + new Vector2(Pt.width / 2, Pt.height / 2);
            NPC.position.X = Pt.position.X + (float)((600 * Math.Cos(teleportAngle)) * -1);
            NPC.position.Y = Pt.position.Y - 35 + (float)((30 * Math.Sin(teleportAngle)) * -1);

            float MinDIST = 560f;
            float MaxDIST = 610f;

            if(AttackMode == 2)
            {
                MinDIST = 700f;
                MaxDIST = 900f;
            }
            Vector2 Diff = NPC.position - Pt.position;
            if (Diff.Length() > MaxDIST)
            {
                Diff *= MaxDIST / Diff.Length();
            }
            if (Diff.Length() < MinDIST)
            {
                Diff *= MinDIST / Diff.Length();
            }
            NPC.position = Pt.position + Diff;

            Vector2 NC = NPC.position + new Vector2(NPC.width / 2, NPC.height / 2);

            float rotation = (float)Math.Atan2(NC.Y - PtC.Y, NC.X - PtC.X);
            NPC.velocity.X = (float)(Math.Cos(rotation) * 20) * -1;
            NPC.velocity.Y = (float)(Math.Sin(rotation) * 20) * -1;
            NPC.netUpdate = true;
        }

        void ChangeAttackModes()
        {
            AttraidiesTeleport();
            
            //Check if every attack has been used, and if so then clear the array
            for(int i = 0; i < 7; i++)
            {
                if (UsedAttacks[i] == false)
                {
                    break;
                }
                if (i == 6)
                {
                    UsedAttacks = new bool[7] { false, false, false, false, false, false, false };
                }
            }

            bool repeat;
            //Make sure the new attack chosen isn't a repeat
            do
            {
                repeat = false;
                AttackMode = Main.rand.Next(7);
                if (UsedAttacks[(int)AttackMode] == true)
                {
                    repeat = true;
                }                
            } while (repeat == true);

            UsedAttacks[(int)AttackMode] = true;

            //Reset all the various nonsense that his other patterns use or set
            DragonSpawned = 0;
            AttackModeCounter = 0;
            SetVelocity = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            TravelDir = 0;
            
            if(Main.netMode == NetmodeID.Server)
            {
                 NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, this.NPC.whoAmI);
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void OnKill() {


            Player player = Main.player[NPC.target];
            //if (Main.netMode != NetmodeID.Server && Filters.Scene["tsorcRevamp:AttraidiesShader"].IsActive())
            //{
            //    Filters.Scene["tsorcRevamp:TheAbyss"].Deactivate();
            //}

            for (int i = 0; i < 50; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(10, 10);
                int dust;
                dust = Dust.NewDust(NPC.Center, 30, 30, DustID.PurpleCrystalShard, vel.X, vel.Y, 100, default, 5f);
                Main.dust[dust].noGravity = true;
                vel = Main.rand.NextVector2Circular(10, 10);
                Dust.NewDust(NPC.Center, 30, 30, DustID.PurpleCrystalShard, vel.X, vel.Y, 240, default, 5f);
                Main.dust[dust].noGravity = true;
                vel = Main.rand.NextVector2Circular(20, 20);
                Dust.NewDust(NPC.Center, 30, 30, 234, vel.X, vel.Y, 240, default, 5f);
                Main.dust[dust].noGravity = true;
                Dust.NewDust(NPC.Center, 30, 30, DustID.Fire, vel.X, vel.Y, 200, default, 3f);
            }

            if (Main.expertMode)
            {
                NPC.DropBossBags();
            }
            else
            {
                Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.TheEnd>());
                Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.GuardianSoul>());
                Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.SoulOfAttraidies>(), Main.rand.Next(15, 23));
                Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.DarkSoul>(), 2000);
                Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Weapons.Magic.BloomShards>(), 1, false, -1);
                Item.NewItem(NPC.getRect(), ItemID.Picksaw);
                if (!tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<NPCs.Bosses.Okiku.FinalForm.Attraidies>()) && player.GetModPlayer<tsorcRevampPlayer>().BearerOfTheCurse) Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.EstusFlaskShard>());
            }

            if (!tsorcRevampWorld.SuperHardMode) {

                UsefulFunctions.BroadcastText("A portal from The Abyss has been opened!", new Color(255, 255, 60));
                UsefulFunctions.BroadcastText("Artorias, the Ancient Knight of the Abyss has entered this world!...", new Color(255, 255, 60));
                UsefulFunctions.BroadcastText("You must seek out the Shaman Elder...", new Color(255, 255, 60));

                Main.hardMode = true;
                tsorcRevampWorld.SuperHardMode = true;
                tsorcRevampWorld.TheEnd = false;
            }

            else {

                UsefulFunctions.BroadcastText("The portal from The Abyss remains open...", new Color(255, 255, 60));
                UsefulFunctions.BroadcastText("You must seek out the Shaman Elder...", new Color(255, 255, 60));                

                tsorcRevampWorld.SuperHardMode = true;
                Main.hardMode = true;
                tsorcRevampWorld.TheEnd = false;
            }
        }
    }
}
