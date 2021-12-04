using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace tsorcRevamp {

    //using static tsorcRevamp.SpawnHelper;
    public static class SpawnHelper {

        //undergroundJungle, undergroundEvil, and undergroundHoly are deliberately missing. call Cavern && p.zone instead.

        public static bool Cavern(Player p) { //if youre calling Cavern without a p.zone check, also call NoSpecialBiome
            return p.position.Y >= Main.rockLayer && p.position.Y <= Main.rockLayer * 25;
        }

        public static bool NoSpecialBiome(Player p) {
            return (!p.ZoneJungle && !p.ZoneCorrupt && !p.ZoneCrimson && !p.ZoneHoly && !p.ZoneMeteor && !p.ZoneDungeon);
        }

        public static bool Sky(Player p) { //p.ZoneSkyHeight is more restrictive than this, so use this if an enemy uses it
            return p.position.Y < Main.worldSurface * 0.44999998807907104;
        }

        public static bool Surface(Player p) {
            return !Sky(p) && (p.position.Y < Main.worldSurface); //dont need to check nospecialbiome here since we're already calling Sky
        }

        public static bool Underground(Player p) {
            return Main.worldSurface > p.position.Y && p.position.Y < Main.rockLayer;
        } 

        public static bool Underworld(Player p) {
            int playerYTile = (int)(p.Bottom.Y + 8f) / 16;
            return playerYTile > Main.maxTilesY - 190;
        }
    }

    //using static tsorcRevamp.oSpawnHelper;
    public static class oSpawnHelper { 

        public static bool oCavern(Player p) {
            return (p.position.Y >= (Main.rockLayer * 17)) && (p.position.Y < (Main.rockLayer * 24));
        }

        public static bool oCavernByTile(Player p) {
            int playerYTile = (int)(p.Bottom.Y + 8f) / 16;
            return playerYTile >= (Main.maxTilesY * 0.4f) && playerYTile < (Main.maxTilesY * 0.6f);
        }

        public static bool oMagmaCavern(Player p) {
            return (p.position.Y >= (Main.rockLayer * 24)) && (p.position.Y < (Main.rockLayer * 32));
        }

        public static bool oMagmaCavernByTile(Player p) {
            int playerYTile = (int)(p.Bottom.Y + 8f) / 16;
            return playerYTile >= (Main.maxTilesY * 0.6f) && playerYTile < (Main.maxTilesY * 0.8f);
        }

        public static bool oSky(Player p) {
            return p.position.Y <= Main.rockLayer * 4;
        }

        public static bool oSkyByTile(Player p) {
            int playerYTile = (int)(p.Bottom.Y + 8f) / 16;
            return playerYTile < (Main.maxTilesY * 0.1f);
        }

        public static bool oSurface(Player p) {
            return !p.ZoneSkyHeight && (p.position.Y <= Main.worldSurface);
        }

        public static bool oUnderground(Player p) {
            return (p.position.Y >= (Main.rockLayer * 13)) && (p.position.Y < (Main.rockLayer * 17));
        }

        public static bool oUndergroundByTile(Player p) {
            int playerYTile = (int)(p.Bottom.Y + 8f) / 16;
            return playerYTile >= (Main.maxTilesY * 0.3f) && playerYTile < (Main.maxTilesY * 0.4f);
        }

        public static bool oUnderSurface(Player p) {
            return (p.position.Y > (Main.rockLayer * 8)) && (p.position.Y < (Main.rockLayer * 13));
        }

        public static bool oUnderSurfaceByTile(Player p) {
            int playerYTile = (int)(p.Bottom.Y + 8f) / 16;
            return (playerYTile >= (Main.maxTilesY * 0.2f) && playerYTile < (Main.maxTilesY * 0.3f));
        }

        public static bool oUnderworld(Player p) {
            return p.position.Y >= (Main.rockLayer * 32);
        }

        public static bool oUnderworldByHeight(Player p) {
            int playerYTile = (int)(p.Bottom.Y + 8f) / 16;
            return (playerYTile >= (Main.maxTilesY * 0.8f));
        }
    }

    public static class VariousConstants {
        public const int CUSTOM_MAP_WORLD_ID = 44874972;
        public const string MUSIC_MOD_URL = "https://github.com/timhjersted/tsorcDownload/raw/main/tsorcMusic.tmod";
        public const string MAP_URL = "https://github.com/timhjersted/tsorcDownload/raw/main/the-story-of-red-cloud.wld";
        public const string CHANGELOG_URL = "https://raw.githubusercontent.com/timhjersted/tsorcDownload/main/changelog.txt";
    }

    public static class PriceByRarity { 
        
        //minimal exploration. pre-hardmode ores. likely no items that craft from souls will use this
        public static readonly int White_0 = Item.buyPrice(platinum: 0, gold: 0, silver: 40, copper: 0);

        //underground chest loots (shoe spikes, CiaB, etc), shadow orb items, floating island
        public static readonly int Blue_1 = Item.buyPrice(platinum: 0, gold: 2, silver: 25, copper: 0);

        //gold dungeon chest (handgun, cobalt shield, etc), goblin invasion
        public static readonly int Green_2 = Item.buyPrice(platinum: 0, gold: 4, silver: 50, copper: 0);

        //hell, underground jungle
        public static readonly int Orange_3 = Item.buyPrice(platinum: 0, gold: 7, silver: 50, copper: 0);

        //early hardmode (hm ores), mimics
        public static readonly int LightRed_4 = Item.buyPrice(platinum: 0, gold: 12, silver: 50, copper: 0);

        //hallowed tier. post mech, pre plantera. pirates.
        public static readonly int Pink_5 = Item.buyPrice(platinum: 0, gold: 25, silver: 50, copper: 0);

        //some biome mimic gear, high level tinkerer combinations (ankh charm, mechanical glove). seldom used in vanilla
        public static readonly int LightPurple_6 = Item.buyPrice(platinum: 0, gold: 37, silver: 50, copper: 0);

        //plantera, golem, chlorophyte
        public static readonly int Lime_7 = Item.buyPrice(platinum: 0, gold: 47, silver: 50, copper: 0);
        
        //post-plantera dungeon, martian madness, pumpkin/frost moon
        public static readonly int Yellow_8 = Item.buyPrice(platinum: 0, gold: 55, silver: 0, copper: 0);
        
        //lunar fragments, dev armor. seldom used in vanilla
        public static readonly int Cyan_9 = Item.buyPrice(platinum: 0, gold: 67, silver: 50, copper: 0);
        
        //luminite, lunar fragment gear, moon lord drops
        public static readonly int Red_10 = Item.buyPrice(platinum: 0, gold: 80, silver: 0, copper: 0);
        
        //no vanilla items have purple rarity base. only cyan and red with modifiers can be purple. im guessing on the price.
        public static readonly int Purple_11 = Item.buyPrice(platinum: 1, gold: 20, silver: 0, copper: 0);



    }

    public static class UsefulFunctions
    {
        ///<summary> 
        ///Returns a vector pointing from a source, to a target, with a speed.
        ///Simplifies all projectile, enemy dash, etc aiming calculations to a single call.
        ///</summary>         
        ///<param name="source">The start point of the vector</param>
        ///<param name="target">The end point it is aiming towards</param>
        ///<param name="speed">The length of the resulting vector</param>
        public static Vector2 GenerateTargetingVector(Vector2 source, Vector2 target, float speed)
        {
            Vector2 diff = target - source;
            float angle = diff.ToRotation();
            Vector2 velocity = new Vector2(speed, 0);
            velocity = velocity.RotatedBy(angle);
            return velocity;
        }

        ///<summary> 
        ///Draws a projectile fully lit. Goes in PreDraw(), simply return false after calling it.
        ///</summary>         
        ///<param name="spriteBatch">The currently open SpriteBatch</param>
        ///<param name="projectile">The projectile to be drawn</param>
        ///<param name="texture">An empty static Texture2D variable that this function can use to cache the projectile's texture.</param>
        public static void DrawSimpleLitProjectile(SpriteBatch spriteBatch, Projectile projectile, ref Texture2D texture)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            if(texture == null)
            {
                texture = ModContent.GetTexture(projectile.modProjectile.Texture);
            }

            int frameHeight = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
            int startY = frameHeight * projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Main.spriteBatch.Draw(texture,
                projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY),
                sourceRectangle, Color.White, projectile.rotation, origin, projectile.scale, spriteEffects, 0f);
        }

        ///<summary> 
        ///Spawns a ring of dust around a point
        ///</summary>         
        ///<param name="center">Center of the dust ring</param>
        ///<param name="radius">Radius of the ring</param>
        ///<param name="dustID">ID of the dust to spawn</param>
        ///<param name="dustCount">How many to spawn per tick</param>
        ///<param name="dustSpeed">How fast dust should rotate</param>
        public static void DustRing(Vector2 center, float radius, int dustID, int dustCount = 5, float dustSpeed = 2)
        {
            for (int j = 0; j < dustCount; j++)
            {
                Vector2 dir = Main.rand.NextVector2CircularEdge(radius, radius);
                Vector2 dustPos = center + dir;
                Vector2 dustVel = new Vector2(dustSpeed, 0).RotatedBy(dir.ToRotation() + MathHelper.Pi / 2);
                Dust.NewDustPerfect(dustPos, dustID, dustVel, 200).noGravity = true;
            }
        }

        ///<summary> 
        ///Broadcasts a message from the server to all players. Safe to use in singleplayer, where it simply defaults to a NewText() instead.
        ///</summary>         
        ///<param name="text">String containing the text</param>
        public static void ServerText(string text)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Yellow);
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text);
            }
        }

        ///<summary> 
        ///Broadcasts a message from the server to all players. Safe to use in singleplayer, where it simply defaults to a NewText() instead.
        ///</summary>         
        ///<param name="text">String containing the text</param>
        ///<param name="color">Color of the text</param>
        public static void ServerText(string text, Color color)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            }
            if(Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text, color);
            }
        }

        
        ///<summary> 
        ///Gets the first npc of a given type. Basically NPC.AnyNPC, except it actually returns what it finds.
        ///Uses nullable ints, aka "int?". Will return null if it can't find one.
        ///</summary>         
        ///<param name="type">Type of NPC to look for</param>
        public static int? GetFirstNPC(int type)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == type)
                {
                    return i;
                }
            }
            
            return null;            
        }


        ///<summary> 
        ///Call in a projectile's AI to allow the projectile to home on enemies
        ///</summary>         
        ///<param name="projectile">The current projectile</param>
        ///<param name="homingRadius">The homing radius</param>
        ///<param name="topSpeed">The projectile's maximum velocity</param>
        ///<param name="rotateTowards">Should the projectile maintain topSpeed speed and rotate towards targets, instead of standard homing?</param>
        ///<param name="homingStrength">The homing strength coefficient. Unused if rotateTowards.</param>
        ///<param name="needsLineOfSight">Does the projectile need line of sight to home on a target?</param>
        public static void HomeOnEnemy(Projectile projectile, float homingRadius, float topSpeed, bool rotateTowards = false, float homingStrength = 1f, bool needsLineOfSight = false) {
            if (!projectile.active || !projectile.friendly) return;
            const int BASE_STRENGTH = 30;

            Vector2 targetLocation = Vector2.UnitY;
            bool foundTarget = false;

            for (int i = 0; i < 200; i++) {
                if (!Main.npc[i].active) continue;
                float toNPCEdge = (Main.npc[i].width / 2) + (Main.npc[i].height / 2); //make homing on larger targets more consistent

                //WithinRange is just faster Distance (skips sqrt)
                if (Main.npc[i].CanBeChasedBy(projectile) && projectile.WithinRange(Main.npc[i].Center, homingRadius + toNPCEdge) && (!needsLineOfSight || Collision.CanHitLine(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))) {
                    targetLocation = Main.npc[i].Center;
                    foundTarget = true;
                    break;
                }
            }

            if (foundTarget) {
                Vector2 homingDirection = Vector2.Normalize(targetLocation - projectile.Center);
                projectile.velocity = (projectile.velocity * (BASE_STRENGTH / homingStrength) + homingDirection * topSpeed) / ((BASE_STRENGTH / homingStrength) + 1);
            }
            if (rotateTowards) {
                if (projectile.velocity.Length() < topSpeed) {
                    projectile.velocity *= topSpeed / projectile.velocity.Length();
                }
            }
            if (projectile.velocity.Length() > topSpeed) {
                projectile.velocity *= topSpeed / projectile.velocity.Length();
            }
        }

        ///<summary> 
        ///Spawns a client-side instanced item similar to treasure bags. Safe to use in single-player, where it simply drops the item normally.
        ///</summary>         
        ///<param name="Position">Where the item should be spawned</param>
        ///<param name="HitboxSize">How big a hitbox it should have</param>
        ///<param name="itemType">The type of item</param>
        ///<param name="itemStack">How many of the item it should spawn</param>
        ///<param name="includeThesePlayers">If it should only drop for specific players, pass a list of them here</param>
        public static void NewItemInstanced(Vector2 Position, Vector2 HitboxSize, int itemType, int itemStack = 1, List<int> includeThesePlayers = null)
        {
            int dummyItemIndex = Item.NewItem(Position, HitboxSize, itemType, itemStack, true, 0, false, false);
            Main.itemLockoutTime[dummyItemIndex] = 54000;
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (includeThesePlayers != null)
                    {
                        if (includeThesePlayers.Contains(i))
                        {
                            NetMessage.SendData(MessageID.InstancedItem, i, number: dummyItemIndex);
                        }
                    }
                    else
                    {
                        NetMessage.SendData(MessageID.InstancedItem, i, number: dummyItemIndex);
                    }
                }
                Main.item[dummyItemIndex].active = false;
            }
        }
    }
}