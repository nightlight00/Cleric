using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using clericclass.ClericBase;
using clericclass.Weapons.Templates;

namespace clericclass.Armor.Priest
{
    [AutoloadEquip(EquipType.Head)]
    class PriestHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Priest's Hood");
            Tooltip.SetDefault("4% increased radient damage" +
                             "\n2% increased cleric critical strike chance");
                              
        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 24;
            item.rare = 1;
            item.defense = 2;
        }

        public override bool DrawHead() => true;

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericRadientMult += 0.04f;
            modPlayer.clericCrit += 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PriestChest>() && legs.type == ModContent.ItemType<PriestBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            player.setBonus = "Healing gives allies a minor blessing" + 
                            "\nDeal 1 additional radient and necrotic damage";

            modPlayer.clericNecroticAdd += 1;
            modPlayer.clericRadientAdd += 1;

            player.GetModPlayer<modplayer>().priestSetBonus = true;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class PriestChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Priest's Robes");
            Tooltip.SetDefault("5% increased radient and necrotic damage");

        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 24;
            item.rare = 1;
            item.defense = 3;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }

        public override bool DrawLegs() => false;

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericRadientMult += 0.05f;
            modPlayer.clericNecroticMult += 0.05f;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class PriestBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Priest's Boots");
            Tooltip.SetDefault("3% increased necrotic damage" +
                             "\n5% increased movement speed");

        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 24;
            item.rare = 1;
            item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.03f;
            player.moveSpeed += 0.05f;
        }
    }
}
