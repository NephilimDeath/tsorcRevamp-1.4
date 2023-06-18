using tsorcRevamp.Projectiles.Ranged.Runeterra;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using tsorcRevamp.Buffs.Runeterra.Ranged;
using tsorcRevamp.Items.Materials;
using Terraria.DataStructures;
using Terraria.Audio;

namespace tsorcRevamp.Items.Weapons.Ranged.Runeterra
{
    public class ToxicShot : ModItem
    {
        public int ShootSoundStyle = 0;
        public float ShootSoundVolume = 0.5f;
        public const int BaseDamage = 20;
        public const int ScoutsBoostOnHitCooldown = 3;
        public const int ScoutsBoost2Duration = 5;
        public const int ScoutsBoost2Cooldown = 25;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsRangedSpecialistWeapon[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 24;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item63;
            Item.DamageType = DamageClass.Ranged; 
            Item.damage = BaseDamage;
            Item.knockBack = 4f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.Seed;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Dart;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(+7f, -9f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ShootSoundStyle == 0)
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Ranged/ToxicShot/Shot1") with { Volume = ShootSoundVolume }, player.Center);
                ShootSoundStyle += 1;
            }
            else
            if (ShootSoundStyle == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Ranged/ToxicShot/Shot2") with { Volume = ShootSoundVolume }, player.Center);
                ShootSoundStyle += 1;
            }
            else
            if (ShootSoundStyle == 2)
            {
                SoundEngine.PlaySound(new SoundStyle("tsorcRevamp/Sounds/Runeterra/Ranged/ToxicShot/Shot3") with { Volume = ShootSoundVolume }, player.Center);
                ShootSoundStyle = 0;
            }
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.Seed)
            {
                type = ModContent.ProjectileType<ToxicShotDart>();
            }
            if (type == ProjectileID.PoisonDartBlowgun)
            {
                damage *= 2;
            }
        }
        public override void HoldItem(Player player)
        {
            if (!player.HasBuff(ModContent.BuffType<ScoutsBoostCooldown>()) && !player.HasBuff(ModContent.BuffType<ScoutsBoost2>()))
            {
                player.AddBuff(ModContent.BuffType<ScoutsBoost>(), 1);
            }
            if (player.itemAnimation > 14 && player.HasBuff(ModContent.BuffType<ScoutsBoost>())) //Scouts Boots 2 blocks Scouts Boost 1 and its cooldown so this won't occur then
            {
                player.velocity *= 0.92f;
            }
            else if (player.itemAnimation > 14 && player.HasBuff(ModContent.BuffType<ScoutsBoostCooldown>()))
            {
                player.velocity *= 0.01f;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ItemID.Blowpipe);
            recipe.AddIngredient(ModContent.ItemType<WorldRune>());
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 2000);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }

    }
}