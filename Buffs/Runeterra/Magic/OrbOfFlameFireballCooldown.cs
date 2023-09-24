﻿using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using tsorcRevamp.Items.Weapons.Magic.Runeterra;

namespace tsorcRevamp.Buffs.Runeterra.Magic
{
    public class OrbOfFlameFireballCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Magic/OrbOfFlame/FireballReady") with { Volume = OrbOfDeception.OrbSoundVolume * 2 });
            }
        }
    }
}
