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

namespace clericclass.Armor.Astrid
{
    class IceElem : clericProj
    {
        public override string Texture => "Terraria/NPC_" + NPCID.IceElemental;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Elemental");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SafeSetDefaults()
        {
            projectile.height = 44;
            projectile.width = 22;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        public override bool CanDamage() => false;

        public override bool? CanCutTiles() => false;

		float idleAccel = 0.05f;
		float viewDist = 400f;
		float chaseDist = 200f;
		float chaseAccel = 6f;
		float inertia = 40f;
		float shootCool = 90f;
		float shootSpeed = 5;
		int shoot = ProjectileID.SnowBallFriendly;
		int atk = 0;

		public override void AI()
        {
			if (++projectile.frameCounter >= 5)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 2)
				{
					projectile.frame = 0;
				}
			}
			Player player = Main.player[projectile.owner];

			if (player.dead || !player.GetModPlayer<modplayer>().astridSetElemental)
			{
				projectile.Kill();
            }
			projectile.timeLeft = 2;
			if (Main.rand.NextBool())
			{
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 92, Scale: 1.4f);
				d.noGravity = true;
				d.velocity *= 0;
			}

			float spacing = (float)projectile.width;
			for (int k = 0; k < 1000; k++)
			{
				Projectile otherProj = Main.projectile[k];
				if (k != projectile.whoAmI && otherProj.active && otherProj.owner == projectile.owner && otherProj.type == projectile.type && Math.Abs(projectile.position.X - otherProj.position.X) + Math.Abs(projectile.position.Y - otherProj.position.Y) < spacing)
				{
					if (projectile.position.X < Main.projectile[k].position.X)
					{
						projectile.velocity.X -= idleAccel;
					}
					else
					{
						projectile.velocity.X += idleAccel;
					}
					if (projectile.position.Y < Main.projectile[k].position.Y)
					{
						projectile.velocity.Y -= idleAccel;
					}
					else
					{
						projectile.velocity.Y += idleAccel;
					}
				}
			}
			Vector2 targetPos = projectile.position;
			float targetDist = viewDist;
			bool target = false;

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
			
