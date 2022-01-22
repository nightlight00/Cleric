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

namespace clericclass.Buffs.Charms
{
    class GourdDefense : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Gourd Morning!");
            Description.SetDefault("Strenghtens your defense by the power of gourds! \nDefense increased by 2");
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 2;
        }
    }

    class SummerSpirit : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Summer Spirit");
            Description.SetDefault("Feels like a pleasent summer day \nIncreased life regen, 10% increased movement speed, and 3 additional defense \nGetting healed grants extra health");
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<modplayer>().summerBuff = true;
            player.lifeRegen += 2;
            player.moveSpeed += 0.1f;
            player.statDefense += 3;
        }
    }
}
