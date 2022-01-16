using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace clericclass.ClericBase
{
    class modglobalnpc : GlobalNPC 
    {
        public override bool InstancePerEntity => true;

        public bool holyfire = false;

        public override void ResetEffects(NPC npc)
        {
            holyfire = false;
        }

        public override void SetDefaults(NPC npc)
        {
            // We want our ExampleJavelin buff to follow the same immunities as BoneJavelin
            npc.buffImmune[ModContent.BuffType<Armor.Flamesilk.HolyFire>()] = npc.buffImmune[BuffID.CursedInferno];
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (holyfire)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 9;
                if (damage < 3)
                {
                    damage = 3;
                }
            }
        }

        public override void NPCLoot(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.GraniteGolem:
                    if (Main.rand.NextBool(50))
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Weapons.PreBoss.Basic.GraniteStaff>(), 1);
                    }
                    break;
                case NPCID.KingSlime:
                    if (Main.rand.NextBool(2) && !Main.expertMode)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Weapons.BossTheme.Slime.OverseerStaff>(), 1);
                    }
                    break;
                case NPCID.IceElemental:
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Materials.Runestone>(), 1);
                    break;
            }
        }
    }

    class modplayer : ModPlayer
    {
        // buffs
        public bool blessingMinor = false;

        // debuffs
        public bool holyFire = false;

        // weapon related
        public int healBonus = 0;
        public int buffBonus = 0;
        public int bloodCost = 0;
        public bool anguish = false;
        public bool healHappy = false;
        public bool healGourd = false;
        public bool healCamp = false;

        // armor set bonuses
        public bool priestSetBonus = false;
        public bool flamesilkSetBonus = false;
        public bool heartSetBonus = false;
        public bool worshipSetBonusEffect = false;
        public bool astridSetElemental = false;

        // for ease of use
        public bool currentWeaponEvil = false;
        public bool charmEquppied = false;

        public override void ResetEffects()
        {
            blessingMinor = false;

            holyFire = false;

            healBonus = 0;
            buffBonus = 0;
            bloodCost = 0;
            anguish = false;
            healHappy = false;
            healGourd = false;
            healCamp = false;

            priestSetBonus = false;
            flamesilkSetBonus = false;
            heartSetBonus = false;
            worshipSetBonusEffect = false;
            astridSetElemental = false;

            currentWeaponEvil = false;
            charmEquppied = false;
        }

        public override void UpdateBadLifeRegen()
        {
            if (anguish)
            {
                // lowers life regen, but doesnt make it go under 0 by itself
                player.lifeRegen /= 2;
            }
            if (holyFire)
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                player.lifeRegen -= 9;
            }
        }
    }
}
