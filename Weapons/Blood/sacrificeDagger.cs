using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using clericclass.ClericBase;
using clericclass.Weapons.Templates;

namespace clericclass.Weapons.Blood
{
    class SacrificeDagger : clericdamageitem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Sacrifical Dagger");
			Tooltip.SetDefault("dev note : get new sprite"
						   + "\nConverts your blood into damaging droplets");
						//   + "\nRight click to perform a life-stealing stab"); ;
        }
        public override void SafeSetDefaults()
		{
			item.damage = 17;
			item.width = 30;
			item.height = 34;
			item.useTime = item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 2.3f;
			item.value = 10000;
			item.rare = 1;
			item.UseSound = SoundID.NPCDeath21;
			item.shoot = ModContent.ProjectileType<BloodDrop>();
			item.shootSpeed = 9;
			item.noMelee = true;
			item.noUseGraphic = true;

			clericEvil = true;
			clericResourceCost = 6;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			for (var i = 0; i < 3; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(18));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale; 
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
        }
    }

	class BloodDrop : clericProj
    {
		public override string Texture => "Terraria/Projectile_" + ProjectileID.WaterBolt;

        public override void SetDefaults()
        {
			projectile.height = projectile.width = 12;
			projectile.friendly = true;
			projectile.timeLeft = 140;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			for (var i = 0; i < 15; i++)
			{
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 5, ((-projectile.velocity.X*1.2f)/15)*i, ((-projectile.velocity.Y * 1.2f)/15)*i);
				d.scale = Main.rand.NextFloat(1.25f, 1.8f);
			}
			if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy) 
			{
				int heal = 1;
				if (crit) { heal++; }
				Projectile.NewProjectile(target.position, -projectile.velocity, ProjectileID.VampireHeal, 0, 0, Main.player[projectile.owner].whoAmI, 0, heal);
			}
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			for (var i = 0; i < 15; i++)
			{
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 5, ((-projectile.velocity.X * 1.2f) / 15) * i, ((-projectile.velocity.Y * 1.2f) / 15) * i);
				d.scale = Main.rand.NextFloat(1.25f, 1.8f);
			}
			return true;
        }

        public override void AI()
        {
			if (projectile.timeLeft < 120)
			{
				projectile.velocity.Y += 0.1f;
			}
            for (var i = 0; i < 2; i++)
            {
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 5, -projectile.velocity.X * 0.75f, -projectile.velocity.Y * 0.75f);
				d.scale = Main.rand.NextFloat(0.9f, 1.2f);
				d.velocity *= 0.3f;
            }
        }
    }

	class daggerThrust : clericProj
	{

		public override string Texture => "clericclass/Weapons/Blood/SacrificeDagger";

		public override void SafeSetDefaults()
		{
			projectile.width = projectile.height = 32;
			projectile.friendly = true;
			projectile.timeLeft = 30;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;

			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.localNPCHitCooldown = 20;
		}

        public override bool? CanHitNPC(NPC target)
        {
			return hit;
        }

        bool hit = true;

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			var player = Main.player[projectile.owner];
			crit = true;
			Heal(player, player, "drained", 5);
			hit = false;
		}	

		public override void AI()
		{
			var player = Main.player[projectile.owner];
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
			projectile.direction = player.direction;
			player.heldProj = projectile.whoAmI;
			projectile.Center = vector;

			projectile.spriteDirection = (projectile.direction = player.direction);
			projectile.alpha -= 127;
			if (projectile.alpha < 0)
			{
				projectile.alpha = 0;
			}
			if (projectile.localAI[0] > 0f)
			{
				projectile.localAI[0] -= 1f;
			}
			float num = (float)player.itemAnimation / (float)player.itemAnimationMax;
			float num2 = 1f - num;
			float num3 = projectile.velocity.ToRotation();
			float num4 = projectile.velocity.Length();
			float num5 = 22f;
			Vector2 spinningpoint = new Vector2(1f, 0f).RotatedBy((double)(3.14159274f + num2 * 6.28318548f), default(Vector2)) * new Vector2(num4, projectile.ai[0]);
			projectile.position += spinningpoint.RotatedBy((double)num3, default(Vector2)) + new Vector2(num4 + num5, 0f).RotatedBy((double)num3, default(Vector2));
			Vector2 destination = vector + spinningpoint.RotatedBy((double)num3, default(Vector2)) + new Vector2(num4 + num5 + 40f, 0f).RotatedBy((double)num3, default(Vector2));
			projectile.rotation = player.AngleTo(destination) + 0.7853982f * (float)player.direction;
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation += 3.14159274f;
			}
			player.DirectionTo(projectile.Center);
		}
    }
}
