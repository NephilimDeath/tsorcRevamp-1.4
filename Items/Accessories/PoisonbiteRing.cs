using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace tsorcRevamp.Items.Accessories {
    public class PoisonbiteRing : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("One of the infamous bite rings commissioned by Sir Arstor of Carim." +
								"\nDespite the dreadful rumors surrounding its creation, this ring is an unmistakable asset," +
                                "\ndue to its ability to prevent becoming poisoned.\n+3 defense");
        }
 
        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.value = PriceByRarity.Blue_1;
            Item.rare = ItemRarityID.Blue;
        }
 
        public override void AddRecipes() {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(Mod.GetItem("SilverRing"), 1);
			recipe.AddIngredient(Mod.GetItem("BloodredMossClump"), 20);
            recipe.AddIngredient(Mod.GetItem("DarkSoul"), 2000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
 
        public override void UpdateEquip(Player player) {
			player.buffImmune[BuffID.Poisoned] = true;
            player.statDefense += 3;
        }
 
    }
}

