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

namespace clericclass.Acessories
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
            player.GetModPlayer<modplayer>().bloodCost += 1;
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
}
