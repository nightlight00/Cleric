using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace clericclass.Materials
{

    class RealityFabirc : ModItem
    {
        public override string Texture => "Terraria/Item_" + ItemID.Silk;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fabric of Reality");
            Tooltip.SetDefault("Bends the fates with even the smallest of creases");
        }

        public override void SetDefaults()
        {
            item.value = 25000;
            item.width = 24;
            item.height = 24;
            item.rare = ItemRarityID.Orange;
            item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<FlameSilk>(), 3);
            recipe.AddIngredient(ItemID.AncientCloth, 3);
            recipe.AddIngredient(ItemID.Ectoplasm, 1);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }

    class FlameSilk : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A luxurious fabric that always keeps you warm");
        }

        public override void SetDefaults()
        {
            item.value = 25000;
            item.width = 32;
            item.height = 34;
            item.rare = ItemRarityID.Orange;
            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddIngredient(ItemID.Fireblossom);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }

    class LifeAlloy : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You can still hear it's heartbeat");
        }

        public override void SetDefaults()
        {
            item.value = 25000;
            item.width = 32;
            item.height = 34;
            item.rare = ItemRarityID.Orange;
            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LifeCrystal);
            recipe.AddIngredient(ItemID.Bone, 25);
            recipe.AddIngredient(ItemID.DemoniteBar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 3);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.LifeCrystal);
            recipe1.AddIngredient(ItemID.Bone, 25);
            recipe1.AddIngredient(ItemID.CrimtaneBar, 5);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this, 3);
            recipe1.AddRecipe();
        }
    }

    class Runestone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Runestone");
            Tooltip.SetDefault("A mysterious gem that animates ice elementals");
        }

        public override void SetDefaults()
        {
            item.value = 25000;
            item.width = 24;
            item.height = 30;
            item.rare = ItemRarityID.Pink;
            item.maxStack = 999;
        }

    }

    class SanguineOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Glacial Runestone");
            Tooltip.SetDefault("Ancient spirits revitalized this blood during the crimson moon");
        }

        public override void SetDefaults()
        {
            item.value = 25000;
            item.width = 24;
            item.height = 32;
            item.rare = ItemRarityID.Pink;
            item.maxStack = 999;
        }

    }


    class Brimstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brimstone");
            Tooltip.SetDefault("An igneous stone with a sulfurous smell");
        }

        public override void SetDefaults()
        {
            item.value = 15000;
            item.width = 36;
            item.height = 40;
            item.rare = 6;
            item.maxStack = 999;
        }
    }

    class BlackholeFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blackhole Fragment");
            Tooltip.SetDefault("'The depth of the universe is being crushed by this fragment'");
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.value = 20000;
            item.width = 26;
            item.height = 22;
            item.rare = 9;
            item.maxStack = 999;
        }
    }
}
