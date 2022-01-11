using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using clericclass.ClericBase;
using clericclass.Weapons.Templates;

namespace clericclass.Armor.Heartbeat
{
    [AutoloadEquip(EquipType.Head)]
    class HeartbeatMask : ModItem
    {
        public override void SetStaticDefaults()
        {
           // DisplayName.SetDefault("Flamesilk Hat");
            Tooltip.SetDefault("5% increased necrotic damage \n3% increased cleric critical strike chance");
                              
        }

        public override void SetDefaults()
        {
            item.height = 20;
            item.width = 32;
            item.rare = 3;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.05f;
            modPlayer.clericCrit += 3;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HeartbeatCloak>() && legs.type == ModContent.ItemType<HeartbeatGreaves>();
        }

        int timer = 0;
        bool heartbreak = false;
        float heartStrength = 0;
   
        public virtual string DeathMessage()
        {
            switch (Main.rand.Next(4))
            {
                default:
                    return "Is your heart broken?";
                case 1:
                    return Main.player[item.owner].name + " experienced 'Heartbreak'";
                case 2:
                    return "The true power of 'Heartbreak'";
                case 3:
                    return Main.player[item.owner].name + " died of heart not work";
            }
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            player.setBonus = "Double tap down to activate / deactivate 'Heartbreak'" +
                            "\nWhile in 'Heartbreak', necrotic damage is greatly increased while your life force suffers";

            player.GetModPlayer<modplayer>().heartSetBonus = true;
            if (player.controlDown && player.releaseDown && player.doubleTapCardinalTimer[0] < 15)
            {
                heartStrength = 0;
                heartbreak = !heartbreak;
                if (heartbreak)
                {
                    CombatText.NewText(player.getRect(), Color.Crimson, "「HEARTBREAK」");
                }
            }
            if (heartbreak)
            {
                timer++;
                modPlayer.clericNecroticMult += heartStrength;
                if (timer == 3 || timer == 9) {
                    player.statLife--;
                    if (Main.hardMode) { player.statLife--; }
                    if (player.statLife <= 0) { heartbreak = false; player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(DeathMessage()), 0, 0); }
                }
                if (timer >= 12)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X, player.Center.Y+(player.height/2)), 271, speed * 6, Scale: 1.25f);
                        d.noGravity = true;
                    }
                    if (heartStrength < 0.25f) { heartStrength += 0.01f; }
                    timer = 0;
                }
            }
              
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.LifeAlloy>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class HeartbeatCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Priest's Robes");
            Tooltip.SetDefault("7% increased necrotic damage" +
                             "\nIncreased cleric knockback");
        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 24;
            item.rare = 3;
            item.defense = 7;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.07f;
            modPlayer.clericKnockback += 0.5f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.LifeAlloy>(), 9);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class HeartbeatGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
           // DisplayName.SetDefault("Priest's Boots");
            Tooltip.SetDefault("6% increased necrotic damage");

        }

        public override void SetDefaults()
        {
            item.height = 22;
            item.width = 24;
            item.rare = 3;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.06f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.LifeAlloy>(), 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class HeartbeatProj : clericProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heartbeat");
            Main.projFrames[projectile.type] = 11;
        }
        public override void SafeSetDefaults()
        {
            projectile.width = projectile.height = 32;
            projectile.timeLeft = 300;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.friendly = true;

            
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.localNPCHitCooldown = 20;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (projectile.ai[0] > 5)
            {
                if (player.controlDown && player.releaseDown && player.doubleTapCardinalTimer[0] < 15)
                {
                    projectile.Kill();
                }
            }
            projectile.ai[0]++;
            projectile.position = new Vector2(player.Center.X, player.Center.Y - (player.height + projectile.height));
            if (++projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 11)
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}
