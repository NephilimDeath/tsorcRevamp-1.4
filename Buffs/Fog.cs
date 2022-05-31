﻿using Terraria;
using Terraria.ModLoader;

namespace tsorcRevamp.Buffs {
    class Fog : ModBuff {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fog");
            Description.SetDefault("Defense is increased by 8!");
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.statDefense += 8;
            Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), player.velocity.X, player.velocity.Y, Mod.Find<ModProjectile>("Fog").Type, 0, 0f, player.whoAmI, 0f, 0f);
        }
    }
}