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
                case NPCID.BloodZombie:
                    if (Main.bloodMoon && Main.rand.NextBool(50))
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Weapons.Blood.SacrificeDagger>(), 1);
                    }
                    break;
            }
        }
    }

    class modplayer : ModPlayer
    {
        // buffs
        public bool blessingMinor = false;
        public bool flameGuard = false;

        // debuffs
        public bool holyFire = false;

        // weapon related
        public int healBonus = 0;
        public int buffBonus = 0;
        public int bloodCost = 0;
        public float bloodCostMult = 0;
        public bool anguish = false;

        // accessories
        public bool healHappy = false;
        public bool healGourd = false;
        public bool healCamp = false;
        public bool healSummer = false;

        // armor set bonuses
        public bool priestSetBonus = false;
        public bool flamesilkSetBonus = false;
        public bool heartSetBonus = false;
        public bool worshipSetBonusEffect = false;
        public bool astridSetElemental = false;
        public bool harvestSetBonus = false;

        // for ease of use
        public bool currentWeaponEvil = false;
        public bool charmEquppied = false;
        public bool summerBuff = false;

        public override void ResetEffects()
        {
            blessingMinor = false;
            flameGuard = false;

            holyFire = false;

            healBonus = 0;
            buffBonus = 0;
            bloodCost = 0;
            bloodCostMult = 0;
            anguish = false;

            healHappy = false;
            healGourd = false;
            healCamp = false;
            healSummer = false;

            priestSetBonus = false;
            flamesilkSetBonus = false;
            heartSetBonus = false;
            worshipSetBonusEffect = false;
            astridSetElemental = false;
            harvestSetBonus = false;

            currentWeaponEvil = false;
            charmEquppied = false;
            summerBuff = false;
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (flameGuard)
            {
                npc.AddBuff(ModContent.BuffType<Armor.Flamesilk.HolyFire>(), 300);
                npc.life -= 40;
                CombatText.NewText(npc.getRect(), Color.OrangeRed, 40);
                Main.PlaySound(SoundID.Item14, npc.position);
                for (var i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, 133, Main.rand.NextFloat(-5, 5), -4, Scale: 1.25f);
                    d.fadeIn = d.scale + 0.5f;
                }
                if (npc.life <= 0) { npc.checkDead(); }
            }
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            if (flameGuard)
            {
                var npc = Main.npc[proj.owner];
                npc.AddBuff(ModContent.BuffType<Armor.Flamesilk.HolyFire>(), 300);
                npc.life -= 40;
                CombatText.NewText(npc.getRect(), Color.Red, 40);
                Main.PlaySound(SoundID.Item14, npc.position);
                for (var i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, 133, Main.rand.NextFloat(-5, 5), -4, Scale: 1.25f);
                    d.fadeIn = d.scale + 0.5f;
                }
                if (npc.life <= 0) { npc.checkDead(); }
            }
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
