using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Accessories {
    public class WolfRing : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("One of the rings worn by Artorias." +
                                "\nImmunity to the on-fire and broken-armor debuffs." + 
								"\n+23 defense within the Abyss, +16 defense otherwise." + 
								"\nSwords inflict fire damage.");
        }

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.defense = 16;
			Item.lifeRegen = 8;
            Item.accessory = true;
            Item.value = PriceByRarity.Red_10;
            Item.rare = ItemRarityID.Red;
        }


        public override void UpdateEquip(Player player) {
            player.AddBuff(BuffID.WeaponImbueFire, 60, false);
			player.buffImmune[BuffID.OnFire] = true;
			player.buffImmune[BuffID.BrokenArmor] = true;
			
			if(Main.bloodMoon) { // Apparently this is the flag used in the Abyss?
				player.statDefense += 7;
			}
        }

    }
}