﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items.Weapons.Summon.Runeterra;
using tsorcRevamp.NPCs;

namespace tsorcRevamp.Projectiles.Summon.Runeterra.Dragons
{
    public class BodySegment
    {
        public Vector2 offset;

        private float Scale;
        public static float BaseScale;

        BodySegment parent;
        public List<BodySegment> frontSegments = new List<BodySegment>();
        public List<BodySegment> backSegments = new List<BodySegment>();

        public int frame = 0;
        public int totalFrames;
        public int targetFrame = -1;
        public int frameUpdateTimer = 0;
        public static int timePrFrameUpdate = 10;
        public static int altTimePrFrameUpdate = 10;

        public int dir = 0;

        int startFrame;
        int endFrame;
        int altStartFrame;
        int altEndFrame;

        public SpriteEffects curEffect;

        public Asset<Texture2D> Texture = null;
        public float rotation;
        public float highSpeedRotaiton;
        public Vector2 segmentOrigin;

        public Vector2 zeroRotOffset;

        public Vector2 finalPosition;
        public float finalRotation;

        public BodySegment(int totalFrames, int startFrame, int loopStartFrame, int loopEndFrame, int loopAltStartFrame = -1, int loopAltEndFrame = -1)
        {
            this.totalFrames = totalFrames;
            this.frame = startFrame;

            this.startFrame = loopStartFrame;
            this.endFrame = loopEndFrame;
            this.altStartFrame = loopAltStartFrame;
            this.altEndFrame = loopAltEndFrame;

            this.Scale = BaseScale;
        }

        public void AddSegment(BodySegment seg, bool behind)
        {
            seg.parent = this;
            if (!behind)
                frontSegments.Add(seg);
            else
                backSegments.Add(seg);
        }

        public bool Update(int frameWorth, Vector2 origin, float usedRotations, SpriteEffects effect, bool altAnimation = false)
        {
            bool finishedAltSeq = false;

            finalRotation = usedRotations;
            finalPosition = origin;

            curEffect = effect;

            int maxFrame = (altAnimation ? (altEndFrame == -1 ? endFrame : altEndFrame) : endFrame);
            int minFrame = (altAnimation ? (altStartFrame == -1 ? startFrame : altStartFrame) : startFrame);

            int trueTimePrFrameUpdate = altAnimation ? (altEndFrame == -1 ? timePrFrameUpdate : altTimePrFrameUpdate) : timePrFrameUpdate;

            if (!(maxFrame == -1 || minFrame == -1))
            {
                if (frame < minFrame)
                {
                    frame = minFrame;
                    frameUpdateTimer = 0;
                }

                frameUpdateTimer += frameWorth;

                if (frameUpdateTimer > trueTimePrFrameUpdate)
                {
                    frameUpdateTimer = 0;

                    frame++;

                    if (frame > maxFrame)
                    {
                        frame = minFrame;

                        if (altAnimation && altEndFrame != -1)
                            finishedAltSeq = true;
                    }
                }
            }

            foreach (BodySegment backSegment in backSegments)
            {
                Vector2 offsetAdd = backSegment.offset;

                if (curEffect == SpriteEffects.FlipHorizontally)
                    offsetAdd.X *= -1;

                offsetAdd = offsetAdd.RotatedBy(finalRotation) * Scale;

                if (backSegment.Update(frameWorth, finalPosition + offsetAdd, finalRotation + backSegment.rotation, curEffect, altAnimation))
                    finishedAltSeq = true;
            }

            foreach (BodySegment frontSegment in frontSegments)
            {
                Vector2 offsetAdd = frontSegment.offset;

                if (curEffect == SpriteEffects.FlipHorizontally)
                    offsetAdd.X *= -1;

                offsetAdd = offsetAdd.RotatedBy(finalRotation) * Scale;

                if (frontSegment.Update(frameWorth, finalPosition + offsetAdd, finalRotation + frontSegment.rotation, curEffect, altAnimation))
                    finishedAltSeq = true;
            }

            return finishedAltSeq;
        }

        public void Draw(Color lightColor)
        {
            if (Texture == null)
                return;

            Rectangle drawRec = new Rectangle(0, Texture.Height() / totalFrames * frame, Texture.Width(), Texture.Height() / totalFrames);

            Vector2 orig = segmentOrigin;
            dir = 1;
            if (curEffect == SpriteEffects.FlipHorizontally)
            {
                dir = -1;
                orig.X = Texture.Width() - orig.X;
            }

            foreach (BodySegment backSegment in backSegments)
                backSegment.Draw(lightColor);

            Main.EntitySpriteDraw(Texture.Value, finalPosition - Main.screenPosition, drawRec, lightColor, finalRotation, orig, Scale, curEffect, 0);

            foreach (BodySegment frontSegment in frontSegments)
                frontSegment.Draw(lightColor);
        }
    }

