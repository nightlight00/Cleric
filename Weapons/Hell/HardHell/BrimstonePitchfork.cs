using clericclass.ClericBase;
using clericclass.Weapons.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using clericclass.Dusts;

namespace clericclass.Weapons.Hell.HardHell
{
	#region Pitchfork
	class BrimstonePitchfork : clericdamageitem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimstone Trident");
			Tooltip.SetDefault("Casts a brimstone blast that erupts on collision");
			Item.staff[item.type] = true;
		}

		public override void SafeSetDefaults()
		{
			item.damage = 60;
			item.width = item.height = 72;
			item.useTime = item.useAnimation = 37;
			item.useStyle = 5;
			item.knockBack = 1.8f;
			item.value = 10000;
			item.rare = 6;
			item.UseSound = SoundID.Item88;
			item.shootSpeed = 6;
			item.shoot = ModContent.ProjectileType<BrimstoneBlast>();
			item.noMelee = true;

			clericEvil = true;
			clericResourceCost = 11;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DevilPitchfork>());
			recipe.AddIngredient(ModContent.ItemType<Materials.Brimstone>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	class BrimstoneBlast : clericProj
	{
		public override string Texture => "Terraria/Projectile_" + ProjectileID.Fireball;

		public override void SafeSetDefaults()
		{
			projectile.width = projectile.height = 32;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.timeLeft = 180;
		}

		public override void AI()
		{
			for (var i = 0; i < 2; i++)
			{
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<BrimstoneDust>(), 0, 0);
				d.noGravity = true;
				d.scale = Main.rand.NextFloat() + 1.2f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.DoT.BrimstoneDebuff>(), 240);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item100, projectile.position);
			Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.position.Y + 24), Vector2.Zero, ModContent.ProjectileType<BrimstoneGeyser>(), (int)(projectile.damage * 0.65f), projectile.knockBack, Main.player[projectile.owner].whoAmI, projectile.damage / 100);
		}
	}

	class BrimstoneGeyser : clericProj
	{

		public override string Texture => "Terraria/Projectile_" + ProjectileID.Fireball;

		public override void SafeSetDefaults()
		{
			projectile.width = 48;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.timeLeft = 70;
			projectile.penetrate = -1;
		}

		int cooldown = 0;
		int dmg;

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.DoT.BrimstoneDebuff>(), 90);
			if (target.type != NPCID.TargetDummy)
			{
				dmg += damage;
			}
		}

		public override void AI()
		{
			if (projectile.ai[0] != 0)
			{
				dmg += (int)(projectile.ai[0] * 100);
				projectile.ai[0] = 0;
			}

			projectile.height += 3;
			projectile.position.Y -= 3;
			cooldown--;

			if (dmg > 65)
			{
				Vector2 shootVel = projectile.Center - Main.player[projectile.owner].Center;
				shootVel.Normalize();
				shootVel *= 8;
				Projectile.NewProjectile(projectile.Center, shootVel, ModContent.ProjectileType<BrimstoneSteal>(), 0, 0, Main.player[projectile.owner].whoAmI, Main.player[projectile.owner].whoAmI, 1);
				dmg -= 65;
			}

			for (var i = 0; i < 2 + Main.rand.Next(2); i++)
			{
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<BrimstoneDust>(), 0, -4);
				//d.noGravity = true;
				d.scale = Main.rand.NextFloat() + 1.7f;
				d.velocity.X *= 0.3f;
			}
		}
	}

	class BrimstoneSteal : Lifesteals.LifeStealBase
	{
		public override string Texture => "Terraria/Projectile_" + ProjectileID.Fireball;

		public override void SetDefaults()
		{
			projectile.height = projectile.width = 4;
			projectile.extraUpdates = 3;
			projectile.timeLeft = 240;
			projectile.alpha = 255;
		}
	}
	#endregion

	#region Lance

	class BrimstoneLance : clericdamageitem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimstone Ignition Javelin");
			Tooltip.SetDefault("Throws a brimstone javelin that explodes on impact \nOnly the inital hit can critically strike, with a gauranted chance");
			Item.staff[item.type] = true;
		}

		public override void SafeSetDefaults()
		{
			item.damage = 45;
			item.width = item.height = 72;
			item.useTime = item.useAnimation = 23;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 4.8f;
			item.value = 10000;
			item.rare = 6;
			item.UseSound = SoundID.Item1;
			item.shootSpeed = 10.8f;
			item.shoot = ModContent.ProjectileType<BrimstoneLanceProj>();
			item.noMelee = true;
			item.noUseGraphic = true;
			item.autoReuse = true;

			clericEvil = true;
			clericResourceCost = 8;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Materials.Brimstone>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	class BrimstoneLanceProj : clericProj
	{

		public override string Texture => "clericclass/Weapons/Hell/HardHell/BrimstoneLance";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimstone Lance");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 20;
			projectile.penetrate = 1;
			projectile.timeLeft = 999;
			projectile.friendly = true;
			//drawOffsetX = -78;
			//drawOriginOffsetX = -39;

			drawOffsetX = -33;
			drawOriginOffsetY = -33;

			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.localNPCHitCooldown = 0;
		}

		float str = 0.04f;

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
			projectile.ai[0]++;
			if (projectile.ai[0] > 20)
			{
				projectile.velocity.Y += str;
				if (str < 0.3f)
				{
					str *= 1.05f;
				}
				if  (projectile.ai[0] > 28 && (projectile.ai[0] % 8 == 0))
                {
					Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<FireBlast>(), projectile.damage / 3, 0, Main.player[projectile.owner].whoAmI);
                }
			}
		}

		public override void Kill(int timeLeft)
		{
			projectile.ai[1] += 1;

			projectile.maxPenetrate = -1;
			projectile.penetrate = -1;
			projectile.localNPCHitCooldown = -1;
			projectile.alpha = 255;
			projectile.position = projectile.Center;
			projectile.width = projectile.height = 140;
			projectile.Center = projectile.position;

			Main.PlaySound(SoundID.Item88, projectile.Center);
			projectile.Damage();

			for (var i = 0; i < 35; i++)
			{
				Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
				Dust dst = Dust.NewDustDirect(projectile.Center, 0, 0, ModContent.DustType<Dusts.BrimstoneDust>(), speed.X * 14, speed.Y * 14);
				dst.scale = Main.rand.NextFloat(1.9f, 2.5f);
				dst.noGravity = true;
				if (i > 20)
				{
					dst.scale *= 1.3f;
				}
			}
			for (var l = 0; l < 30; l++)
			{
				Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
				Dust dst2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, speed.X * 12, speed.Y * 12);
				dst2.scale = Main.rand.NextFloat(0.5f, 0.6f);
				dst2.fadeIn = Main.rand.NextFloat(0.85f, 1f);
				dst2.noGravity = true;
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (projectile.ai[1] == 0)
			{
				crit = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.DoT.BrimstoneDebuff>(), 120);
			target.AddBuff(BuffID.OnFire, 240);

			if (projectile.ai[1] == 0)
			{
				if (target.lifeMax > 5 && target.type != NPCID.TargetDummy)
				{
					Projectile.NewProjectile(projectile.Center, -projectile.velocity, ModContent.ProjectileType<BrimstoneSteal>(), 0, 0, Main.player[projectile.owner].whoAmI, Main.player[projectile.owner].whoAmI, 3);
				}
				projectile.Kill();
			}
			else if (target.lifeMax > 5 && target.type != NPCID.TargetDummy)
			{
				Projectile.NewProjectile(projectile.Center, -projectile.velocity, ModContent.ProjectileType<BrimstoneSteal>(), 0, 0, Main.player[projectile.owner].whoAmI, Main.player[projectile.owner].whoAmI, 1);
			}
		}
	}

	class FireBlast : clericProj
    {
		public override string Texture => "Terraria/Projectile_" + ProjectileID.Fireball;

        public override void SafeSetDefaults()
        {
			projectile.width = projectile.height = 20;
			projectile.timeLeft = 45;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			target.AddBuff(BuffID.OnFire, 360);
        }

        public override bool CanDamage()
        {
			if (projectile.timeLeft > 30)
			{
				return false;
			}
			return true;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 30)
            {
				Main.PlaySound(SoundID.Item88, projectile.Center);
				for (var l = 0; l < 12; l++)
				{
					Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
					Dust dst2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, speed.X * 2, speed.Y * 2);
					dst2.scale = Main.rand.NextFloat(0.5f, 0.6f);
					dst2.fadeIn = Main.rand.NextFloat(0.85f, 1f);
					dst2.noGravity = true;
				}
			}
			else if (projectile.timeLeft > 30)
            {
				Dust d = Dust.NewDustDirect(projectile.Center, 0, 0, 6, 0, 0);
				d.noGravity = true;
			}
        }
    }

    #endregion
}
