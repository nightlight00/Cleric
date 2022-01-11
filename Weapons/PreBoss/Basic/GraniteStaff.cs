using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using clericclass.ClericBase;
using clericclass.Weapons.Templates;

namespace clericclass.Weapons.PreBoss.Basic
{
    class GraniteStaff : clericdamageitem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Energy Staff");
			Tooltip.SetDefault("Conjures energy blasts");
        }

		public override void SafeSetDefaults()
		{
			item.damage = 12;
			item.width = 22;
			item.height = 70;
			item.useTime = item.useAnimation = 15;
			item.autoReuse = true;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.knockBack = 0.5f;
			item.value = 10000;
			item.rare = 0;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<GraniteProj>();
			item.noMelee = true;
			item.rare = 2;

			clericEvil = true;
			clericResourceCost = 1;
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
	}

	class GraniteProj : clericProj
    {
		public override string Texture => "Terraria/Item_" + ItemID.Mushroom;

        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Granite Energy");
        }
		public override void SafeSetDefaults()
		{
			projectile.width = projectile.height = 40;
			projectile.timeLeft = 10;
			projectile.tileCollide = false;
			projectile.penetrate = 4;
			projectile.alpha = 255;
			projectile.friendly = true;

			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.localNPCHitCooldown = 20;
		}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			var player = Main.player[projectile.owner];
			if (target.position.X < player.position.X)
			{
				hitDirection = -hitDirection;
			}
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if (target.life <= 0)
			{
				Heal(Main.player[projectile.owner], Main.player[projectile.owner], "drained", 3, true);
			}
		}

        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
				for (int i = 0; i < 30; i++)
				{
					Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
					Dust d = Dust.NewDustPerfect(projectile.Center, 132, speed * 2, Scale: 1.5f);
					d.noGravity = true;
				}
				projectile.ai[0]++;
            }
        }
    }
}
