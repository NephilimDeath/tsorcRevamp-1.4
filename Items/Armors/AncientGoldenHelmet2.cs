﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Armors {
    [AutoloadEquip(EquipType.Head)]
    public class AncientGoldenHelmet2 : ModItem {
        public override string Texture => "tsorcRevamp/Items/Armors/AncientGoldenHelmet";
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ancient Golden Helmet II");
            Tooltip.SetDefault("It is the famous Helmet of the Stars. \n9% melee speed\nSet bonus boosts all critical hits by 8%, +9% melee and ranged damage, +60 mana");
        }

        public override void SetDefaults() {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 5;
            Item.value = 15000;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateEquip(Player player) {
            player.meleeSpeed += 0.09f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return body.type == ModContent.ItemType<AncientGoldenArmor>() && legs.type == ModContent.ItemType<AncientGoldenGreaves>();
        }

        public override void UpdateArmorSet(Player player) {
            player.meleeDamage += 0.09f;
            player.rangedDamage += 0.09f;
            player.statManaMax2 += 60;
            player.rangedCrit += 8;
            player.magicCrit += 8;
            player.meleeCrit += 8;
            player.thrownCrit += 8; //lol
        }

        public override void AddRecipes() {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(ModContent.ItemType<AncientGoldenHelmet>());
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 500);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
