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

namespace clericclass.Armor.Astrid
{

    [AutoloadEquip(EquipType.Head)]
    class AstridCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flamesilk Hat");
            Tooltip.SetDefault("7% increased radiant damage" + 
                             "\nHealing gives an additional 2 health" + 
                             "\nApplies buffs last 2 seconds longer");
                              
        }

        public override void SetDefaults()
        {
            item.height = 14;
            item.width = 22;
            item.rare = 5;
            item.defense = 5;
        }

        public override bool DrawHead() => true;
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            drawAltHair = true;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericRadientMult += 0.07f;
            player.GetModPlayer<modplayer>().healBonus += 2;
            player.GetModPlayer<modplayer>().buffBonus += 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AstridJacket>() && legs.type == ModContent.ItemType<AstridBoots>();
        }

        bool Niflheim = false;
        int timer = 0;

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            player.setBonus = "Nearby allies gain Ymir's Blessing";
          /*  player.setBonus = "Double tap down to activate 'Niflheim'" + 
                            "\nWhile 'Niflheim' is active, a storm of snow bellows from within you" +
                            "\nThe snow damages enemies and heals allies, but comes at a blood cost";
          */
            for (int i = 0; i < 30; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(player.Center + (speed * 106), 92, speed * 0, Scale: 1.5f);
                // looks pretty cool Dust d = Dust.NewDustPerfect(Main.LocalPlayer.Top, 92, speed * 53, Scale: 1.5f);
                d.noGravity = true;
            }


                player.GetModPlayer<modplayer>().healBonus += 2;
           // player.GetModPlayer<modplayer>().flamesilkSetBonus = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 8);
            recipe.AddIngredient(ModContent.ItemType<Materials.Runestone>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.AdamantiteBar, 8);
            recipe1.AddIngredient(ModContent.ItemType<Materials.Runestone>());
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Head)]
    class AstridHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flamesilk Hat");
            Tooltip.SetDefault("14% increased necrotic damage" +
                             "\nIncreased life regen");

        }

        public override void SetDefaults()
        {
            item.height = 14;
            item.width = 22;
            item.rare = 5;
            item.defense = 9;
        }

        public override bool DrawHead() => true;

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericNecroticMult += 0.14f;
            player.lifeRegen += 1;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AstridJacket>() && legs.type == ModContent.ItemType<AstridBoots>();
        }

        bool Niflheim = false;
        int timer = 0;

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            player.setBonus = "Summons in ice elemental to fight for you";
            /*  player.setBonus = "Double tap down to activate 'Niflheim'" + 
                              "\nWhile 'Niflheim' is active, a storm of snow bellows from within you" +
                              "\nThe snow damages enemies and heals allies, but comes at a blood cost";
            */

            
            if (player.ownedProjectileCounts[ModContent.ProjectileType<IceElem>()] == 0)
            {
                Projectile.NewProjectile(player.Center, new Vector2(0, -3), ModContent.ProjectileType<IceElem>(), 0, 0, player.whoAmI);
            }

            player.GetModPlayer<modplayer>().astridSetElemental = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 8);
            recipe.AddIngredient(ModContent.ItemType<Materials.Runestone>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.AdamantiteBar, 8);
            recipe1.AddIngredient(ModContent.ItemType<Materials.Runestone>());
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class AstridJacket : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Priest's Robes");
            Tooltip.SetDefault("8% increased radiant and necrotic damage");
        }

        public override void SetDefaults()
        {
            item.height = 26;
            item.width = 34;
            item.rare = 5;
            item.defense = 13;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<modplayer>().buffBonus += 2;
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericRadientMult += 0.08f;
            modPlayer.clericNecroticMult += 0.08f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 12);
            recipe.AddIngredient(ModContent.ItemType<Materials.Runestone>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.AdamantiteBar, 12);
            recipe1.AddIngredient(ModContent.ItemType<Materials.Runestone>());
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class AstridBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
           // DisplayName.SetDefault("Priest's Boots");
            Tooltip.SetDefault("8% increased cleric critical strike chance" + 
                             "\n12% increased movement speed");

        }

        public override void SetDefaults()
        {
            item.height = 18;
            item.width = 26;
            item.rare = 5;
            item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = clericmodplayer.ModPlayer(player);
            modPlayer.clericCrit += 8;
            player.moveSpeed += 0.12f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 10);
            recipe.AddIngredient(ModContent.ItemType<Materials.Runestone>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.AdamantiteBar, 10);
            recipe1.AddIngredient(ModContent.ItemType<Materials.Runestone>());
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }
}
