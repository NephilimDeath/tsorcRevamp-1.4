using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Accessories {
    public class SpikedNecklace : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Thorns Effect.");

        }

        public override void SetDefaults() {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 22;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Blue;//for the soul cost
            Item.value = PriceByRarity.Blue_1; 
            Item.defense = 1;
        }

        public override void AddRecipes() {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(ItemID.IronBar, 2);
            recipe.AddIngredient(Mod.GetItem("DarkSoul"), 2000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override void UpdateEquip(Player player) {
            player.thorns += 1f;
        }
    }
}
