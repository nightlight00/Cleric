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

namespace clericclass.Weapons.Melee
{
    class clericMelee : clericdamageitem
    {
        public override string Texture => "clericclass/Weapons/Melee/big_hammer_pixelart_by_listochik_deby5ir-fullview";

        int Timer = 60;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guardians Hammer");
            Tooltip.SetDefault("dev note : sprite taken off of google, needs to be changed" + "\nRight click to summon protective spirits to guard over your current position");
        }

        public override void SafeSetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = item.height = 44;
            item.damage = 20;
            item.useTime = item.useAnimation = 32;
            item.knockBack = 3;
            item.value = 10000;
            item.rare = 3;
            item.UseSound = SoundID.Item1;

            item.shoot = ModContent.ProjectileType<SpiritGuardians>();
            item.shootSpeed = 0;

            clericEvil = false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = item.useAnimation = 20;
                item.knockBack = 0;
                item.shoot = ModContent.ProjectileType<SpiritGuardians>();
                item.noUseGraphic = true;
                item.noMelee = true;
                item.useStyle = ItemUseStyleID.HoldingUp;
                item.autoReuse = false;
                item.UseSound = SoundID.Item4;
                if (Timer > 0) { return false; }
            }
            else
            {
                item.useTime = item.useAnimation = 32;
                item.knockBack = 3;
                item.shoot = ProjectileID.None;
                item.noUseGraphic = false;
                item.noMelee = false;
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.UseSound = SoundID.Item1;
                item.autoReuse = false;
            }
            return base.CanUseItem(player);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            for (var i = 0; i < (target.width/20)+8; i++)
            {
                Dust dst = Dust.NewDustDirect(new Vector2(target.position.X, target.position.Y + (target.height / 2)), target.width, 0, 133, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, -5));
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                //Projectile.NewProjectile(player.Center, new Vector2(0, 0), ModContent.ProjectileType<SpiritGuardians>(), 15, 0);
                for (var i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(player.Center, new Vector2(0, 0), ModContent.ProjectileType<GuardianFly>(), 30, 0, player.whoAmI, i);
                }
                Timer = 900;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void UpdateInventory(Player player)
        {
            Timer--;
        }
    }

    class GuardianFly : clericProj
    {
        public override void SetDefaults()
        {
            projectile.height = 40;
            projectile.width = 66;
            projectile.timeLeft = 300;
            projectile.tileCollide = false;
            projectile.alpha = 150;
            projectile.penetrate = -1;
            projectile.friendly = true;

            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.localNPCHitCooldown = 15;
        }

        float rot;
        Vector2 pos;
        float dist = 96;
        float spd = 0.065f;

        public override void AI()
        {
            if (projectile.timeLeft < 45)
            {
                dist += 2.5f;
                spd *= 1.02f;
                projectile.alpha += 105/45;
            }
            if (projectile.ai[0] != 4)
            {
                pos = projectile.position;
                rot = MathHelper.ToRadians(projectile.ai[0] * 120);
                projectile.ai[0] = 4;
            }
            projectile.position = pos;
            rot += spd;
            projectile.position.X += (float)(Math.Cos(rot) * dist);
            projectile.position.Y += (float)(Math.Sin(rot) * dist);
            Dust d = Dust.NewDustPerfect(projectile.Center, 133, new Vector2(0,0), Scale: 0.55f);
            d.noGravity = true;
        }
    }

    class SpiritGuardians : clericProj
    {
        public override string Texture => "clericclass/Weapons/Templates/clericProjBase";
        public override void SetDefaults()
        {
            projectile.height = projectile.width = 192;
            projectile.timeLeft = 300;
            projectile.tileCollide = false;
            projectile.alpha = 225;
            projectile.penetrate = -1;
            projectile.friendly = true;

            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.localNPCHitCooldown = 45;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            if (projectile.timeLeft < 30)
            {
                projectile.alpha += 1;
            }
            //projectile.Center = Main.player[projectile.owner].Center;
            Buff(Main.player[projectile.owner], Main.LocalPlayer, ModContent.BuffType<Guardians>(), 3);
        }
    }

    class Guardians : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Guardians");
            Description.SetDefault("Protected by heavenly light \n5% increased damage reduction and 8 defense");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            longerExpertDebuff = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance = 0.05f;
            player.statDefense += 8;
            Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
            Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X + (speed.X * 12), player.Center.Y - player.height + (speed.Y * 6)), 133, speed *= 0);
            d.noGravity = true;
        }
    }
}
