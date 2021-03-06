using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using clericclass.ClericBase;
using clericclass.Weapons.Templates;

namespace clericclass.Armor.Flamesilk
{

    class HolyFire : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Holy Fire");
            Description.SetDefault("Burns with wrath from above");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<modplayer>().holyFire = true;
            if (Main.rand.NextBool(5))
            {
                Dust dst = Dust.NewDustDirect(player.position, player.width, player.height, 133, Main.rand.NextFloat(-1, 1), -4, newColor: Color.Yellow);
                dst.scale = Main.rand.NextFloat(1.2f, 1.5f);
                dst.fadeIn = dst.scale * 1.3f;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<modglobalnpc>().holyfire = true;
            if (Main.rand.NextBool(5))
            {
                Dust dst = Dust.NewDustDirect(npc.position, npc.width, npc.height, 133, Main.rand.NextFloat(-1, 1), -4, newColor: Color.Yellow);
                dst.scale = Main.rand.NextFloat(1.2f, 1.5f);
                dst.fadeIn = dst.scale * 1.3f;
            }
        }
    }

    class Flameguard : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Flame Guard");
            Description.SetDefault("Taking hits counters with holy fire");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<modplayer>().flameGuard = true;
        }
    }

    class HolyFireAura : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.Fireball;
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 120;
            projectile.friendly = true;
            projectile.timeLeft = 2;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.localNPCHitCooldown = 60;
            projectile.alpha = 255;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<HolyFire>(), 240);
        }

        public override void AI()
        {
            var player = Main.player[projectile.owner];
            projectile.Center = player.Center;
            projectile.timeLeft = 2;
            if (!player.GetModPlayer<modplayer>().flamesilkSetBonus || player.dead)
            {
                projectile.Kill();
            }
            for (int i = 0; i < 30; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(player.Center + (speed * 60), 6, speed * 0);
                d.noGravity = true;
            }
        }
    }


    [AutoloadEquip(EquipType.Head)]
    class FlamesilkHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flamesilk Hat");
            Tooltip.SetDefault("7% increased radiant damage");
                              
        }

        public override void SetDefaults()
        {
            item.height = 20;
            item.width = 32;
            item.rare = 3;
            item.defense = 4;
        }

        public override bool DrawHead() => true;

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericRadientMult += 0.07f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FlamesilkCloak>() && legs.type == ModContent.ItemType<FlamesilkBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            player.setBonus = "Healing gives an additional 1 health \nNearby foes are set aflame with Holy Fire \nHealing allies gives them vengeful flames";
            // rework this into : nearby foes are inflicted with holy fire , healing allies gives them 'vengeful flames' (thorns but holy fire)
  
            player.GetModPlayer<modplayer>().flamesilkSetBonus = true;
            player.GetModPlayer<modplayer>().healBonus += 1;

            if (player.ownedProjectileCounts[ModContent.ProjectileType<HolyFireAura>()] == 0)
            {
                Projectile.NewProjectile(player.Center, new Vector2(0, -3), ModContent.ProjectileType<HolyFireAura>(), 10, 0, player.whoAmI);
            }

            if ((player.velocity.X > 4 || player.velocity.X < -4) && player.velocity.Y == 0 && Main.rand.NextBool())
            {
                Dust dst = Dust.NewDustDirect(new Vector2(player.Center.X, player.Center.Y + player.width), 0, 0, 133, 0, -2, newColor: Color.Yellow);
                dst.scale = Main.rand.NextFloat(0.8f, 1);
                dst.fadeIn = dst.scale * 1.3f;
                dst.velocity *= 0;
                dst.noGravity = true;
            }
        }
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.FlameSilk>(), 8);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class FlamesilkCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Priest's Robes");
            Tooltip.SetDefault("Healing gives an additional 2 health" +
                             "\nApplied buffs last 2 seconds longer");
        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 24;
            item.rare = 3;
            item.defense = 5;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().buffBonus += 2;
            player.GetModPlayer<modplayer>().healBonus += 2;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.FlameSilk>(), 12);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class FlamesilkBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
           // DisplayName.SetDefault("Priest's Boots");
            Tooltip.SetDefault("7% increased cleric critical strike chance");

        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 24;
            item.rare = 3;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericCrit += 7;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.FlameSilk>(), 10);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
