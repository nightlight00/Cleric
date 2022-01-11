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

namespace clericclass.Weapons.PreBoss.Mushroom
{
	public class GlowingStaff :  clericdamageitem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Glowing Mushroom Staff");
			Tooltip.SetDefault("Slain foes replenish your life force");
		}

		public override void SafeSetDefaults() 
		{
			item.damage = 14;
			item.width = 30;
			item.height = 72;
			item.useTime = item.useAnimation = 32;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.knockBack = 2.3f;
			item.value = 10000;
			item.rare = 0;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<MushroomStaffProjectile>();
			item.noMelee = true;

			clericEvil = true;
			clericResourceCost = 2;
		}

		public override bool SafeCanUseItem(Player player)
		{
			bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, Main.MouseWorld, 2, 2);
			if (lineOfSight)
			{
				return true;
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectileDirect(Main.MouseWorld, new Vector2(0, 0), type, damage, knockBack, player.whoAmI, 1);
			return false;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GlowingMushroom, 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class MushroomStaff : clericdamageitem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Mushroom Staff");
			Tooltip.SetDefault("Converts damage into a regenerative aura");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 12;
			item.width = 30;
			item.height = 72;
			item.useTime = item.useAnimation = 48;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.knockBack = 0;
			item.value = 10000;
			item.rare = 0;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<MushroomStaffProjectile>();
			item.noMelee = true;

			clericEvil = false;
			item.mana = 4;
		}

		public override bool SafeCanUseItem(Player player)
		{
			bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, Main.MouseWorld, 2, 2);
			if (lineOfSight)
			{
				return true;
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectileDirect(Main.MouseWorld, new Vector2(0, 0), type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Mushroom, 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	class MushroomStaffProjectile : clericProj
	{
		public override string Texture => "clericclass/Weapons/Templates/clericProjBase";

        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Mushy Aura");
        }

        public override void SafeSetDefaults()
		{
			projectile.height = projectile.width = 192;
			projectile.timeLeft = 20;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 50;
			projectile.friendly = true;

			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.localNPCHitCooldown = 20; 
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.ai[0] == 0) // good
			{
				return false;
			}
			return true;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if (CheckKilled(target, damage, crit))
			{
				Heal(Main.player[projectile.owner], Main.player[projectile.owner], "drained", 5, true);
			}
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			var player = Main.player[projectile.owner];
			if (target.position.X < player.position.X)
			{
				hitDirection = -hitDirection;
			}
		}
		public override void AI()
		{
			var player = Main.player[projectile.owner];
			//projectile.Center = player.Center;
			projectile.alpha += 205/25;
			if (projectile.ai[0] == 0)
			{
				Buff(player, Main.LocalPlayer, BuffID.Regeneration, projectile.damage);
			//	int time = (projectile.damage + HealExtra) * 60;
			//	player.AddBuff(BuffID.Regeneration, time);
				Lighting.AddLight(projectile.Center, 2.54f/2, .67f/2, .54f/2);
			}
			else
            {
				Lighting.AddLight(projectile.Center, .95f/2, 1.10f/2, 2.55f/2);
			}
			
		}
	}
}