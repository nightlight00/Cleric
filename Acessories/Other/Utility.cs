using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using clericclass.ClericBase;
using clericclass.Weapons.Templates;
using clericclass.Buffs.DoT;

namespace clericclass.Acessories.Other
{
    [AutoloadEquip(EquipType.Face)]
    class BrimstoneRose : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Reduces lava damage \nGrants immunity to fireblocks \nGrants immunity to 'On Fire!' and 'Brimstone Flames'");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 28;
            item.height = 38;
            item.rare = 6;
        }
        public override void UpdateEquip(Player player)
        {
            player.fireWalk = true;
            player.lavaRose = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[ModContent.BuffType<BrimstoneDebuff>()] = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ObsidianRose);
            recipe.AddIngredient(ModContent.ItemType<Materials.Brimstone>(), 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class BrimstoneCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Molten Fang Necklace");
            Tooltip.SetDefault("Grants complete immunity to lava \nIncreases armor penetration by 8");
        }

        public override void SetDefaults()
        {
            item.defense = 4;
            item.accessory = true;
            item.width = 40;
            item.height = 36;
            item.rare = 6;
        }
        public override void UpdateEquip(Player player)
        {
            player.lavaImmune = true;
            player.armorPenetration += 8;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LavaCharm);
            recipe.AddIngredient(ItemID.SharkToothNecklace);
            recipe.AddIngredient(ModContent.ItemType<Materials.Brimstone>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }


    public class BrimstoneLocket : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("9th Layer Locket");
            Tooltip.SetDefault("Turns the wearer into a Red Devil (dev note : visual is a wip)" +
                             "\n10% increased necrotic damage" +
                             "\nGrants complete immunity to lava and fire blocks" +
                             "\nGrants immunity to 'On Fire!' and 'Brimstone Flames'" + 
                             "\nIncreases armor penetration by 8");
        }

        public override void SetDefaults()
        {
            item.defense = 4;
            item.accessory = true;
            item.width = 40;
            item.height = 36;
            item.rare = 8;
        }
        public override void UpdateEquip(Player player)
        {
            clericmodplayer.ModPlayer(player).clericNecroticMult += 0.1f;
            player.fireWalk = true;
            player.lavaImmune = true;
            player.armorPenetration += 8;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[ModContent.BuffType<BrimstoneDebuff>()] = true;

            player.GetModPlayer<modplayer>().brimstoneDevil = true;

            for (int n = 13; n < 18 + player.extraAccessorySlots; n++)
            {
                Item item = player.armor[n];
                if (item.type == ItemID.DemonWings)
                {
                    player.wingTimeMax *= 2;
                    player.GetModPlayer<modplayer>().trueDevil = true;
                    if (Main.rand.NextBool(2))
                    {
                        Dust d = Dust.NewDustDirect(player.position, player.width, player.height, 6, Scale: 1.2f);
                        d.fadeIn = d.scale * 1.2f;
                        d.noGravity = true;
                    }
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<modplayer>().devilVanity = true;
            if (hideVisual)
            {
                player.GetModPlayer<modplayer>().devilVanity = true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<BrimstoneCharm>());
            recipe.AddIngredient(ModContent.ItemType<BrimstoneRose>());
            recipe.AddIngredient(ItemID.FireFeather);
            recipe.AddIngredient(ItemID.ObsidianSkull);
            recipe.AddIngredient(ItemID.Ectoplasm, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class DevilHead : EquipTexture
    {
        public override bool DrawHead()
        {
            return false;
        }
    }

    public class DevilBody : EquipTexture
    {
        public override bool DrawBody()
        {
            return false;
        }
    }

    public class DevilLegs : EquipTexture
    {
        public override bool DrawLegs()
        {
            return false;
        }
    }
}
