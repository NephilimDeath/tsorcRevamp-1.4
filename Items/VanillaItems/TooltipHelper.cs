﻿using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.VanillaItems
{
    class TooltipHelper : GlobalItem
    {

        //this method adds potentially multiple tooltip lines to the end of an item's tooltip stack 
        public static void SimpleModTooltip(Mod mod, Item item, List<TooltipLine> tooltips, int ItemID, string TipToAdd1, string TipToAdd2 = null)
        {
            if (item.type == ItemID)
            {
                int ttindex = tooltips.FindLastIndex(t => t.mod == "Terraria" && t.Name != "ItemName" && t.Name != "Social" && t.Name != "SocialDesc" && !t.Name.Contains("Prefix"));
                if (ttindex != -1)
                {// if we find one
                    //insert the extra tooltip line
                    tooltips.Insert(ttindex + 1, new TooltipLine(mod, "", TipToAdd1));
                    if (TipToAdd2 != null)
                    {
                        tooltips.Insert(ttindex + 2, new TooltipLine(mod, "", TipToAdd2));
                    }
                }
            }
        }

        public static void SimpleGlobalModTooltip(Mod mod, List<TooltipLine> tooltips, string TipToAdd1, string TipToAdd2 = null) //Same but not linked to a specific item.
        {
            int ttindex = tooltips.FindLastIndex(t => t.mod == "Terraria"); //find the last tooltip line
            if (ttindex != -1)
            {// if we find one
             //insert the extra tooltip line
                tooltips.Insert(ttindex + 1, new TooltipLine(mod, "", TipToAdd1));
                if (TipToAdd2 != null)
                {
                    tooltips.Insert(ttindex + 2, new TooltipLine(mod, "", TipToAdd2));
                }
            }

        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            //SimpleModTooltip(mod, item, tooltips, ItemID., "a");
            //SimpleModTooltip(mod, item, tooltips, ItemID., "a", "b");
            //SimpleModTooltip(mod, item, tooltips, ItemID.FlaskofFire, "Adds 10% melee damage");  don't do this. flask of fire's tooltip goes at a specific index, not the end
            SimpleModTooltip(Mod, item, tooltips, ItemID.AdamantiteBreastplate, "Set can be upgraded in 3 ways, with 4000 Dark Souls for each piece");
            SimpleModTooltip(Mod, item, tooltips, ItemID.AdamantiteDrill, "Use this to open the Adamantite gates in the", "Corruption Temple to the west of the village");
            SimpleModTooltip(Mod, item, tooltips, ItemID.AngelWings, "You will discover these in time...", "Can be upgraded with 1 Supersonic Boots, 1 Cloud in a Balloon, and 20000 Dark Souls");
            SimpleModTooltip(Mod, item, tooltips, ItemID.BandofRegeneration, "Can be upgraded with the Band of Starpower and 4000 Dark Souls");
            SimpleModTooltip(Mod, item, tooltips, ItemID.BandofStarpower, "Can be upgraded with the Band of Regeneration and 4000 Dark Souls");
            SimpleModTooltip(Mod, item, tooltips, ItemID.CobaltBreastplate, "Set can be upgraded with 9000 Dark Souls");
            SimpleModTooltip(Mod, item, tooltips, ItemID.CobaltDrill, "Use this to gain entry to the Wyvern Mage's", "Fortress above the hallowed caverns to the east");
            SimpleModTooltip(Mod, item, tooltips, ItemID.DivingHelmet, "Can be placed in an accessory slot or in your head slot.");
            SimpleModTooltip(Mod, item, tooltips, ItemID.GoldHelmet, "Can be upgraded with Dark Souls");
            SimpleModTooltip(Mod, item, tooltips, ItemID.GrapplingHook, "Don't buy or craft this until you have discovered it in the jungle");
            SimpleModTooltip(Mod, item, tooltips, ItemID.HerosHat, "Can be upgraded eventually with the flippers", "and diving helmet, when you acquire them");
            SimpleModTooltip(Mod, item, tooltips, ItemID.ManaCrystal, "Can be used with 200 Dark Souls to create a Mana Bomb");
            SimpleModTooltip(Mod, item, tooltips, ItemID.MechanicalEye, "Item is non-consumable.");
            SimpleModTooltip(Mod, item, tooltips, ItemID.MechanicalWorm, "It's heavier than you expected.\nYou get the feeling a way to stay in the air may be key...\nItem is non-consumable.");
            SimpleModTooltip(Mod, item, tooltips, ItemID.MeteorSuit, "Can be augmented with Souls of Light");
            SimpleModTooltip(Mod, item, tooltips, ItemID.MoltenFury, "Can be upgraded with 1 Soul of Sight and 70,000 Dark Souls");
            SimpleModTooltip(Mod, item, tooltips, ItemID.MoltenPickaxe, "Use this to open the Cobalt Gate to the east of", "the jungle ruins after you defeat the Wall of Flesh");
            SimpleModTooltip(Mod, item, tooltips, ItemID.MythrilChainmail, "This armor set can be upgraded with 9000 Dark Souls");
            SimpleModTooltip(Mod, item, tooltips, ItemID.ShadowHelmet, "Can be upgraded with 1000 Dark Souls");
            SimpleModTooltip(Mod, item, tooltips, ItemID.SilverWatch, "Can be upgraded with Dark Souls to change day to night.");
            SimpleModTooltip(Mod, item, tooltips, ItemID.StickyBomb, "More fun to use than a pickaxe!");
            SimpleModTooltip(Mod, item, tooltips, ItemID.WireCutter, "Do not use this!");
            SimpleModTooltip(Mod, item, tooltips, ItemID.WormFood, "Item is not consumed so you can retry the fight until victory");
            SimpleModTooltip(Mod, item, tooltips, ItemID.Wrench, "Do not use this!");
            SimpleModTooltip(Mod, item, tooltips, ItemID.BlueWrench, "Do not use this!");
            SimpleModTooltip(Mod, item, tooltips, ItemID.GreenWrench, "Do not use this!");
            SimpleModTooltip(Mod, item, tooltips, ItemID.YellowWrench, "Do not use this!");
            SimpleModTooltip(Mod, item, tooltips, ItemID.MulticolorWrench, "Do not use this!");
            SimpleModTooltip(Mod, item, tooltips, ItemID.CopperAxe, "All axes do 2x damage to woody enemies");
            SimpleModTooltip(Mod, item, tooltips, ItemID.DemonBow, "Can be upgraded with 4000 Dark Souls and 10 Shadow Scales");
            SimpleModTooltip(Mod, item, tooltips, ItemID.Diamond, "Vital ingredient in the crafting of a very powerful potion");
            SimpleModTooltip(Mod, item, tooltips, ItemID.IronOre, "Perhaps you can use these for making special arrows..?");
            SimpleModTooltip(Mod, item, tooltips, ItemID.FeralClaws, "Can be upgraded with 3000 Dark Souls, an Aglet and an Anklet of the Wind");
            SimpleModTooltip(Mod, item, tooltips, ItemID.Revolver, "Can be upgraded with 6000 Dark Souls and 10 Souls of Light or Dark");


            Player player = Main.LocalPlayer;

            if (player.GetModPlayer<tsorcRevampPlayer>().BearerOfTheCurse && player.whoAmI == Main.myPlayer && item.healLife > 0)
            {
                SimpleGlobalModTooltip(Mod, tooltips, "Doesn't heal the [c/6d8827:Bearer of the Curse]");
            }

            if (player.GetModPlayer<tsorcRevampPlayer>().BearerOfTheCurse && player.whoAmI == Main.myPlayer)
            {
                SimpleModTooltip(Mod, item, tooltips, ItemID.ShinePotion, "Has no effect on the [c/6d8827:Bearer of the Curse]");
            }

        }
    }
}
