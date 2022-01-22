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

namespace clericclass.Weapons.BossTheme.Slime
{
	class OverseerStaff : clericdamageitem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slime Overseer Command Rod");
			Tooltip.SetDefault("Creates slime constructs to do your dirty work");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 18;
			item.width = 30;
			item.height = 72;
			item.useTime = item.useAnimation = 38;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.knockBack = 2.3f;
			item.value = 10000;
			item.rare = 1;
			item.UseSound = SoundID.Item69;
			item.shoot = ModContent.ProjectileType<SlimeConstruct>();
			item.noMelee = true;

			clericEvil = true;
			clericResourceCost = 13;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			return true;
		}
	}

	class SlimeConstruct : clericProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slime Construct");
			Main.projFrames[projectile.type] = 2;
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 26;
			projectile.height = 22;
			projectile.timeLeft = 180;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.friendly = true;
		}

		int lifeBounce = 3;
		int phaseTiles = 0;
		bool dropRuby = false;

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			if (phaseTiles > 0)
            {
				return true;
            }
			fallThrough = false;
			return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (phaseTiles > 0) { return false; }

			lifeBounce--;
			if (lifeBounce < 0) { projectile.Kill(); }
			Player player = Main.player[projectile.owner];

			Vector2 targetPos = projectile.position;
			float targetDist = 600;
			bool target = false;
			projectile.tileCollide = true;
			for (int k = 0; k < 200; k++)
			{
				NPC npc = Main.npc[k];
				if (npc.CanBeChasedBy(this, false))
				{
					float distance = Vector2.Distance(npc.Center, projectile.Center);
					if ((distance < targetDist || !target) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
					{
						targetDist = distance;
						targetPos = npc.Center;
						target = true;
					}
				}
			}
			if (target)
			{
				Vector2 shootVel = targetPos - projectile.Center;
				if (shootVel == Vector2.Zero)
				{
					shootVel = new Vector2(0f, 1f);
				}
				shootVel.Normalize();
				projectile.velocity = shootVel * 8;
				if (projectile.velocity.Y >= 0.5f) { phaseTiles = 8; lifeBounce++; }
				if (projectile.velocity.Y < 0.5 && projectile.velocity.Y > -2) { projectile.velocity.Y = -2; }
				if (projectile.velocity.Y < -10) { projectile.velocity.Y = -10; }
			}
			else
			{
				projectile.velocity = new Vector2(projectile.velocity.X + Main.rand.NextFloat(-2, 2), -5);
			}
			Main.PlaySound(SoundID.NPCHit1, projectile.position);
			for (int i = 0; i < 8; i++)
            {
				Dust dst = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 4, Main.rand.NextFloat(-2, 2), -2);
				dst.scale = Main.rand.NextFloat(0.8f, 1);
				dst.color = new Color(0, 155, 255);
			}
			return false;
        }

		public override bool? CanCutTiles() => false;

        public override void Kill(int timeLeft)
        {
			Main.PlaySound(SoundID.NPCDeath1, projectile.position);
			for (int i = 0; i < 20; i++)
			{
				Dust dst = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 4, Main.rand.NextFloat(-2, 2), -2);
				dst.scale = Main.rand.NextFloat(0.8f, 1.1f);
				dst.color = new Color(0, 155, 255);
			}
			if (!dropRuby) { return; }
			Item.NewItem(projectile.getRect(), ModContent.ItemType<Pickups.RubyHeart>());
        }

        public override void AI()
		{
			phaseTiles--;
			projectile.timeLeft = 2;
			projectile.velocity.Y += 0.12f;
			if (++projectile.frameCounter >= 5)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 2)
				{
					projectile.frame = 0;
				}
			}
			if (projectile.velocity.X > 0) { projectile.spriteDirection = 1; }
            else { projectile.spriteDirection = -1; }
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			dropRuby = true;
        }
    }
}