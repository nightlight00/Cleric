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

namespace clericclass.Acessories.Buffs
{
    class SunflowerCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Healing makes people happy!" +
                             "\nThey were probably already happy that you're healing them");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = item.height = 30;
            item.rare = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().healHappy = true;
            player.GetModPlayer<modplayer>().charmEquppied = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (player.GetModPlayer<modplayer>().charmEquppied)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Sunflower);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    class PumpkinCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Healing applies the power of gourds!" +
                             "\nAlso makes them smell like pumpkin pie...");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 30;
            item.height = 26;
            item.rare = 1;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().healGourd = true;
            player.GetModPlayer<modplayer>().charmEquppied = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (player.GetModPlayer<modplayer>().charmEquppied)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pumpkin, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    class CampfireCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Campfire Memento");
            Tooltip.SetDefault("Healing reminds people of a cozy campfire!" +
                             "\n'Pass the marshmallows, would ya?'");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 24;
            item.height = 28;
            item.defense = 1;
            item.rare = 1;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().healCamp = true;
            player.GetModPlayer<modplayer>().charmEquppied = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (player.GetModPlayer<modplayer>().charmEquppied)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chain, 3);
            recipe.AddIngredient(ItemID.Wood, 5);
            recipe.AddTile(TileID.Campfire);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
