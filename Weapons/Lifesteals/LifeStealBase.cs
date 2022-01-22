using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using clericclass.ClericBase;
using clericclass.Weapons.Templates;
using System;

namespace clericclass.Weapons.Lifesteals
{
	public abstract class LifeStealBase : ModProjectile
	{

		public override bool CanDamage() => false;

		public override void AI()
		{

			int num492 = (int)projectile.ai[0];
			float num493 = 4f;
			Vector2 vector39 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
			float num494 = Main.player[num492].Center.X - vector39.X;
			float num495 = Main.player[num492].Center.Y - vector39.Y;
			float num496 = (float)Math.Sqrt((double)(num494 * num494 + num495 * num495));
			if (num496 < 50f && projectile.position.X < Main.player[num492].position.X + (float)Main.player[num492].width && projectile.position.X + (float)projectile.width > Main.player[num492].position.X && projectile.position.Y < Main.player[num492].position.Y + (float)Main.player[num492].height && projectile.position.Y + (float)projectile.height > Main.player[num492].position.Y)
			{
				if (projectile.owner == Main.myPlayer && !Main.player[Main.myPlayer].moonLeech)
				{
					int num497 = (int)projectile.ai[1];
					Player player = Main.player[num492];
					Player healer = Main.player[projectile.owner];
					// add cleric bonuses
					var modPlayer = clericmodplayer.ModPlayer(player);
					num497 += healer.GetModPlayer<modplayer>().healBonus / 2;
					if (healer.GetModPlayer<modplayer>().harvestSetBonus && Main.rand.NextBool(10))
                    {
						num497 *= 2;
                    }

					num497 = (int)Math.Round((double)num497);
					Main.player[num492].HealEffect(num497, false);
					player.statLife += num497;
					if (Main.player[num492].statLife > Main.player[num492].statLifeMax2)
					{
						Main.player[num492].statLife = Main.player[num492].statLifeMax2;
					}
					NetMessage.SendData(66, -1, -1, null, num492, (float)num497, 0f, 0f, 0, 0, 0);
				}
				projectile.Kill();
			}
			num496 = num493 / num496;
			num494 *= num496;
			num495 *= num496;
			projectile.velocity.X = (projectile.velocity.X * 15f + num494) / 16f;
			projectile.velocity.Y = (projectile.velocity.Y * 15f + num495) / 16f;
			if (projectile.type == ModContent.ProjectileType<PreBoss.Trident.BubbleBeam>())
			{
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 33);
				d.noGravity = true;
				/*
				for (int num498 = 0; num498 < 3; num498 = num3 + 1)
				{
					float num499 = projectile.velocity.X * 0.334f * (float)num498;
					float num500 = -(projectile.velocity.Y * 0.334f) * (float)num498;
					int num501 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 183, 0f, 0f, 100, default(Color), 1.1f);
					Main.dust[num501].noGravity = true;
					Dust dust3 = Main.dust[num501];
					dust3.velocity *= 0f;
					Dust dust70 = Main.dust[num501];
					dust70.position.X = dust70.position.X - num499;
					Dust dust71 = Main.dust[num501];
					dust71.position.Y = dust71.position.Y - num500;
					num3 = num498;
				}
				*/
			}
		}
	}
}