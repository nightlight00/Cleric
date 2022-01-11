using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using clericclass.ClericBase;
using clericclass.Weapons.Templates;

namespace clericclass.Pickups
{
    class RubyHeart : ModItem
    {
        public override string Texture => "Terraria/Item_" + ItemID.Ruby;

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Heart);
            item.alpha = 100;
        }
        public override bool CanPickup(Player player)
        {
            return true;
        }

        public override bool OnPickup(Player player)
        {
            CombatText.NewText(player.getRect(), Color.Lime, 3);
            Main.PlaySound(SoundID.Grab, player.position);
            player.statLife += 3;
            if (player.statLife > player.statLifeMax2) { player.statLife = player.statLifeMax2; }
            item.TurnToAir();
            return true;
        }
    }
}
