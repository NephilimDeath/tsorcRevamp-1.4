﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items;

namespace tsorcRevamp.NPCs.Bosses.WyvernMage
{
    [AutoloadBossHead]
    class MechaDragonHead : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wyvern Mage Disciple");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = 6;
            NPC.netAlways = true;
            NPC.npcSlots = 1;
            NPC.width = 45;
            NPC.height = 45;
            NPC.timeLeft = 22750;
            NPC.damage = 90;
            NPC.defense = 10;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath10;
            NPC.lifeMax = 91000;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.behindTiles = true;
            NPC.boss = true;
            NPC.value = 25000;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.buffImmune[BuffID.CursedInferno] = true;
            despawnHandler = new NPCDespawnHandler(DustID.OrangeTorch);

            bodyTypes = new int[] { ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonLegs>(), ModContent.NPCType<MechaDragonBody>(),
                ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonLegs>(), ModContent.NPCType<MechaDragonBody>(),
                ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonLegs>(), ModContent.NPCType<MechaDragonBody>(),
                ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonLegs>(), ModContent.NPCType<MechaDragonBody>(),
                ModContent.NPCType<MechaDragonBody2>(), ModContent.NPCType<MechaDragonBody3>() };

        }
        public static int[] bodyTypes;

        /**public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return head ? (bool?)null : false;
		}**/

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
        }

        NPCDespawnHandler despawnHandler;
        public override void AI()
        {
            despawnHandler.TargetAndDespawn(NPC.whoAmI);

            //Generic Worm Part Code:
            NPC.behindTiles = true;
            tsorcRevampGlobalNPC.AIWorm(NPC, ModContent.NPCType<MechaDragonHead>(), bodyTypes, ModContent.NPCType<MechaDragonTail>(), 23, -1f, 12f, 0.13f, true, false, true, false, false); //3f was 6f

            //Code unique to this body part:
            Color color = new Color();
            int dust = Dust.NewDust(new Vector2((float)NPC.position.X, (float)NPC.position.Y), NPC.width, NPC.height, 62, 0, 0, 100, color, 1.0f);
            Main.dust[dust].noGravity = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 origin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
            Color alpha = Color.White;
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - (float)TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + (float)NPC.height - (float)TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / (float)Main.npcFrameCount[NPC.type] + 4f + origin.Y * NPC.scale + 56f), NPC.frame, alpha, NPC.rotation, origin, NPC.scale, effects, 0f);
            NPC.alpha = 255;
            return true;
        }

        private static int ClosestSegment(NPC head, params int[] segmentIDs)
        {
            List<int> segmentIDList = new List<int>(segmentIDs);
            Vector2 targetPos = Main.player[head.target].Center;
            int closestSegment = head.whoAmI; //head is default, updates later
            float minDist = 1000000f; //arbitrarily large, updates later
            for (int i = 0; i < Main.npc.Length; i++)
            { //iterate through every NPC
                NPC npc = Main.npc[i];
                if (npc != null && npc.active && segmentIDList.Contains(npc.type))
                { //if the npc is part of the wyvern
                    float targetDist = (npc.Center - targetPos).Length();
                    if (targetDist < minDist)
                    { //if we're closer than the previously closer segment (or closer than 1,000,000 if it's the first iteration, so always)
                        minDist = targetDist; //update minDist. future iterations will compare against the updated value
                        closestSegment = i; //and set closestSegment to the whoAmI of the closest segment
                    }
                }
            }
            return closestSegment; //the whoAmI of the closest segment
        }

        public override bool PreKill()
        {
            int closestSegmentID = ClosestSegment(NPC, ModContent.NPCType<MechaDragonBody>(), ModContent.NPCType<MechaDragonBody2>(), ModContent.NPCType<MechaDragonBody3>(), ModContent.NPCType<MechaDragonLegs>(), ModContent.NPCType<MechaDragonTail>());
            NPC.position = Main.npc[closestSegmentID].position; //teleport the head to the location of the closest segment before running npcloot

            return true;        
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void OnKill()
        {

            //Kind of like EoW, it always drops this many extra souls whether it's been killed or not.
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<DarkSoul>(), 900);
            if (!Main.expertMode)
            {
                if (!(tsorcRevampWorld.Slain.ContainsKey(ModContent.NPCType<MechaDragonHead>())))
                { //If the boss has not yet been killed
                    Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<DarkSoul>(), 5000); //Then drop the souls
                }
            }
        }
    }
}
