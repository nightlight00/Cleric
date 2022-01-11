using clericclass.ClericBase;
using clericclass.Weapons.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace clericclass.Weapons.PreBoss.Trident
{
    class GoldenTrident : clericdamageitem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Trident");
			Tooltip.SetDefault("Conjures a trident to rise from the ground"
					       + "\nStruck foes create a restorative bubble");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.width = 30;
			item.height = 86;
			item.useTime = item.useAnimation = 26;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.knockBack = 1.8f;
			item.value = 10000;
			item.rare = 0;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<GoldTridentProj>();
			item.noMelee = true;

			clericEvil = false;
			item.mana = 4;
//			clericResourceCost = 4;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			//Projectile.NewProjectile(worldX, worldY - pushYUp, 0f, 0f, type, damage, knockBack, Main.myPlayer);
			Projectile.NewProjectileDirect(new Vector2(worldX, worldY + (pushYUp * 3)), new Vector2(0, -9), type, damage, knockBack, player.whoAmI, 1);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GoldBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	class PlatinumTrident : clericdamageitem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Trident");
			Tooltip.SetDefault("Conjures a trident to rise from the ground"
						   + "\nStruck foes create a restorative bubble");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 11;
			item.width = 30;
			item.height = 86;
			item.useTime = item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.knockBack = 1.8f;
			item.value = 10000;
			item.rare = 0;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<PlatinumTridentProj>();
			item.noMelee = true;

			clericEvil = false;
			item.mana = 4;
//			clericResourceCost = 4;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			//Projectile.NewProjectile(worldX, worldY - pushYUp, 0f, 0f, type, damage, knockBack, Main.myPlayer);
			Projectile.NewProjectileDirect(new Vector2(worldX, worldY + (pushYUp * 3)), new Vector2(0, -9), type, damage, knockBack, player.whoAmI, 1);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PlatinumBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	class Bubble : clericProj
    {
		public override void SafeSetDefaults()
		{
			projectile.height = 24;
			projectile.width = 24;
			projectile.timeLeft = 120;
			projectile.penetrate = 3;
			projectile.tileCollide = false;

			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.localNPCHitCooldown = 20;
		}
        public override void AI()
        {
			projectile.velocity *= 0.94f;
			HealCollision(Main.player[projectile.owner], Main.LocalPlayer, "restored", 1, false, 45, true);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
				Dust dst = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 33, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }
    }

	class PlatinumTridentProj : clericProj
	{
		public override string Texture => "clericclass/Weapons/PreBoss/Trident/PlatinumTrident";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Trident");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SafeSetDefaults()
		{
			projectile.height = 86;
			projectile.width = 30;
			projectile.timeLeft = 45;
			projectile.penetrate = 3;
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
			projectile.alpha += 6;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 15; i++)
			{
				Dust dst = Dust.NewDustDirect(target.position, target.width, target.height, 33, Main.rand.NextFloat(-4, 4), -5);
				dst.scale = Main.rand.NextFloat(1.1f, 1.6f);
			}
			if (target.lifeMax > 5 && target.type != NPCID.TargetDummy && !target.friendly)
			{
				Projectile.NewProjectile(target.Center, projectile.velocity / 2, ModContent.ProjectileType<Bubble>(), 0, 0, Main.player[projectile.owner].whoAmI);
			}
		}
	}

	class GoldTridentProj : clericProj
    {
		public override string Texture => "clericclass/Weapons/PreBoss/Trident/GoldenTrident";

        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Gold Trident");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SafeSetDefaults()
		{
			projectile.height = 86;
			projectile.width = 30;
			projectile.timeLeft = 45;
			projectile.penetrate = 3;
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
			projectile.alpha += 6;
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 15; i++)
            {
				Dust dst = Dust.NewDustDirect(target.position, target.width, target.height, 33, Main.rand.NextFloat(-4, 4), -5);
				dst.scale = Main.rand.NextFloat(1.1f, 1.6f);
			}
			if (target.lifeMax > 5 && target.type != NPCID.TargetDummy && !target.friendly)
			{
				Projectile.NewProjectile(target.Center, projectile.velocity/2, ModContent.ProjectileType<Bubble>(), 0, 0, Main.player[projectile.owner].whoAmI);
			}
		}
    }
}
