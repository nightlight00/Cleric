using Terraria;
using Terraria.ModLoader;
using clericclass.ClericBase;
using Microsoft.Xna.Framework;

namespace clericclass.Buffs
{
    class AnguishedSoul : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Anguished Soul");
            Description.SetDefault("Maybe performing dark arts wasn't such a good idea \nAll life regen halved");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            longerExpertDebuff = true;
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<modplayer>().anguish = true;
        }
    }
}