    public abstract class RuneterraDragon : ModProjectile
    {
        public static Effect effect;

        public BodySegment Mouth;
        public BodySegment Head;
        public BodySegment BackBody;
        public BodySegment BackLLeg;
        public BodySegment BackRLeg;
        public BodySegment FrontBody;
        public BodySegment FrontLLeg;
        public BodySegment FrontRLeg;

        private bool AltSequence = false;
        private bool SyncAltSequence = false;

        public float maxBreathSize = 2700;
        public float breathSize = 2700;

        public BodySegment[] NeckSegments;
        public abstract float Scale { get; }
        public abstract int PairedProjectileType { get; }
        public abstract int BuffType { get; }
        public abstract int DebuffType { get; }
        public abstract int DragonType { get; }

        public float BaseOriginalDamage;
        public int BaseAttackSpeed = 15;
        public abstract void SetupBody();

        public override void AutoStaticDefaults()
        {
            TextureAssets.Projectile[Projectile.type] = ModContent.Request<Texture2D>(Texture + "/Dragon_Head");
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.DontAttachHideToAlpha[Type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.SummonTagDamageMultiplier[Projectile.type] = ScorchingPoint.DragonSummonTagDmgMult / 100f;
        }
        SoundStyle BreathLoopStyle;
        SlotId BreathLoopID;
        public override void SetDefaults()
        {
            // Projectile.width = 40; Projectile.height = 40;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;

            Projectile.minion = true;
            Projectile.minionSlots = 0.5f;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ContinuouslyUpdateDamageStats = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = BaseAttackSpeed;

            BodySegment.BaseScale = Scale;

            switch (DragonType)
            {
                case 1:
                    {
                        BreathLoopStyle = new SoundStyle("tsorcRevamp/Sounds/Runeterra/Summon/ScorchingPoint/BreathLoop") with { Volume = ScorchingPoint.SoundVolume * 0.75f, IsLooped = true, MaxInstances = 1 };
                        break;
                    }
                case 2:
                    {
                        BreathLoopStyle = new SoundStyle("tsorcRevamp/Sounds/Runeterra/Summon/InterstellarVessel/BreathLoop") with { Volume = InterstellarVesselGauntlet.SoundVolume, IsLooped = true, MaxInstances = 1 };
                        break;
                    }
                case 3:
                    {
                        BreathLoopStyle = new SoundStyle("tsorcRevamp/Sounds/Runeterra/Summon/CenterOfTheUniverse/BreathLoop") with { Volume = CenterOfTheUniverse.SoundVolume * 0.35f, IsLooped = true, MaxInstances = 1 };
                        break;
                    }
            }

            SetupBody();
        }
        public override void OnSpawn(IEntitySource source)
        {
            BaseOriginalDamage = Projectile.originalDamage;
        }
        public override void OnKill(int timeLeft)
        {
            if (SoundEngine.TryGetActiveSound(BreathLoopID, out var ActiveSound))
            {
                ActiveSound.Stop();
            }
        }

        public abstract void AltSequenceEnd();
        public void StartAltSequence()
        {
            AltSequence = true;
            SyncAltSequence = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.GetGlobalNPC<tsorcRevampGlobalNPC>().lastHitPlayerSummoner = player;
            target.AddBuff(DebuffType, ScorchingPoint.DragonDebuffDuration * 60);
            if (Main.rand.NextBool((int)(100f / ScorchingPoint.MarkChance)))
            {
                switch (DragonType)
                {
                    case 1:
                        {
                            if (target.GetGlobalNPC<tsorcRevampGlobalNPC>().ScorchMarks < 6)
                            {
                                target.GetGlobalNPC<tsorcRevampGlobalNPC>().ScorchMarks++;
                                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Summon/ScorchingPoint/Marked") with { Volume = ScorchingPoint.SoundVolume * 0.5f });
                            }
                            break;
                        }
                    case 2:
                        {
                            if (target.GetGlobalNPC<tsorcRevampGlobalNPC>().ShockMarks < 6)
                            {
                                target.GetGlobalNPC<tsorcRevampGlobalNPC>().ShockMarks++;
                                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Summon/InterstellarVessel/Marked") with { Volume = InterstellarVesselGauntlet.SoundVolume * 0.5f });
                            }
                            break;
                        }
                    case 3:
                        {
                            if (target.GetGlobalNPC<tsorcRevampGlobalNPC>().SunburnMarks < 6)
                            {
                                target.GetGlobalNPC<tsorcRevampGlobalNPC>().SunburnMarks++;
                                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Summon/CenterOfTheUniverse/Marked") with { Volume = CenterOfTheUniverse.SoundVolume * 0.5f });
                            }
                            break;
                        }
                }
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.HasBuff(BuffType))
            {
                Projectile.timeLeft = 2;
            }
            Projectile.originalDamage = (int)(BaseOriginalDamage / 19f * (float)player.ownedProjectileCounts[PairedProjectileType]);
            if (Projectile.damage < 1)
            {
                Projectile.damage = 1;
            }
            if (player.GetModPlayer<tsorcRevampPlayer>().InterstellarBoost)
            {
                Projectile.localNPCHitCooldown = BaseAttackSpeed - (BaseAttackSpeed / 3);
            }
            else { Projectile.localNPCHitCooldown = BaseAttackSpeed; }

            Vector2 movementVec = Main.MouseWorld - Projectile.Center;

            float length = movementVec.Length();

            switch (length)
            {
                case float gear1 when (gear1 < 25f):
                    {
                        Projectile.velocity += (Vector2.Normalize(movementVec) * MathF.Pow((length), 1f / 2f)) * 0.01f;
                        break;
                    }
                case float gear2 when (gear2 < 100f && gear2 >= 25f):
                    {
                        Projectile.velocity += (Vector2.Normalize(movementVec) * MathF.Pow((length), 1f / 2f)) * 0.08f;
                        break;
                    }
                case float gear3 when (gear3 >= 100f):
                    {
                        Projectile.velocity += (Vector2.Normalize(movementVec) * MathF.Pow((length), 1f / 2f)) * 0.15f;
                        break;
                    }
            }

            Projectile.velocity *= 0.84f;

            if (Projectile.velocity.Length() < 0.2f)
                Projectile.velocity *= 0.01f;

            Projectile.rotation = movementVec.ToRotation() - (Projectile.velocity.X > 0f ? 0f : MathF.PI);

            NPC targetMob = GetTargetWithinXDegree(Main.player[Projectile.owner], 300f, out bool keepCharge, out bool flip);

            int dir = 1;
            if (Projectile.velocity.X < 0)
                dir = -1;

            if (flip)
            {
                dir *= -1;
            }

            if (AltSequence)
            {
                targetMob = null;
                Projectile.velocity *= 0.01f;
                Projectile.rotation = 0f;
                dir = FrontBody.dir;
            }

            int baseHeadFrames = (DragonType == 3 ? 0 : 4);

            float totalRotationTarget = 0f;
            if (targetMob != null)
            {
                totalRotationTarget = Projectile.rotation - (NeckSegments[0].finalPosition - targetMob.Center).ToRotation();

                if (dir == 1)
                {
                    totalRotationTarget -= MathF.PI;
                }


                while (totalRotationTarget < -MathF.PI)
                {
                    totalRotationTarget += MathF.PI * 2f;
                }

                Head.frameUpdateTimer++;
                if (Head.frameUpdateTimer > 20) // head update timer
                {
                    Head.frameUpdateTimer = 0;

                    Head.frame++;

                    if (Head.frame > baseHeadFrames + 7)
                        Head.frame = baseHeadFrames + 4;
                }
                if (Head.frame >= baseHeadFrames + 4 && !SoundEngine.TryGetActiveSound(BreathLoopID, out var ActiveSound))
                {
                    BreathLoopID = SoundEngine.PlaySound(BreathLoopStyle);
                }
                else if (Head.frame < baseHeadFrames + 4 && SoundEngine.TryGetActiveSound(BreathLoopID, out var activeSound))
                {
                    activeSound.Pause();
                }
            }
            else if (keepCharge)
            {
                if (SoundEngine.TryGetActiveSound(BreathLoopID, out var ActiveSound))
                {
                    ActiveSound.Pause();
                }
                if (Head.frame > baseHeadFrames + 3)
                    Head.frame = baseHeadFrames + 3;

                if (Head.frame < baseHeadFrames + 1)
                    Head.frame = baseHeadFrames + 1;

                Head.frameUpdateTimer++;

                if (Head.frameUpdateTimer > 20) // head update timer
                {
                    Head.frameUpdateTimer = 0;

                    Head.frame++;

                    if (Head.frame > baseHeadFrames + 3)
                        Head.frame = baseHeadFrames + 3;
                }
            }
            else if (!keepCharge)
            {
                if (SoundEngine.TryGetActiveSound(BreathLoopID, out var ActiveSound))
                {
                    ActiveSound.Pause();
                }
                Head.frameUpdateTimer++;

                if (Head.frameUpdateTimer > 10) // head update timer
                {
                    Head.frameUpdateTimer = 0;

                    Head.frame--;

                    if (Head.frame < baseHeadFrames)
                        Head.frame = baseHeadFrames;
                }
            }

            Mouth.rotation = Math.Abs(Mouth.rotation) * dir;

            if (targetMob != null)
                totalRotationTarget += Mouth.rotation;

            float segmentRotation = -totalRotationTarget / (NeckSegments.Length - 1);
            bool flipped = dir < 0;

            // clear rotation;
            foreach (BodySegment NS in NeckSegments)
                NS.rotation = 0f;
            Head.rotation = 0f;

            FrontBody.Update(0, Projectile.Center, Projectile.rotation, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, AltSequence);
            Mouth.zeroRotOffset = Mouth.finalPosition; // save zeroRotOffset
            // set rotation;
            foreach (BodySegment NS in NeckSegments)
            {
                if (totalRotationTarget == 0)
                    NS.rotation *= 0.8f;
                else
                    NS.rotation = segmentRotation;
            }

            Head.rotation = -segmentRotation;

            if (FrontBody.Update(1, Projectile.Center, Projectile.rotation, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, AltSequence))
            {
                // end alt seq
                AltSequence = false;
                AltSequenceEnd();
            }
        }

        public NPC GetTargetWithinXDegree(Player owner, float degree, out bool keepCharge, out bool flip)
        {
            keepCharge = false;
            flip = false;

            float targetRRange = (0.5f - (degree / 360)) * 2f;
            float MaxDist = (320f * size / maxSize) * 1.4f;
            float MinDist = 150f * Scale;

            // This code is required if your minion weapon has the targeting feature
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                Vector2 between = npc.Center - Mouth.zeroRotOffset;

                float distBetween = between.Length();
                float rotationDiff = Vector2.Dot(Vector2.Normalize(between), Vector2.Normalize(Projectile.velocity).RotatedBy(Mouth.rotation));

                bool canHit = rotationDiff > targetRRange;

                // Reasonable distance away so it doesn't target across multiple screens
                if (MinDist < distBetween && distBetween < MaxDist)
                {
                    if (canHit)
                        flip = false;
                    else
                        flip = true;

                    return npc;
                }
            }

            float closestDist = MaxDist;
            NPC foundNPC = null;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.CanBeChasedBy() || npc.type == 488)
                {
                    Vector2 between = npc.Center - Projectile.Center; // 

                    float distBetween = between.Length();
                    bool closest = closestDist > distBetween;

                    float rotationDiff = Vector2.Dot(Vector2.Normalize(between), Vector2.Normalize(Projectile.velocity).RotatedBy(Mouth.rotation));

                    bool canHit = rotationDiff > targetRRange;

                    if (closest && (MinDist < distBetween && distBetween < MaxDist))
                    {
                        keepCharge = true;
                        closestDist = distBetween;

                        if (canHit)
                            flip = false;
                        else
                            flip = true;

                        foundNPC = npc;
                    }


                    if (distBetween < MaxDist * 1.7f)
                    {
                        keepCharge = true;
                    }
                }
            }