			if (Vector2.Distance(player.Center, projectile.Center) > (target ? 1000f : 500f))
			{
				projectile.ai[0] = 1f;
				projectile.netUpdate = true;
			}
			if (target && projectile.ai[0] == 0f)
			{
				//Vector2 direction = targetPos - projectile.Center;
				Vector2 direction = ((targetPos - projectile.Center) + (player.Center - projectile.Center)) / 2;
				if (direction.Length() > chaseDist)
				{
					direction.Normalize();
					projectile.velocity = (projectile.velocity * inertia + direction * chaseAccel) / (inertia + 1);
				}
				else
				{
					projectile.velocity *= (float)Math.Pow(0.97, 40.0 / inertia);
				}
			}
			else
			{
				if (!Collision.CanHitLine(projectile.Center, 1, 1, player.Center, 1, 1))
				{
					projectile.ai[0] = 1f;
				}
				float speed = 6f;
				if (projectile.ai[0] == 1f)
				{
					speed = 15f;
				}
				Vector2 center = projectile.Center;
				Vector2 direction = player.Center - center;
				projectile.ai[1] = 3600f;
				projectile.netUpdate = true;
				direction.Y -= player.height * 2;
				float distanceTo = direction.Length();
				if (distanceTo > 200f && speed < 9f)
				{
					speed = 9f;
				}
				if (distanceTo < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.ai[0] = 0f;
					projectile.netUpdate = true;
				}
				if (distanceTo > 2000f)
				{
					projectile.Center = player.Center;
				}
				if (distanceTo > 48f)
				{
					direction.Normalize();
					direction *= speed;
					float temp = inertia / 2f;
					projectile.velocity = (projectile.velocity * temp + direction) / (temp + 1);
				}
				else
				{
					projectile.direction = Main.player[projectile.owner].direction;
					projectile.velocity *= (float)Math.Pow(0.9, 40.0 / inertia);
				}
			}
			projectile.rotation = projectile.velocity.X * 0.13f;
			if (projectile.velocity.X > 0f)
			{
				projectile.spriteDirection = projectile.direction = -1;
			}
			else if (projectile.velocity.X < 0f)
			{
				projectile.spriteDirection = projectile.direction = 1;
			}
			if (projectile.ai[1] > 0f)
			{
				projectile.ai[1] += 1f;
				if (Main.rand.NextBool(3))
				{
					projectile.ai[1] += 1f;
				}
			}
			if (projectile.ai[1] > shootCool)
			{
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
			}
			if (projectile.ai[0] == 0f)
			{
				if (target)
				{
					if ((targetPos - projectile.Center).X > 0f)
					{
						projectile.spriteDirection = projectile.direction = -1;
					}
					else if ((targetPos - projectile.Center).X < 0f)
					{
						projectile.spriteDirection = projectile.direction = 1;
					}
					if (projectile.ai[1] == 0f)
					{
						projectile.ai[1] = 1f;
						if (Main.myPlayer == projectile.owner)
						{
							Vector2 shootVel = targetPos - projectile.Center;
							float distanceTo = shootVel.Length();
							Main.PlaySound(SoundID.NPCHit5, projectile.position);
							if (distanceTo < 150 || Main.rand.NextBool(8)){
								for (var i = 0; i < 4; i++)
                                {
									ClericProjectile(player, projectile.Center, shootVel, ModContent.ProjectileType<IceShot>(), 18, 1, false, 2, i);
								}
								shootCool = 90;
                            }
							else if (distanceTo > 350 || Main.rand.NextBool(4)) // frost beam
                            {
								if (shootVel == Vector2.Zero)
								{
									shootVel = new Vector2(0f, 1f);
								}
								shootVel.Normalize();
								shootVel *= 8.5f;
								ClericProjectile(player, projectile.Center, shootVel, ModContent.ProjectileType<IceShot>(), 28, 4, false, 1);
								shootCool = 70;
							}
							else // snowball shotgun
							{
								for (var i = 0; i < 5; i++)
								{
									shootVel = targetPos - projectile.Center;
									if (shootVel == Vector2.Zero)
									{
										shootVel = new Vector2(0f, 1f);
									}
									shootVel.Normalize();
									shootVel = shootVel.RotatedByRandom(MathHelper.ToRadians(16));
									shootVel *= 6.5f + Main.rand.NextFloat(3);
									ClericProjectile(player, projectile.Center, shootVel, ModContent.ProjectileType<IceShot>(), 15, 1, false);
								}
								shootCool = 120;
							}
						}
					}
					else
                    {
						shootCool = 90;
                    }
				}
			}
		}
    }

	class IceShot : clericProj
    {
		public override string Texture => "Terraria/Projectile_" + ProjectileID.SnowBallFriendly;

        public override void SafeSetDefaults()
        {
			projectile.width = projectile.height = 12;
			projectile.friendly = true;
			projectile.timeLeft = 120;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.ai[0] != 0 && Main.rand.NextBool())
            {
				target.AddBuff(BuffID.Frostburn, 180);
            }
        }

		public override void Kill(int timeLeft)
		{
			if (projectile.ai[0] == 0)
			{
				Main.PlaySound(SoundID.Item51, projectile.position);
			}
		}

		Vector2 pos;
		float rot;
		float dist = 16;

        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
				if (projectile.timeLeft < 95)
                {
					projectile.velocity.Y += 0.15f;
                }
				projectile.rotation = projectile.velocity.ToRotation();
            }
			else
            {
				projectile.extraUpdates = 2;
				for (var i = 0; i < 2; i++)
				{
					Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 92);
					d.noGravity = true;
					d.velocity *= 0;
					d.scale = 1.25f + (Main.rand.NextFloat() * 0.4f);
				}
				projectile.alpha = 255;
            }
			if (projectile.ai[0] == 2)
            {
				if (projectile.ai[1] != 5)
                {
					rot = MathHelper.ToRadians(projectile.ai[1] * 90);
					pos = projectile.position;
					projectile.ai[1] = 5;
				}
				rot += 0.25f;
				dist += 0.75f;
				projectile.position = pos;
				projectile.position.X += (float)(Math.Cos(rot) * dist);
				projectile.position.Y += (float)(Math.Sin(rot) * dist);
			}
        }
    }
}
