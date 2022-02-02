using clericclass.ClericBase;
using clericclass.Weapons.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace clericclass.Weapons.Hell
{
    class DevilPitchfork : clericdamageitem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Devilish Pitchfork");
			Tooltip.SetDefault("Conjures a lava geyser at the cursor");
			Item.staff[item.type] = true;
		}

		public override void SafeSetDefaults()
		{
			item.damage = 35;
			item.width = item.height = 66;
			item.useTime = item.useAnimation = 26;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 1.8f;
			item.value = 10000;
			item.rare = 3;
			item.UseSound = SoundID.Item88;
			item.shootSpeed = 0.1f;
			item.shoot = ModContent.ProjectileType<FirePillar>();
			item.noMelee = true;

			clericEvil = true;
			clericResourceCost = 8;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			//Projectile.NewProjectile(worldX, worldY - pushYUp, 0f, 0f, type, damage, knockBack, Main.myPlayer);
			Projectile.NewProjectileDirect(new Vector2(worldX, worldY), Vector2.Zero, type, damage, knockBack, player.whoAmI, 0);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Weapons.PreBoss.Trident.GoldenTrident>());
			recipe.AddIngredient(ModContent.ItemType<Materials.FlameSilk>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ModContent.ItemType<Weapons.PreBoss.Trident.PlatinumTrident>());
			recipe1.AddIngredient(ModContent.ItemType<Materials.FlameSilk>(), 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.SetResult(this);
			recipe1.AddRecipe();
		}
	}

	class FirePillar : clericProj
    {
		public override string Texture => "Terraria/Projectile_" + ProjectileID.Fireball;

		public override void SafeSetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.timeLeft = 50;
			projectile.penetrate = -1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 90);
		}

		public override void AI()
		{
			projectile.height += 3;
			projectile.position.Y -= 3;

			for (var i = 0; i < 2; i++)
			{
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0, -3);
				d.noGravity = true;
				d.scale = Main.rand.NextFloat() + 1.7f;
				d.velocity.X *= 0.3f;
			}
		}
	}

	class DevilPitchfork2 : clericProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Devilish Pitchfork");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			Main.projFrames[projectile.type] = 2;
		}

		public override void SafeSetDefaults()
		{
			projectile.height = 84;
			projectile.width = 30;
			projectile.timeLeft = 50;
			projectile.penetrate = 6;
			projectile.tileCollide = false;
			projectile.friendly = true;

			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.localNPCHitCooldown = 20;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void AI()
		{
			if (projectile.ai[0] == 1)
            {
				projectile.frame = 1;
				projectile.timeLeft -= 1;
				projectile.alpha = 100;
				Lighting.AddLight(projectile.Center, new Vector3(1.23f, .26f, 2.20f));
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height / 2, 27);
            }
			else {
				projectile.frame = 0;
				projectile.alpha += 5;
				Lighting.AddLight(projectile.Center, new Vector3(2.55f, 1.23f, .15f));
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height / 2, 6);
				d.scale += 0.25f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.ai[0] == 1)
			{
				target.AddBuff(BuffID.ShadowFlame, 120);
				for (int i = 0; i < 15; i++)
				{
					Dust dst = Dust.NewDustDirect(target.position, target.width, target.height, 27, Main.rand.NextFloat(-4, 4), -5);
					dst.scale = Main.rand.NextFloat(0.9f, 1.3f);
				}
				return;
			}
			Main.PlaySound(SoundID.Item100, projectile.position);
			for (int i = 0; i < 15; i++)
			{
				Dust dst = Dust.NewDustDirect(target.position, target.width, target.height, 6, Main.rand.NextFloat(-4, 4), -5);
				dst.scale = Main.rand.NextFloat(1.1f, 1.6f);
			}
			var player = Main.player[projectile.owner];
			for (var i = 0; i < 2; i++)
			{
				Vector2 pos = new Vector2(target.position.X + Main.rand.NextFloat(-projectile.width * 2, projectile.width * 2), target.position.Y + (target.height * 1.5f));
				Projectile.NewProjectileDirect(pos, new Vector2(0, -15), ModContent.ProjectileType<DevilPitchfork2>(), (int)(projectile.damage * 0.66f), projectile.knockBack, player.whoAmI, 1);
				for (int d = 0; d < 15; d++)
				{
					Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 0.5f);
					Dust dst = Dust.NewDustDirect(pos + (speed * 3), target.width, target.height, 27, 0, 0);
					dst.scale = Main.rand.NextFloat(1.1f, 1.6f);
					dst.noGravity = true;
				}
			}
		}
	}
}
