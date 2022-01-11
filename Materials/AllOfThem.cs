using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace clericclass.Materials
{
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
}