            return foundNPC;
        }

        public abstract float maxSize { get; }
        public abstract float size { get; }

        public override bool? CanDamage()
        {
            return true;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            int HeadFrameForBreathing = 4;
            switch (DragonType)
            {
                default: break;
                case 1:
                    {
                        HeadFrameForBreathing = 8;
                        break;
                    }
                case 2:
                    {
                        HeadFrameForBreathing = 8;
                        break;
                    }
            }
            if (Head.frame < HeadFrameForBreathing)
                return false;

            Vector2 between = Mouth.finalPosition - targetHitbox.Center();

            float angle = MathHelper.TwoPi / 10f;
            float shaderRotation = MathF.PI * 0.9f;
            float degree = (shaderRotation + angle - MathHelper.Pi - 0.2f) / MathF.PI;

            float ShootRotation = (Head.dir < 0 ? MathF.PI + Mouth.finalRotation : Mouth.finalRotation);

            float targetRRange = (0.5f - degree) * 2f;
            float rotationDiff = Vector2.Dot(Vector2.Normalize(between), -ShootRotation.ToRotationVector2());
            bool canHit = rotationDiff > targetRRange;

            //Console.WriteLine(rotationDiff + ": " + Vector2.Normalize(between) + " x " + Head.finalRotation.ToRotationVector2());

            float betweenDistance = between.Length();
            if (betweenDistance < 320f * size / maxSize && canHit)
            {
                return true;
            }

            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(SyncAltSequence);
            SyncAltSequence = false;

            // size?
            // maby other things..?
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            if (reader.ReadBoolean())
                AltSequence = true;
        }
    }
}