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
using clericclass.Dusts;

namespace clericclass.Buffs.DoT
{
    class BrimstoneDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Brimstone Flames");
            Description.SetDefault("Even those from the farthest reaches of hell can be burned");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<modglobalnpc>().brimstoneDebuff = true;
            Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.BrimstoneDust>(), Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, -4.5f), Scale: Main.rand.NextFloat(1.7f, 2));
            if (Main.rand.NextBool(4))
            {
                d.scale *= 0.8f;
            }
            d.fadeIn = d.scale * 1.15f;
            //d.velocity.X *= 0.8f;
            d.noGravity = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<modplayer>().brimstoneDebuff = true;
            Dust d = Dust.NewDustDirect(player.position, player.width, player.height, ModContent.DustType<Dusts.BrimstoneDust>(), Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, -4.5f), newColor: Color.DarkBlue, Scale: Main.rand.NextFloat(1.7f, 2));
            if (Main.rand.NextBool(4))
            {
                d.scale *= 0.8f;
            }
            d.fadeIn = d.scale * 1.15f;
            d.noGravity = true;
        }
    }
}
