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
    class BalefulHarvest : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Baleful Harvest");
            Description.SetDefault("Rapidly losing life and extra damage when hit");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<modglobalnpc>().balefulHarvest = true;
            Dust dst = Dust.NewDustDirect(npc.position, npc.width, npc.height, 109, Main.rand.NextFloat(-1, 1), -2);
            dst.scale = Main.rand.NextFloat(1.2f, 1.5f);
            dst.fadeIn = dst.scale * 1.3f;
            dst.noGravity = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<modplayer>().balefulHarvest = true;
            Dust dst = Dust.NewDustDirect(player.position, player.width, player.height, 109, Main.rand.NextFloat(-1, 1), -2);
            dst.scale = Main.rand.NextFloat(1.2f, 1.5f);
            dst.fadeIn = dst.scale * 1.3f;
            dst.noGravity = true;
        }
    }


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
            player.setBonus = "Necoritc attacks can inflict Baleful Harvest \n12% increased necrotic damage and cleric critical strike chance";
            // rework this into : nearby foes are inflicted with holy fire , healing allies gives them 'vengeful flames' (thorns but holy fire)

            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.12f;
            modPlayer.clericCrit += 12;
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
                             "\nBlood cost decreased by 22%");
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
            player.GetModPlayer<modplayer>().bloodCostMult += 0.22f;
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
