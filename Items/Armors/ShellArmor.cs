﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Armors {
    [AutoloadEquip(EquipType.Body)]
    class ShellArmor : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Armor made from the shell of a legendary creature. \n20% chance not to consume ammo");
        }

        public override void SetDefaults() {
            Item.defense = 7;
            Item.rare = ItemRarityID.LightRed;
            Item.width = 18;
            Item.height = 18;
            Item.value = 30000;
        }

        public override void AddRecipes() {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(ItemID.NecroBreastplate);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 3000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override void UpdateEquip(Player player) {
            player.ammoCost80 = true;
        }
    }
}
