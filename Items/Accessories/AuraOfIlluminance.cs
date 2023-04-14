﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Accessories
{
    public class AuraOfIlluminance : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Aura of Illuminance");
            // Tooltip.SetDefault("Creates a glowing trail behind you");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.value = PriceByRarity.Green_2;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateEquip(Player player)
        {
            if (player.whoAmI != Main.myPlayer)
            {
                return;
            }

            //If no projectile exists for this player, spawn it
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if(Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<Projectiles.AuraOfIlluminance>() && Main.projectile[i].ai[0] == player.whoAmI)
                {
                    return;
                }
            }

            Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.AuraOfIlluminance>(), 50, 0, Main.myPlayer, player.whoAmI);
        }
    }
}