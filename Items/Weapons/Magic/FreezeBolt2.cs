﻿using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Magic {
    class FreezeBolt2 : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Freeze Bolt II");
            Tooltip.SetDefault("Casts a fast-moving bolt of ice");
        }

        public override void SetDefaults() {
            Item.width = 24;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.maxStack = 1;
            Item.damage = 73;
            Item.knockBack = 5;
            Item.autoReuse = true;
            Item.scale = 1;
            Item.UseSound = SoundID.Item21;
            Item.rare = ItemRarityID.Pink;
            Item.shootSpeed = 9;
            Item.mana = 12;
            Item.value = PriceByRarity.Pink_5;
            Item.DamageType = DamageClass.Magic;
            Item.shoot = ModContent.ProjectileType<Projectiles.FreezeBolt>();
        }
        public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(Mod.Find<ModItem>("FreezeBolt").Type, 1);
            recipe.AddIngredient(ItemID.FallenStar, 30);
            recipe.AddIngredient(ItemID.SoulofMight, 1);
            recipe.AddIngredient(Mod.Find<ModItem>("DarkSoul").Type, 60000);
            recipe.AddTile(TileID.DemonAltar);
            
            recipe.Register();
        }
    }
}
