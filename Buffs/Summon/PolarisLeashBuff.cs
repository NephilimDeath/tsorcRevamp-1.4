using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Items.Weapons.Summon.Whips;

namespace tsorcRevamp.Buffs.Summon
{
	public class PolarisLeashBuff : ModBuff
	{
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Polaris");
			// Description.SetDefault("Polaris will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                int WhipDamage = (int)player.GetTotalDamage(DamageClass.SummonMeleeSpeed).ApplyTo(PolarisLeash.BaseDamage * 0.66f);
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Whips.PolarisLeashPolaris>()] == 0)
                {
                    Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.One, ModContent.ProjectileType<Projectiles.Summon.Whips.PolarisLeashPolaris>(), WhipDamage, 1f, Main.myPlayer);
                }
            }
        }
    }
}
