using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {
    public class ForgottenPolearm : ModItem {

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Forgotten Polearm");
            Tooltip.SetDefault("Shimmering ephemeral energy.");
        }

        public override void SetDefaults() {
            Item.damage = 27;
            Item.knockBack = 3f;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 11;
            Item.useTime = 6;
            Item.shootSpeed = 10;
            //item.shoot = ProjectileID.DarkLance;
            
            Item.height = 50;
            Item.width = 50;

            Item.melee = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.value = PriceByRarity.Orange_3;
            Item.rare = ItemRarityID.Orange;
            Item.maxStack = 1;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<Projectiles.ForgottenPolearm>();
        }

        public override void AddRecipes() {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(ModContent.ItemType<EphemeralDust>(), 35);
            recipe.AddIngredient(Mod.GetItem("DarkSoul"), 6000);
            recipe.SetResult(this, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.AddRecipe();
        }
    }
}
