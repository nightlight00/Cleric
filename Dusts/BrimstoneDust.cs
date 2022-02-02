using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ModLoader.IO;
using Terraria.ID;

namespace clericclass.Dusts
{
    class BrimstoneDust : ModDust
    {
        public override void OnSpawn(Terraria.Dust dust)
        {
			dust.scale *= 0.7f;
			dust.alpha = 30;
		}

		public override bool MidUpdate(Terraria.Dust dust)
		{
			dust.scale -= Main.rand.NextFloat(0.025f, 0.03f);
			if (dust.scale < 0.2f)
			{
				dust.active = false;
			}

			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.05f;
			}

			if (!dust.noLight)
			{
				Lighting.AddLight(dust.position, .66f/3, 1.25f/3, 2.46f/3);
			}
			return false;
		}
	}
}
