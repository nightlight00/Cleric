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

namespace clericclass.Acessories.Healer
{
    class CrystallineCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystalline Charm");
            Tooltip.SetDefault("Healing gives an additional 2 health" +
                             "\nApplied buffs last 2 seconds longer");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = item.height = 32;
            item.rare = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().healBonus += 2;
            player.GetModPlayer<modplayer>().buffBonus += 2;
            player.GetModPlayer<modplayer>().charmEquppied = true;
            player.GetModPlayer<modplayer>().crystalCharmEquipped = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (player.GetModPlayer<modplayer>().charmEquppied || player.GetModPlayer<modplayer>().crystalCharmEquipped)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrystalShard, 8);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class CrystallineEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystalline Emblem");
            Tooltip.SetDefault("10% reduced cleric mana cost" +
                             "\nHealing gives an additional 2 health" +
                             "\nApplied buffs last 4 seconds longer");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 38;
            item.height = 36;
            item.rare = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().manaReduce = 0.1f;
            player.GetModPlayer<modplayer>().healBonus += 2;
            player.GetModPlayer<modplayer>().buffBonus += 4;
            player.GetModPlayer<modplayer>().charmEquppied = true;
            player.GetModPlayer<modplayer>().crystalCharmEquipped = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (player.GetModPlayer<modplayer>().charmEquppied || player.GetModPlayer<modplayer>().crystalCharmEquipped)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CrystallineCharm>());
            recipe.AddIngredient(ItemID.AvengerEmblem);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Shield)]
    class CrystallineShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystalline Shield");
            Tooltip.SetDefault("Absorbs 25% of damage done to players on your team while above 25% max life" +
                             "\n15% reduced cleric mana cost" +
                             "\nHealing gives an additional 3 health" +
                             "\nApplied buffs last 5 seconds longer");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 38;
            item.height = 36;
            item.defense = 8;
            item.rare = 8;
        }
        public override void UpdateEquip(Player player)
        {
            player.hasRaisableShield = true;

            player.GetModPlayer<modplayer>().manaReduce = 0.15f;
            player.GetModPlayer<modplayer>().healBonus += 3;
            player.GetModPlayer<modplayer>().buffBonus += 5;
            player.GetModPlayer<modplayer>().crystalCharmEquipped = true;
            player.hasPaladinShield = true;
            player.noKnockback = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (player.GetModPlayer<modplayer>().crystalCharmEquipped || player.hasPaladinShield)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CrystallineEmblem>());
            recipe.AddIngredient(ItemID.PaladinsShield);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    //[AutoloadEquip(EquipType.Shield)]
    class AbyssalShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Shield");
            Tooltip.SetDefault("While below half health, nearby players have immunity to Moon Bite" +
                             "\nWhile above half health, nearby players gain the ability to regenerate some health when hurt" +
                             "\n10% reduced cleric mana cost and blood cost");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 38;
            item.height = 36;
            item.rare = 10;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().manaReduce += 0.1f;
            player.GetModPlayer<modplayer>().bloodCostMult += 0.1f;
            player.GetModPlayer<modplayer>().abyssalShield = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.BlackholeFragment>(), 8);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
