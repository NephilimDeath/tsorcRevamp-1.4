using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Summon.Whips
{
	public class NightsCracker : ModItem
	{
		public const int BaseDamage = 50;
		public static float MinSummonTagDamage = 2; //this doesn't affect anything
        public static float MaxSummonTagDamage = 8; //this doesn't affect anything
		public static float MinSummonTagCrit = 1;//this doesn't affect anything
        public static float MaxSummonTagCrit = 4;//this doesn't affect anything
		public static float MinSummonTagAttackSpeed = 6;//this doesn't affect anything
        public static float MaxSummonTagAttackSpeed = 24;//this doesn't affect anything
        public static float SearingLashEfficiency = 50;//this doesn't affect anything
        public static float CritDamage = 33;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MinSummonTagDamage, MaxSummonTagDamage, MinSummonTagCrit, MaxSummonTagCrit, MinSummonTagAttackSpeed, MaxSummonTagAttackSpeed, SearingLashEfficiency, CritDamage);
        public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;  //journey mode lmao
		}

		public override void SetDefaults()
		{
			Item.height = 39;
			Item.width = 46;

			Item.DamageType = DamageClass.SummonMeleeSpeed;
			Item.damage = BaseDamage;
			Item.knockBack = 2.5f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.buyPrice(0, 30, 0, 0);

			Item.shoot = ModContent.ProjectileType<Projectiles.Summon.Whips.NightsCrackerProjectile>();
			Item.shootSpeed = 4;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
		}
		public override bool MeleePrefix()
		{
			return true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Dominatrix>());
			recipe.AddIngredient(ItemID.ThornWhip);
			recipe.AddIngredient(ItemID.BoneWhip);
			recipe.AddIngredient(ModContent.ItemType<SearingLash>());
			recipe.AddIngredient(ItemID.SoulofNight, 7);
			recipe.AddIngredient(ItemID.SoulofFright, 20);
			recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 26000);

			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}