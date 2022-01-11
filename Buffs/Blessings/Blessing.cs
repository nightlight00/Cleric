using Terraria;
using Terraria.ModLoader;


namespace clericclass.Buffs.Blessings
{
    class BlessingMinor : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Minor Blessing");
            Description.SetDefault("Grants a minor increase to defense and life regen");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 1;
            player.statDefense += 2; //Grant a +4 defense boost to the player while the buff is active.
        }
    }
}
