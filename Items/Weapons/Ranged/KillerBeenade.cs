using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AntiverseMod.Projectiles.Ranged.BetterBeenades;
using System;

namespace AntiverseMod.Items.Weapons.Ranged;

public class KillerBeenade : ModItem {
	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 99;
	}

	public override void SetDefaults() {
		Item.damage = 28;
		Item.DamageType = DamageClass.Ranged;
		Item.maxStack = Item.CommonMaxStack;
		Item.consumable = true;
		Item.width = 22;
		Item.height = 32;
		Item.useTime = 15;
		Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.knockBack = 1;
		Item.shoot = ModContent.ProjectileType<KillerBeenadeProj>();
		Item.shootSpeed = 8f;
		Item.value = Item.sellPrice(silver: 1, copper: 50);
		Item.rare = ItemRarityID.LightRed;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = false;
		Item.useTurn = false;
		Item.noMelee = true;
		Item.noUseGraphic = true;
	}

	public override void AddRecipes() {
		const int amt = 50;
		
		Recipe recipe = CreateRecipe(amt);
		recipe.AddIngredient(ItemID.Beenade, amt);
		recipe.AddIngredient(ItemID.CobaltBar, 1);
		recipe.AddTile(TileID.Anvils);
		recipe.Register();

		recipe = CreateRecipe(amt);
		recipe.AddIngredient(ItemID.Beenade, amt);
		recipe.AddIngredient(ItemID.PalladiumBar, 1);
		recipe.AddTile(TileID.Anvils);
		recipe.Register();
	}
}