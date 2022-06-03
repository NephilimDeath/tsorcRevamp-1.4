﻿using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class AncientDemonShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Powerful, but reduces non-melee damage by 30%" +
                               "\nGreat Shield that reduces damage taken by 8%, grants immunity to knockback and gives thorns buff" +
                               "\nAlso provides immunity to fire blocks");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 38;
            Item.defense = 10;
            Item.accessory = true;
            Item.value = PriceByRarity.Orange_3;
            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateEquip(Player player)
        {
            player.noKnockback = true;
            player.GetDamage(DamageClass.Ranged) -= 0.3f;
            player.GetDamage(DamageClass.Magic) -= 0.3f;
            player.GetDamage(DamageClass.Summon) -= 0.3f;
            player.thorns = 1f;
            player.fireWalk = true;
            player.endurance += 0.08f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //only insert the tooltip if the last valid line is not the name, the "Equipped in social slot" line, or the "No stats will be gained" line (aka do not insert if in a vanity slot)
            int ttindex = tooltips.FindLastIndex(t => t.Mod == "Terraria" && t.Name != "ItemName" && t.Name != "Social" && t.Name != "SocialDesc" && !t.Name.Contains("Prefix"));
            if (ttindex != -1)
            {// if we find one
                //insert the extra tooltip line
                tooltips.Insert(ttindex + 1, new TooltipLine(Mod, "RevampShieldDR", "Reduces damage taken by 8%"));
            }
        }

        public override void AddRecipes()
        {
            Terraria.Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ObsidianShield);
            recipe.AddIngredient(ModContent.ItemType<Accessories.SpikedIronShield>());
            recipe.AddIngredient(ModContent.ItemType<Items.DarkSoul>(), 10000);
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }
    }
}
