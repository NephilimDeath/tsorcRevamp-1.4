using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Projectiles.Enemy
{
	class EnemySporeTrap : ModProjectile
	{
		int spriteType;

		public override void SetDefaults()
		{
			//projectile.aiStyle = ProjectileID.SporeTrap;
			
			Projectile.height = 8;
			
			Projectile.light = 1;
			
			//projectile.penetrate = 1; //was 8
			
			Projectile.CloneDefaults(ProjectileID.SporeTrap);
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = true;
			Projectile.width = 8;
			Projectile.timeLeft = 80;
			spriteType = Main.rand.Next(2);
			//aiType = ProjectileID.SporeTrap;
		}


		//Turn into the spore cloud by expanding and changing sprites
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			Projectile.width += 20;
			Projectile.height += 20;
			spriteType = Main.rand.Next(3, 5);

			if (tsorcRevampWorld.Slain.ContainsKey(NPCID.EaterofWorldsHead))
			{
				target.AddBuff(20, 600, false); //poisoned
			}

			if (tsorcRevampWorld.Slain.ContainsKey(NPCID.SkeletronHead))
			{
				//target.AddBuff(30, 150, false); //bleeding
				target.AddBuff(ModContent.BuffType<Buffs.CurseBuildup>(), 18000, false); //-20 HP after several hits
				target.GetModPlayer<tsorcRevampPlayer>().CurseLevel += 1;
			}
		}

		
		

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = null;
			if (spriteType == 0)
			{
				texture = Main.projectileTexture[ProjectileID.SporeTrap];
			}
			else if (spriteType == 1)
			{
				texture = Main.projectileTexture[ProjectileID.SporeTrap2];
			}
			else if (spriteType == 2)
			{
				texture = Main.projectileTexture[ProjectileID.SporeGas];
			}
			else if (spriteType == 3)
			{
				texture = Main.projectileTexture[ProjectileID.SporeGas2];
			}
			else if (spriteType == 4)
			{
				texture = Main.projectileTexture[ProjectileID.SporeGas3];
			}

			if (texture != null)
			{
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}

			return false;
		}

		/*public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			//Texture2D texture = Main.projectileTexture[ProjectileID.SporeTrap];
			Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			return false;
		}
		*/


		
	}
}

