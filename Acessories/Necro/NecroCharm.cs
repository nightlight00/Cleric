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

namespace clericclass.Acessories.Necro
{
    class NecroCharm : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Curse");
            Tooltip.SetDefault("7% increased necrotic damage" +
                             "\nBlood cost increased by 1");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = item.height = 38;
            item.rare = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().bloodCost -= 1;
            clericmodplayer.ModPlayer(player).clericNecroticMult += 0.07f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bone, 50);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }

    class CursedDevilCharm : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernal Decay Curse");
            Tooltip.SetDefault("10% increased necrotic damage" +
                             "\nBlood cost increased by 20%" +
                             "\nNecrotic weapons can inflict Cursed Inferno");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 38;
            item.height = 42;
            item.rare = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().cursedCharm = true;
            player.GetModPlayer<modplayer>().bloodCostMult -= .2f;
            clericmodplayer.ModPlayer(player).clericNecroticMult += 0.1f;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (player.GetModPlayer<modplayer>().ichorCharm || player.GetModPlayer<modplayer>().brimstoneCharm)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<NecroCharm>());
            recipe.AddIngredient(ItemID.CursedFlame, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }

    class IchorCurse : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Vein Curse");
            Tooltip.SetDefault("10% increased necrotic damage" +
                             "\nBlood cost increased by 20%" +
                             "\nNecrotic weapons can inflict Ichor");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 38;
            item.height = 42;
            item.rare = 5;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().ichorCharm = true;
            player.GetModPlayer<modplayer>().bloodCostMult -= .2f;
            clericmodplayer.ModPlayer(player).clericNecroticMult += 0.1f;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (player.GetModPlayer<modplayer>().cursedCharm || player.GetModPlayer<modplayer>().brimstoneCharm)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<NecroCharm>());
            recipe.AddIngredient(ItemID.Ichor, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class BrimstoneDevilCurse : ModItem
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bloody Vein Curse");
            Tooltip.SetDefault("'If you gaze into the abyss, the abyss will gaze right back'" +
                             "\n15% increased necrotic damage" +
                             "\nBlood cost increased by 30%" +
                             "\nDamage taken is increased by 10%" +
                             "\nNecrotic weapons can inflict Brimstone Flames");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 38;
            item.height = 46;
            item.rare = 6;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().brimstoneCharm = true;
            player.GetModPlayer<modplayer>().bloodCostMult -= .3f;
            clericmodplayer.ModPlayer(player).clericNecroticMult += 0.15f;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (player.GetModPlayer<modplayer>().cursedCharm || player.GetModPlayer<modplayer>().ichorCharm)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CursedDevilCharm>());
            recipe.AddIngredient(ModContent.ItemType<Materials.Brimstone>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ModContent.ItemType<IchorCurse>());
            recipe1.AddIngredient(ModContent.ItemType<Materials.Brimstone>(), 8);
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }

    }
}
