using Terraria;
using Terraria.ID;
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
            Description.SetDefault("Maybe performing dark arts wasn't such a good idea \nAll life regen halved ");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<modplayer>().anguish = true;
        }
    }


    class AnguishedSoul2 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Anguished Soul");
            Description.SetDefault("Darkness consumes at your soul, but you're used to it now \n7% reduced damage taken and all life regen halved"); 
                 //("Maybe performing dark arts wasn't such a good idea \nAll life regen halved ");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.07f;
            player.GetModPlayer<modplayer>().anguish = true;
        }
    }

    class AnguishedSoul3 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Anguished Soul");
            Description.SetDefault("Selling your soul to the dark arts was possibly a good idea \n10% reduced damage taken, 5% increased necrotic damage, all life regen halved");
            //("Maybe performing dark arts wasn't such a good idea \nAll life regen halved ");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.05f;
            player.endurance += 0.1f;
            player.GetModPlayer<modplayer>().anguish = true;
        }
    }

    class MoonRidden : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Moon Ridden");
            Description.SetDefault("You are able to absorb healing effects");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.MoonLeech] = true;
        }
    }

    class MoonBeat : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Moon Leech");
            Description.SetDefault("Taking damage replenishs some health and mana");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<modplayer>().moonLeech = true;
        }
    }
}
