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
	class WoodenCross : clericdamageitem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wooden Staff");
			Tooltip.SetDefault("dev note : heals to many times, fix or broken" + "\nCasts a light wave that increases in strength as it travels" +
							 "\nReplenishes health equal to the amount of enemies hit");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 8;
			item.width = 26;
			item.height = 54;
			item.useTime = item.useAnimation = 29;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.knockBack = 0;
			item.value = 10000;
			item.rare = 0;
			item.UseSound = SoundID.Item43;
			item.shoot = ModContent.ProjectileType<WoodenBeam>();
			item.noMelee = true;

			clericEvil = false;
			item.mana = 2;
			//clericResourceCost = 2;
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
			Projectile.NewProjectileDirect(Main.MouseWorld, new Vector2(0,10), type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	class WoodenBeam : clericProj
	{
		public override string Texture => "clericclass/Weapons/Templates/HolyBeam";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Beam");
		}

		public override void SafeSetDefaults()
		{
			projectile.height = 58;
			projectile.width = 128;
			projectile.timeLeft = 25;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 50;
			projectile.friendly = true;
			
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.localNPCHitCooldown = 20;
		}

		int timer = 0;
		int healAmount = 0;

        public override void AI()
        {
			timer++;
			if (timer == 4 || timer == 2)
            {
				//projectile.hostile = true;
				if (timer == 4)
				{
					projectile.damage += 1;
					timer = 0;
				}
            }
			if (healAmount != 0)
			{
				HealCollision(Main.player[projectile.owner], Main.LocalPlayer, "stolen", healAmount);
			}
			Lighting.AddLight(projectile.Center, 1.275f*1.5f, 1.275f*1.5f, 1.275f*1.5f);
		}

        public override void AfterHeal()
        {
			healAmount = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if (target.lifeMax > 5 && target.type != NPCID.TargetDummy)
			{
				healAmount++;
			}
        }
    }
}
