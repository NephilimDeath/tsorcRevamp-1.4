﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace tsorcRevamp.Items
{
    class Humanity : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.rare = ItemRarityID.Green;
            Item.value = 75000;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item4;
            Item.maxStack = 500;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return (player.GetModPlayer<tsorcRevampPlayer>().cursePoints > 0);
        }

        public override bool? UseItem(Player player)
        {
            int restore = player.GetModPlayer<tsorcRevampPlayer>().cursePoints;
            player.GetModPlayer<tsorcRevampPlayer>().cursePoints = 0;
            if (Main.myPlayer == player.whoAmI)
            {
                player.HealEffect(restore, true);
            }
            return true;
        }


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;

            //only insert the tooltip if the last valid line is not the name, the "Equipped in social slot" line, or the "No stats will be gained" line (aka do not insert if in a vanity slot)
            int ttindex = tooltips.FindLastIndex(t => t.Mod == "Terraria" && t.Name != "ItemName" && t.Name != "Social" && t.Name != "SocialDesc" && !t.Name.Contains("Prefix"));
            if (ttindex != -1)
            {// if we find one
             //insert the extra tooltip line
                int totalLife = player.statLifeMax + player.GetModPlayer<tsorcRevampPlayer>().cursePoints;
                tooltips.Insert(ttindex + 1, new TooltipLine(Mod, "Record", Language.GetTextValue("Mods.tsorcRevamp.Items.PurgingStone.Record") + totalLife));
            }
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color lightColor, Microsoft.Xna.Framework.Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {

            Texture2D texture = (Texture2D)Terraria.GameContent.TextureAssets.Item[Item.type];
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(0, 4), Item.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
