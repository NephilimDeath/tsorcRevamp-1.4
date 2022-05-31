﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientDemonArmor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Forged by those who brave Annihilation.\nMana Regen Skill activates when health falls below 160 \nSet bonus: +15% Magic Critical Chance, -15% Mana Usage, +15% Magic Damage");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 13;
            Item.value = 40000;
            Item.rare = ItemRarityID.Orange;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.10f;
        }

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(new Vector2((float)drawPlayer.position.X, (float)drawPlayer.position.Y), drawPlayer.width, drawPlayer.height, 6, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 200, Color.Yellow, 1.0f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLight = false;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(ItemID.MoltenBreastplate, 1);
            recipe.AddIngredient(Mod.GetItem("DarkSoul"), 3000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
