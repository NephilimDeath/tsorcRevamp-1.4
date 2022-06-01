﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class ArcherOfLumeliaShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Archer of Lumelia Shirt");
            Tooltip.SetDefault("Gifted with lethal archery abilities\n25% chance not to consume ammo.\nSet Bonus: +23% Ranged Crit, +15% Ranged Damage, Archery Skill (arrow speed & dmg +20%)");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 20;
            Item.value = 5000;
            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateEquip(Player player)
        {
            player.ammoCost75 = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AdamantiteBreastplate, 1);
            recipe.AddIngredient(Mod.Find<ModItem>("DarkSoul").Type, 4000);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }
    }
}
