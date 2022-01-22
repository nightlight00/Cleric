using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using clericclass.ClericBase;
using Microsoft.Xna.Framework.Graphics;

namespace clericclass.Weapons.Templates
{
    public abstract class clericProj : ModProjectile
    {

        public int HealExtra;

        public override void SetDefaults()
        {
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.localNPCHitCooldown = 10;
            SafeSetDefaults();
        }

        public override bool? CanCutTiles() => false;

        public virtual void Buff(Player owner, Player target, int buff, int time, int radius = -1)
        {
            Vector2 center = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
            float offsetX = target.Center.X - center.X;
            float offsetY = target.Center.Y - center.Y;
            float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
            int range = projectile.width;
            if (projectile.height > range) { range = projectile.height; }
            if (radius != -1) { range = radius; }
            if (distance < range && projectile.position.X < target.position.X + target.width && projectile.position.X + projectile.width > target.position.X && projectile.position.Y < target.position.Y + target.height && projectile.position.Y + projectile.height > target.position.Y)
            {
                target.AddBuff(buff, (time + owner.GetModPlayer<modplayer>().healBonus) * 60);
            }
        }

        // for spawning new projectiles that have unique damage values so that they gain the bonuses
        public virtual void ClericProjectile(Player owner, Vector2 position, Vector2 velocity, int type, int damage, float knockback, bool radiant, float ai0 = 0, float ai1 = 0)
        {
            if (radiant)
            {
                damage += (int)clericmodplayer.ModPlayer(owner).clericRadientAdd;
                damage = (int)(damage * clericmodplayer.ModPlayer(owner).clericRadientMult + ((clericmodplayer.ModPlayer(owner).clericNecroticMult - 1) * 0.25f));
            }
            else
            {
                damage += (int)clericmodplayer.ModPlayer(owner).clericNecroticAdd;
                damage = (int)(damage * clericmodplayer.ModPlayer(owner).clericNecroticMult + ((clericmodplayer.ModPlayer(owner).clericRadientMult - 1) * 0.25f));
            }
            Projectile.NewProjectile(position, velocity, type, damage, knockback, owner.whoAmI, ai0, ai1);
        }

        public virtual void Heal(Player owner, Player target, string style, int heal, bool draw = false)
        {
            var amount = heal + owner.GetModPlayer<modplayer>().healBonus;
            if (amount > (target.statLifeMax2 - target.statLife))
            {
                amount = target.statLifeMax2 - target.statLife;
            }
            if (amount != 0)
            {
                // ApplyBuffs(owner, target);
                if (owner.GetModPlayer<modplayer>().summerBuff) { amount++; }
                target.statLife += amount;
                CombatText.NewText(target.getRect(), Color.Lime, amount + " health " + style);
            }
            if (owner.GetModPlayer<modplayer>().priestSetBonus)
            {
                target.AddBuff(ModContent.BuffType<Buffs.Blessings.BlessingMinor>(), (12 + (amount*2)) * 60);
            }
            if (owner.GetModPlayer<modplayer>().flamesilkSetBonus)
            {
                target.AddBuff(ModContent.BuffType<Armor.Flamesilk.Flameguard>(), (10 + (amount * 2)) * 60);
            }
            AfterHeal();
        }

        public virtual void AfterHeal() { }

        public virtual void HealCollision(Player owner, Player target, string style, int heal, bool draw = false, int radius = -1, bool destroy = false)
        {
            Vector2 center = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
            float offsetX = target.Center.X - center.X;
            float offsetY = target.Center.Y - center.Y;
            float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
            int range = projectile.width;
            if (projectile.height > range) { range = projectile.height; }
            if (radius != -1) { range = radius; }
            if (distance < range && projectile.position.X < target.position.X + target.width && projectile.position.X + projectile.width > target.position.X && projectile.position.Y < target.position.Y + target.height && projectile.position.Y + projectile.height > target.position.Y)
            {
                var amount = heal + owner.GetModPlayer<modplayer>().healBonus;
                if (amount > (target.statLifeMax2 - target.statLife))
                {
                    amount = target.statLifeMax2 - target.statLife;
                }
                if (amount != 0)
                {
                    ApplyBuffs(owner, target);

                    if (owner.GetModPlayer<modplayer>().summerBuff) { amount++; }

                    CombatText.NewText(target.getRect(), Color.Lime, amount + " health " + style);
                    if (destroy)
                    {
                        projectile.Kill();
                    }
                }
                if (owner.GetModPlayer<modplayer>().priestSetBonus)
                {
                    target.AddBuff(ModContent.BuffType<Buffs.Blessings.BlessingMinor>(), (12 + (amount * 2)) * 60);
                }
                if (owner.GetModPlayer<modplayer>().flamesilkSetBonus)
                {
                    target.AddBuff(ModContent.BuffType<Armor.Flamesilk.Flameguard>(), (10 + (amount * 2)) * 60);
                }
            }
        }

        public virtual void ApplyBuffs(Player owner, Player target)
        {
            int buffBonus = owner.GetModPlayer<modplayer>().buffBonus;
            if (owner.GetModPlayer<modplayer>().healHappy) { target.AddBuff(BuffID.Sunflower, (8 + buffBonus) * 60); }
            if (owner.GetModPlayer<modplayer>().healGourd) { target.AddBuff(ModContent.BuffType<Buffs.Charms.GourdDefense>(), (8 + buffBonus) * 60); }
            if (owner.GetModPlayer<modplayer>().healCamp) { target.AddBuff(BuffID.Campfire, (8 + buffBonus) * 60); }
            if (owner.GetModPlayer<modplayer>().healSummer) { target.AddBuff(ModContent.BuffType<Buffs.Charms.SummerSpirit>(), (7 + buffBonus) * 60); }

            // armor bonuses
            if (owner.GetModPlayer<modplayer>().worshipSetBonusEffect) { target.AddBuff(ModContent.BuffType<Armor.Worshipper.WorshipBuff>(), (10 + buffBonus) * 60); }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.rand.Next(99) <= clericmodplayer.ModPlayer(Main.player[projectile.owner]).clericCrit)
            {
                crit = true;
            }
        }

        public virtual bool CheckKilled(NPC target, int damage, bool crit)
        {
            if (target.life <= 0 && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
            {
                return true;
            }
            return false;
        }

        public virtual void SafeSetDefaults()
        {
        }

    }
}
