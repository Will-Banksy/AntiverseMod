using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AntiverseMod.Tiles.Crafting;

namespace AntiverseMod.Items.Materials;

public class Boron : ModItem {
	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 25;
	}

	public override void SetDefaults() {
		Item.width = 22;
		Item.height = 20;

		Item.maxStack = 999;
		Item.value = Item.sellPrice(silver: 12);
		Item.rare = ItemRarityID.Green;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<Sassolite>()
			.AddRecipeGroup("IronBar")
			.AddTile(TileID.Hellforge)
			.Register();
	}
}