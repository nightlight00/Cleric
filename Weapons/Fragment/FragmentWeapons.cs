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

namespace clericclass.Weapons.Fragment
{
    class Seraphim : clericdamageitem
    {

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("interesting radiant weapon coming soon...." +
                             "\nFires a holy beam" + 
                             "\nAllies hit by the beam enter an angelic trance and release all dark emotions" +
                             "\nAdditionaly fires a spread of dark energy that transforms into light energy on impact");
            Item.staff[item.type] = true;
        }

        public override void SafeSetDefaults()
        {
            item.rare = 10;
            item.width = item.height = 100;
            item.damage = 50;
            item.noMelee = true;
            item.useStyle = ItemUseStyleID.HoldingOut;

            item.shootSpeed = 12f;
            item.shoot = ModContent.ProjectileType<HolyBeam>();
            item.knockBack = 4;
            item.useTime = item.useAnimation = 20;
            item.autoReuse = true;

            healAmount = 3;
            item.mana = 25;
        }

        // heal also gives immunity frames
        // trance : increased damage, increased endurance, increased immune frames
        // trance gives allies a floating eye and wings (purely cosmetic)
        // when given trance, spawns 5 dark energy

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (var i = 0; i < 3 + Main.rand.Next(3); i++)
            {
                float spd = Main.rand.NextFloat(0.7f, 0.95f);
                Vector2 speed = new Vector2(speedX * spd, speedY * spd).RotatedByRandom(MathHelper.ToRadians(24));
                Projectile.NewProjectile(position, speed, ModContent.ProjectileType<DarkEnergy>(), damage, knockBack, player.whoAmI, 1);
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.BlackholeFragment>(), 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class Apotheosis : clericdamageitem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Rains down dark stars from the sky" + 
                             "\nDark stars create mini blackholes on death that pulse destruction" + 
                             "\nAfter a while blackholes will condense in to light and dark energy");
            Item.staff[item.type] = true;
        }

        public override void SafeSetDefaults()
        {
            item.autoReuse = true;
            item.rare = 10;
            item.width = item.height = 96;
            item.damage = 69;
            item.noMelee = true;
            item.useStyle = ItemUseStyleID.HoldingOut;

            item.shootSpeed = 12f;
            item.shoot = ModContent.ProjectileType<DarkStar>();
            item.knockBack = 4;
            item.useTime = item.useAnimation = 32;

           // item.UseSound = SoundID.Item9;

            clericEvil = true;
            clericResourceCost = 18;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (var i = 0; i < 4; i++)
            {
                float originX = ((player.position.X + Main.MouseWorld.X) / 2) + Main.rand.NextFloat(-210, 210);
                float originY = player.position.Y - ((Main.screenHeight / 2) + Main.rand.NextFloat(30));
                Vector2 shootVel = new Vector2(Main.MouseWorld.X + Main.rand.NextFloat(-50, 50), Main.MouseWorld.Y) - new Vector2(originX, originY);
                shootVel.Normalize();
                shootVel *= 12f + Main.rand.NextFloat(5);

                Projectile.NewProjectile(new Vector2(originX, originY), shootVel, type, damage, knockBack, player.whoAmI);
                Main.PlaySound(SoundID.Item9, (int)originX, (int)originY);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.BlackholeFragment>(), 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class LightEnergy : Lifesteals.LifeStealBase
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.DD2DarkMageBolt;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 4;
            projectile.alpha = 255;
            projectile.timeLeft = 360;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.extraUpdates = 3;
        }

        public override void CreateDust()
        {
            Dust dst = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 204, Scale: 1.2f);
            dst.noGravity = true;
            dst.velocity /= 5;
        }
    }

    class HolyBeam : clericProj
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.DD2DarkMageBolt;

        public override void SafeSetDefaults()
        {
            projectile.width = projectile.height = 16;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.timeLeft = 240;
            //projectile.
            // dust 132
        }

        public override bool CanDamage() => false;

        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(projectile.Center + (speed * 8), 204, speed * 0, Scale: 1.5f);
                d.noGravity = true;
            }
        }
    }

    class DarkEnergy : clericProj
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.DD2DarkMageBolt;

        public override void SafeSetDefaults()
        {
            projectile.width = projectile.height = 4;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.timeLeft = 240;
            //projectile.
         // dust 132
        }

        public override void AI()
        {
            Vector2 move = Vector2.Zero;
            float distance = 200f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
                {
                    Vector2 newMove = Main.npc[k].Center - projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                projectile.velocity = (10 * projectile.velocity + move) / 11f;
                AdjustMagnitude(ref projectile.velocity);
            }

            int dust = Dust.NewDust(projectile.Center, 0, 0, 62, Scale: 1.8f);
            Main.dust[dust].velocity /= 5f;
            Main.dust[dust].noGravity = true;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 9.5f)
            {
                vector *= 9.5f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.ai[0] > 0 && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
            {
                projectile.ai[0]++;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[0] == 2)
            {
                Projectile.NewProjectile(projectile.Center, -projectile.velocity, ModContent.ProjectileType<LightEnergy>(), 0, 0, Main.player[projectile.owner].whoAmI, ai1: 3);
            }
        }
    }

    class DarkStar : clericProj
    {
        public override void SafeSetDefaults()
        {
            projectile.width = projectile.height = 22;
            projectile.friendly = true;
            projectile.timeLeft = 150;
            projectile.tileCollide = false;
        }

        Vector2 pos;

        public override void AI()
        {
            if (projectile.velocity.X < 0)
            {
                projectile.rotation -= MathHelper.ToRadians(11);
            }
            else
            {
                projectile.rotation += MathHelper.ToRadians(11);
            }
            //Lighting.AddLight(projectile.Center, Color.Purple.R/15, Color.Purple.G/15, Color.Purple.B/15);

            Dust dst = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 62, SpeedY: -projectile.velocity.Y, Scale: Main.rand.NextFloat(2.2f, 2.4f));
            dst.noGravity = true;

            if (projectile.ai[0] == 0)
            {
                if (Main.myPlayer == projectile.owner)
                {
                    pos = Main.MouseWorld;
                    pos.Y += Main.rand.NextFloat(40);
                    projectile.ai[0]++;
                }
            }
            if (projectile.position.Y > pos.Y)
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCHit54, (int)projectile.Center.X, (int)projectile.Center.Y);
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<DarkMatterOrb>(), (int)(projectile.damage * 0.75f), 0, Main.player[projectile.owner].whoAmI);
            for (var i = 0; i < 15; i++)
            {
                Dust dst = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 62, Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                dst.noGravity = true;
                dst.scale = Main.rand.NextFloat(1.4f, 1.9f);
            }
        }
    }

    class DarkMatterOrb : clericProj
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
        }

        public override void SafeSetDefaults()
        {
            projectile.width = projectile.height = 30;
            projectile.friendly = true;
            projectile.timeLeft = 120;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        public override bool CanDamage() => false;

        public override void AI()
        {
            if (++projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 5)
                {
                    Main.PlaySound(SoundID.Item116, (int)projectile.Center.X, (int)projectile.Center.Y);
                    Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<DarkPulse>(), projectile.damage, 0, Main.player[projectile.owner].whoAmI);
                    projectile.frame = 0;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(projectile.Center + (speed * 15), 62, speed * 0, Scale: 1.5f);
                // looks pretty cool Dust d = Dust.NewDustPerfect(Main.LocalPlayer.Top, 92, speed * 53, Scale: 1.5f);
                d.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCDeath52, (int)projectile.Center.X, (int)projectile.Center.Y);
            Vector2 vel = Main.player[projectile.owner].position - projectile.Center;
            vel.Normalize();
            vel *= 3;
            vel = vel.RotatedByRandom(MathHelper.ToRadians(360));
            for (var d = 0; d < Main.rand.Next(3, 5); d++)
            {
                Vector2 vel2 = Main.player[projectile.owner].position - projectile.Center;
                vel2.Normalize();
                vel2 *= 9;
                vel2 = vel2.RotatedByRandom(MathHelper.ToRadians(360));
                Projectile.NewProjectile(projectile.Center, vel2, ModContent.ProjectileType<DarkEnergy>(), projectile.damage / 2, projectile.knockBack, Main.player[projectile.owner].whoAmI);
            }
            Projectile.NewProjectile(projectile.Center, vel, ModContent.ProjectileType<LightEnergy>(), 0, 0, Main.player[projectile.owner].whoAmI, Main.player[projectile.owner].whoAmI, 2);
            for (var i = 0; i < 15; i++)
            {
                Dust dst = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 62, Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6));
                dst.noGravity = true;
                dst.scale = Main.rand.NextFloat(1.7f, 2.1f);
            }
        }
    }

    class DarkPulse : clericProj
    {

        public override void SafeSetDefaults()
        {
            projectile.width = projectile.height = 30;
            projectile.friendly = true;
            projectile.timeLeft = 120;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.scale += 0.1f;
            projectile.alpha += 255 / 20;
            if (projectile.scale > 3)
            {
                projectile.Kill();
            }
        }
    }
}
