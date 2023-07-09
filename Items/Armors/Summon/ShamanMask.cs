﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using tsorcRevamp.Buffs.Armor;
using tsorcRevamp.Items.Materials;

namespace tsorcRevamp.Items.Armors.Summon
{
    [AutoloadEquip(EquipType.Head)]
    public class ShamanMask : ModItem
    {
        public static float WhipDmg = 28f;
        public static float WhipRange = 30f;
        public static float SummonTagDuration = 35f;
        public static float AtkSpeed = 16f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(WhipDmg, WhipRange, AtkSpeed, ShunpoDash.Cooldown, SummonTagDuration);
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.defense = 11;
            Item.rare = ItemRarityID.Pink;
            Item.value = PriceByRarity.fromItem(Item);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.SummonMeleeSpeed) += WhipDmg;
            player.GetModPlayer<tsorcRevampPlayer>().SummonTagDuration += SummonTagDuration / 100f;
            player.whipRangeMultiplier += WhipRange / 100f;
            player.GetAttackSpeed(DamageClass.Summon) += AtkSpeed / 100f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ShamanCloak>() && legs.type == ModContent.ItemType<ShamanPants>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<tsorcRevampPlayer>().Shunpo = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TikiMask);
            recipe.AddIngredient(ItemID.AdamantiteBar);
            recipe.AddIngredient(ModContent.ItemType<DarkSoul>(), 9000);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.TikiMask);
            recipe2.AddIngredient(ItemID.TitaniumMask);
            recipe2.AddTile(TileID.DemonAltar);

            recipe2.Register();
        }
    }
}