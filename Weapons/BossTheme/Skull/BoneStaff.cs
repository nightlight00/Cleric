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

namespace clericclass.Weapons.BossTheme.Skull
{
    class BoneMarrow : clericdamageitem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Skull");
			Tooltip.SetDefault("Conjures a tombstone that animates cursed skulls");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 26;
			item.width = 30;
			item.height = 72;
			item.useTime = item.useAnimation = 41;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.knockBack = 2.3f;
			item.value = 10000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<Tombstone>();
			item.noMelee = true;

			clericEvil = true;
			clericResourceCost = 8;
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
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			//Projectile.NewProjectile(worldX, worldY - pushYUp, 0f, 0f, type, damage, knockBack, Main.myPlayer);
			Projectile.NewProjectileDirect(new Vector2(worldX, worldY - pushYUp/2), new Vector2(0, 0), type, damage, knockBack, player.whoAmI, 1);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bone, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	class Tombstone : clericProj
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeleton Tomb");
		}

		public override void SafeSetDefaults()
		{
			projectile.height = 38;
			projectile.width = 26;
			projectile.timeLeft = 200;
			projectile.penetrate = -1;

			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.localNPCHitCooldown = 20;
		}

		public override bool CanDamage() => false;

		int timer = 0;

        public override void AI()
        {
			Vector2 targetCenter = projectile.position;
			bool foundTarget = false;
			float distance = 700;

			if (!foundTarget)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy())
					{
						float npcDistance = Vector2.Distance(npc.Center, projectile.Center);
						bool closest = Vector2.Distance(projectile.Center, targetCenter) > npcDistance;

						Vector2 newMove = Main.npc[i].Center - projectile.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);

						if (closest || !foundTarget && distanceTo < distance)
						{
							// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
							// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
							bool closeThroughWall = npcDistance < 100f;
							bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);

							if (lineOfSight || closeThroughWall)
							{
								targetCenter = npc.Center;
								foundTarget = true;
							}
						}
					}
				}
			}
			if (Main.rand.Next(4) == 0) { timer++; } 
			timer++;
			if (timer >= 40 && foundTarget)
            {
				for (int i = 0; i < 10; i++)
				{
					Dust dst = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 26, Main.rand.NextFloat(-3, 3), -2);
					dst.scale = Main.rand.NextFloat(1, 1.4f);
				}
				Vector2 shootVel = targetCenter - projectile.Center;
				if (shootVel == Vector2.Zero)
				{
					shootVel = new Vector2(0f, 1f);
				}
				shootVel.Normalize();
				shootVel *= 6;
				var player = Main.player[projectile.owner];
				Projectile proj = Projectile.NewProjectileDirect(new Vector2(projectile.Center.X, projectile.position.Y-4), shootVel, ModContent.ProjectileType<TombSkull>(), projectile.damage, projectile.knockBack, player.whoAmI);
				timer = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
				Dust dst = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 26, Main.rand.NextFloat(-3, 3), -2);
				dst.scale = Main.rand.NextFloat(1, 1.4f);
            }
        }
    }

    class TombSkull : clericProj
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SafeSetDefaults()
		{
			projectile.height = 30;
			projectile.width = 26;
			projectile.timeLeft = 120;
			projectile.penetrate = 1;
			projectile.friendly = true;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (CheckKilled(target, damage, crit))
            {
				var steal = target.lifeMax / 35;
				if (steal > 6) { steal = 6; }
				var player = Main.player[projectile.owner];
				Heal(player, player, "stolen", steal, true);
            }
        }

        public override void AI()
        {
			Dust dst = Dust.NewDustDirect(new Vector2(projectile.Center.X, projectile.Center.Y), 4, 4, 5, -projectile.velocity.X/2, -projectile.velocity.Y/2);

			projectile.direction = projectile.spriteDirection = projectile.velocity.X > 0f ? 1 : -1;
			projectile.rotation = projectile.velocity.ToRotation();
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation -= MathHelper.Pi;
			}

			Vector2 move = Vector2.Zero;
			float distance = 400f;
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
		}
		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 6f / magnitude;
			}
		}
	}
}
