using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Magic {
    public class Bolt1Tome : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bolt 1 Tome");
            Tooltip.SetDefault("A lost beginner's tome" +
                                "\nDrops a small lightning strike upon collision" +
                                "\nHas a chance to electrify enemies" +
                                "\nCan be upgraded");

        }

        public override void SetDefaults() {
            Item.damage = 12;
            Item.height = 10;
            Item.knockBack = 0f;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 6f;
            Item.magic = true;
            Item.noMelee = true;
            Item.mana = 9;
            Item.useAnimation = 27;
            Item.UseSound = SoundID.Item21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 27;
            Item.value = PriceByRarity.Blue_1;
            Item.width = 34;
            Item.shoot = ModContent.ProjectileType<Projectiles.Bolt1Ball>();
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 speed, int type, int damage, float knockBack)
        {
            if (player.wet)
            {
                player.AddBuff(BuffID.Electrified, 90);
            }

            return true;
        }

        public override void AddRecipes() {
            Recipe recipe = new Recipe(Mod);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(Mod.GetItem("DarkSoul"), 4000);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }


    }
}
