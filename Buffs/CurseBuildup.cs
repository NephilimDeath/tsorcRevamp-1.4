﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Buffs {
    class CurseBuildup : ModBuff {

        public override void SetDefaults() {
            DisplayName.SetDefault("Curse Buildup");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare) {
            tip = "When the counter reaches 100, something bad happens. Curse buildup is at " + Main.LocalPlayer.GetModPlayer<tsorcRevampPlayer>().CurseLevel;
        }
        public override void Update(Player player, ref int buffIndex) {
            if ((100 <= player.GetModPlayer<tsorcRevampPlayer>().CurseLevel) && player.statLifeMax > 100) { //if curse is 100 or above, and the player has max hp to lose
                player.statLifeMax -= 20;
                Main.NewText("You have been cursed! -20 HP!");
                player.GetModPlayer<tsorcRevampPlayer>().CurseLevel = 0; //Reset it to 0
                Main.NewText("You feel invincible!");
                player.AddBuff(ModContent.BuffType<Invincible>(), 480, false); // 8 seconds
                player.AddBuff(ModContent.BuffType<Strength>(), 3600, false);

                for (int i = 0; i < 30; i++)
                {
                    int dust2 = Dust.NewDust(new Vector2((float)player.position.X, (float)player.position.Y), player.width, player.height, 21, Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2), 200, Color.Pink, Main.rand.NextFloat(2f, 5f));
                    Main.dust[dust2].noGravity = true;
                }
            }
        }

        public override bool ReApply(Player player, int time, int buffIndex) {
            player.GetModPlayer<tsorcRevampPlayer>().CurseLevel += Main.rand.Next(22, 36); //22-35, aka 3-4 hits before curse proc (projectiles now also inflict a bit of buildup; +5 in the case of bio spit from basalisks)
            
            for (int i = 0; i < 10; i++)
            {
                int dust2 = Dust.NewDust(new Vector2((float)player.position.X, (float)player.position.Y), player.width, player.height, 21, 0, 0, 200, Color.Pink, Main.rand.NextFloat(2f, 3f));
                Main.dust[dust2].noGravity = true;
            }

            return true;
        }
    }
}
