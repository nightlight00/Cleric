using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using clericclass.ClericBase;
using clericclass.Weapons.Templates;

namespace clericclass.Armor.Harvest
{

    [AutoloadEquip(EquipType.Head)]
    class HarvestMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grim Harvest Mask");
            Tooltip.SetDefault("12% increased necrotic damage");
                              
        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 26;
            item.rare = 8;
            item.defense = 13;
        }

        public override bool DrawHead() => true;

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.12f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HarvestJacket>() && legs.type == ModContent.ItemType<HarvestBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Spins 2 deadly pumpkin scythes around you \nCleric life steal can critically heal \n10% increased necrotic damage and cleric critical strike chance";
            // rework this into : nearby foes are inflicted with holy fire , healing allies gives them 'vengeful flames' (thorns but holy fire)

            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.1f;
            modPlayer.clericCrit += 10;
            player.GetModPlayer<modplayer>().harvestSetBonus = true;

        }

        public override void ArmorSetShadows(Player player)
        {
            if (player.GetModPlayer<modplayer>().harvestSetBonus)
            {
                player.armorEffectDrawOutlines = true;
                player.armorEffectDrawShadow = true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pumpkin, 20);
            recipe.AddIngredient(ItemID.SpookyWood, 50);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class HarvestJacket : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grim Harvest Jacket");
            Tooltip.SetDefault("10% increased cleric critical strike chance" +
                             "\nBlood cost decreased by 18%");
        }

        public override void SetDefaults()
        {
            item.height = 24;
            item.width = 26;
            item.rare = 8;
            item.defense = 19;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericCrit += 10;
            player.GetModPlayer<modplayer>().bloodCostMult += 0.18f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pumpkin, 20);
            recipe.AddIngredient(ItemID.SpookyWood, 100);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class HarvestBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grim Harvest Boots");
            Tooltip.SetDefault("12% increased necrotic damage");

        }

        public override void SetDefaults()
        {
            item.height = 16;
            item.width = 26;
            item.rare = 8;
            item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.12f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pumpkin, 20);
            recipe.AddIngredient(ItemID.SpookyWood, 75);
            recipe.AddIngredient(ItemID.SoulofFright, 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
