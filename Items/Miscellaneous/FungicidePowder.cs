using AntiverseMod.Items.Materials;
using AntiverseMod.Projectiles.Miscellaneous;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AntiverseMod.Items.Miscellaneous; 

public class FungicidePowder : ModItem {
	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 25;
	}

	public override void SetDefaults() {
		Item.maxStack = Item.CommonMaxStack;
		Item.value = Item.sellPrice(copper: 20);
		Item.width = 16;
		Item.height = 24;
		Item.consumable = true;
		Item.UseSound = SoundID.Item1;
		Item.useTime = 15;
		Item.useAnimation = 15;
		Item.noMelee = true;
		Item.shoot = ModContent.ProjectileType<FungicidePowderProj>();
		Item.shootSpeed = 4f;
		Item.useStyle = ItemUseStyleID.Swing;
	}

	public override void AddRecipes() {
		CreateRecipe(5)
			.AddIngredient<Sassolite>()
			.AddTile(TileID.Bottles)
			.Register();
	}
}