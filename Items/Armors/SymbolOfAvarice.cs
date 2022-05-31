﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]

    public class SymbolOfAvarice : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases Dark soul absorption from defeated enemies by 40%"
                                + "\nbut the curse of the branded also [c/FF0000:drains HP].");
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.defense = 2;
            Item.value = 3000000;
            Item.rare = ItemRarityID.Lime;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<tsorcRevampPlayer>().SOADrain = true;
        }
    }
}
