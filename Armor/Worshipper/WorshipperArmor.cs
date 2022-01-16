using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using clericclass.ClericBase;
using clericclass.Weapons.Templates;

namespace clericclass.Armor.Worshipper
{
    class WorshipBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ancient's Blessing");
            Description.SetDefault("Protected by thine's worshipped \nAll damage increased by 10%, damage taken decreased by 6%");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.magicDamageMult += 0.1f;
            player.meleeDamageMult += 0.1f;
            player.rangedDamageMult += 0.1f;
            player.minionDamageMult += 0.1f;
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.08f;
            modPlayer.clericRadientMult += 0.08f;
            player.endurance += 0.06f;
        }
    }

    [AutoloadEquip(EquipType.Head)]
    class WorshipperHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Worshipper Hood");
            Tooltip.SetDefault("9% increased radiant damage" +
                             "\n6% increased cleric critical strike chance");
                              
        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 24;
            item.rare = 4;
            item.defense = 7;
        }

        public override bool DrawHead() => true;

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericRadientMult += 0.09f;
            modPlayer.clericCrit += 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<WorshipperRobe>() && legs.type == ModContent.ItemType<WorshipperBoots>();
        }

        public virtual string DeathMessage()
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "Ra's dissapointed in you, " + Main.player[item.owner].name;
                case 1:
                    return Main.player[item.owner].name + " wanted to become a mummy";
                case 2:
                    return Main.player[item.owner].name + " became a sacrifice for Ra";
            }
        }

        int abilityTimer = 60;
        string status = "not ready";
        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            player.setBonus = "Double tap down to sacrifice 10 health in order to activate 'Ra's Blessing'" +
                            "\nWhile you have 'Ra's Blessing', healed allies gain numerous benefits" +
                            "\nStatus : " + status +
                            "\nApplied buffs last 4 seconds longer";
            // benefits -> all fire and poison-like debuffs are negated, all damage is increased by 10% (cleric damages by 8%), and 8% damage reduction

            player.GetModPlayer<modplayer>().buffBonus += 4;
            abilityTimer--;
            if (player.controlDown && player.releaseDown && player.doubleTapCardinalTimer[0] < 15 && abilityTimer <= 0)
            {
                player.statLife -= 10;
                abilityTimer = 1200;
                if (player.statLife <= 0) { 
                    player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(DeathMessage()), 0, 0);
                    abilityTimer = 100;
                }
            }
            if (abilityTimer > 900)
            {
                player.GetModPlayer<modplayer>().worshipSetBonusEffect = true;
                Dust dst = Dust.NewDustDirect(player.position, player.width, player.height, 87, -player.velocity.X, -3);
                dst.noGravity = true;
                status = "activated";
            }
            else if (abilityTimer > 0) { status = "not ready"; }
            else { status = "ready"; }
            if (abilityTimer == 0)
            {
                CombatText.NewText(player.getRect(), Color.LightGoldenrodYellow, "Ra's Blessing Ready!");
            }
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class WorshipperRobe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Worshipper Robes");
            Tooltip.SetDefault("7% increased radiant and necrotic damage" +
                             "\nHealing gives an additonal 3 health");

        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 24;
            item.rare = 4;
            item.defense = 10;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }


        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericRadientMult += 0.07f;
            modPlayer.clericNecroticMult += 0.07f;
            player.GetModPlayer<modplayer>().healBonus += 3;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class WorshipperBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Worshipper Boots");
            Tooltip.SetDefault("9% increased necrotic damage" +
                             "\n10% increased movement speed");

        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 24;
            item.rare = 4;
            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.09f;
            player.moveSpeed += 0.1f;
        }
    }
}
