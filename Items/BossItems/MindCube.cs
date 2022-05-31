﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.NPCs.Bosses.Okiku.FirstForm;
using tsorcRevamp.NPCs.Bosses.Okiku.SecondForm;
using tsorcRevamp.NPCs.Bosses.Okiku.ThirdForm;


namespace tsorcRevamp.Items.BossItems {
    class MindCube : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Summons Attraidies, the Mindflayer King \n" +
                "This is it. The final battle. \n" +
                "Item is not consumed on use. To find the light and dark shards needed to craft this in adventure mode... Seek the Dark Tower of Attraidies.");
        }

        public override void SetDefaults() {
            Item.rare = ItemRarityID.LightRed;
            Item.width = 38;
            Item.height = 34;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.maxStack = 1;
            Item.consumable = false;
        }


        public override bool? UseItem(Player player) {
            NPC.NewNPC((int)player.position.X, (int)player.position.Y - 64, ModContent.NPCType<DarkShogunMask>());
            Main.NewText("You are a fool, Red. You think you can defeat me?...", 175, 75, 255);;
            return true;
        }
        public override bool CanUseItem(Player player) {
            if (NPC.AnyNPCs(ModContent.NPCType<DarkShogunMask>())
                || NPC.AnyNPCs(ModContent.NPCType<DarkDragonMask>())
                || NPC.AnyNPCs(ModContent.NPCType<Okiku>())
                || NPC.AnyNPCs(ModContent.NPCType<BrokenOkiku>())
                ) {
                return false;
            }
            return true;
        }

        public override void AddRecipes() {
            Recipe recipe = new Recipe(Mod);

            if (ModContent.GetInstance<tsorcRevampConfig>().AdventureModeItems)
            {
                recipe.AddIngredient(ItemID.LightShard, 99);
                recipe.AddIngredient(ItemID.DarkShard, 99);
            }
            recipe.AddIngredient(Mod.GetItem("CrestOfFire"), 1);
            recipe.AddIngredient(Mod.GetItem("CrestOfWater"), 1);
            recipe.AddIngredient(Mod.GetItem("CrestOfEarth"), 1);
            recipe.AddIngredient(Mod.GetItem("CrestOfSky"), 1);
            recipe.AddIngredient(Mod.GetItem("CrestOfCorruption"), 1);
            recipe.AddIngredient(Mod.GetItem("CrestOfSteel"), 1);
            recipe.AddIngredient(Mod.GetItem("CrestOfLife"), 1);
            recipe.AddIngredient(Mod.GetItem("CrestOfStone"), 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
