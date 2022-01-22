using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System;

namespace clericclass.ClericBase
{
    class clericmodplayer : ModPlayer
	{
		public static clericmodplayer ModPlayer(Player player)
		{
			return player.GetModPlayer<clericmodplayer>();
		}

		// radiant is mispelled lol
		public float clericRadientAdd;
		public float clericRadientMult = 1f;
		public float clericNecroticAdd;
		public float clericNecroticMult = 1f;

		public float clericKnockback;
		public int clericCrit;

		public override void ResetEffects()
		{
			ResetVariables();
		}

		public override void UpdateDead()
		{
			ResetVariables();
		}

		private void ResetVariables()
		{
			clericRadientAdd = 0f;
			clericRadientMult = 1f;
			clericNecroticAdd = 0f;
			clericNecroticMult = 1f;

			clericKnockback = 0f;
			clericCrit = 4;
		}
	}

	public abstract class clericdamageitem : ModItem
	{
		public override bool CloneNewInstances => true;
		public int clericResourceCost = 0;
		public bool clericEvil = false;

		int resourceCostTrue;

		// Custom items should override this to set their defaults
		public virtual void SafeSetDefaults()
		{
		}

		// By making the override sealed, we prevent derived classes from further overriding the method and enforcing the use of SafeSetDefaults()
		// We do this to ensure that the vanilla damage types are always set to false, which makes the custom damage type work
		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			// all vanilla damage types must be false for custom damage types to work
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
		}

		// As a modder, you could also opt to make these overrides also sealed. Up to the modder
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			if (!clericEvil)
			{
				// radient damage, plus 25% necrotic damage
				flat += clericmodplayer.ModPlayer(player).clericRadientAdd;
				mult *= clericmodplayer.ModPlayer(player).clericRadientMult + ((clericmodplayer.ModPlayer(player).clericNecroticMult - 1) * 0.25f);
			}
			else
            {
				// same as above but in reverse
				flat += clericmodplayer.ModPlayer(player).clericNecroticAdd;
				mult *= clericmodplayer.ModPlayer(player).clericNecroticMult + ((clericmodplayer.ModPlayer(player).clericRadientMult - 1) * 0.25f);

				resourceCostTrue = (int)Math.Round((clericResourceCost - player.GetModPlayer<modplayer>().bloodCost) * (1 - player.GetModPlayer<modplayer>().bloodCostMult));
			}
		}
		
        public override void GetWeaponKnockback(Player player, ref float knockback)
		{
			// Adds knockback bonuses
			knockback += clericmodplayer.ModPlayer(player).clericKnockback;
		}

		public override void GetWeaponCrit(Player player, ref int crit)
		{
			// Adds crit bonuses
			crit += clericmodplayer.ModPlayer(player).clericCrit;
		}

        public override void HoldItem(Player player)
        {
			if (clericEvil)
            {
				player.GetModPlayer<modplayer>().currentWeaponEvil = true;
			}
			else
            {
				player.GetModPlayer<modplayer>().currentWeaponEvil = false;
			}
        }

        // Because we want the damage tooltip to show our custom damage, we need to modify it
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			// Get the vanilla damage tooltip
			var modPlayer = clericmodplayer.ModPlayer(Main.player[item.owner]);
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				// Change the tooltip text
				string damageType = " radiant ";
				float dmg = (float)Math.Round((modPlayer.clericRadientMult - 1), 2) * 100;
				float dmgOp = (float)Math.Round((modPlayer.clericNecroticMult - 1) * 0.25f, 2) * 100;
				string percent = $" ({dmg}% radiant + {dmgOp}% necrotic)";
				if (clericEvil) { 
					damageType = " necrotic ";
					dmg = (float)Math.Round((modPlayer.clericNecroticMult - 1), 2) * 100;
					dmgOp = (float)Math.Round((modPlayer.clericRadientMult - 1) * 0.25f, 2) * 100; 
					percent = $" ({dmg}% necrotic + {dmgOp}% radiant)";
				}
				
				tt.text = damageValue + damageType + damageWord + percent;
			}

			if (clericResourceCost > 0)
			{
				if (clericEvil)
				{
					string cost = "drops";
					if (resourceCostTrue <= 1) { 
						cost = "drop";
						resourceCostTrue = 1;
					}
					tooltips.Add(new TooltipLine(mod, "Cleric Resource Cost", $"Drains {resourceCostTrue} blood " + cost));
				}
			}
		}

		public virtual bool SafeCanUseItem(Player player)
		{
			return true;
		}

        public override bool CanUseItem(Player player)
		{
			if (!SafeCanUseItem(player))
            {
				return false;
            }

			if (clericEvil)
			{
				if (resourceCostTrue <= 0) { return true; }
				if (player.statLife > resourceCostTrue)
				{
					player.statLife -= resourceCostTrue;
					CombatText.NewText(player.getRect(), Color.Red, resourceCostTrue, true);
					player.AddBuff(ModContent.BuffType<Buffs.AnguishedSoul>(), (item.useTime / 3 + resourceCostTrue) * 45, false);
					return true;
				}
			}
            else
            {
				if (player.statMana >= clericResourceCost)
                {
					return true;
                }
            }
			return false;
		}
	}

}
