using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AntiverseMod.Tiles.Crafting;

namespace AntiverseMod.Items.Placeables;

public class ForgeryItem : ModItem {
	public override void SetDefaults() {
		Item.width = 30;
		Item.height = 20;

		Item.maxStack = 999;
		Item.consumable = true;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useTurn = true;
		Item.autoReuse = true;

		Item.rare = ItemRarityID.Lime;
		Item.value = Item.buyPrice(gold: 2);

		Item.createTile = ModContent.TileType<Forgery>();
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddRecipeGroup("Hardmode Forges")
			.AddRecipeGroup("Hardmode Anvils")
			.AddIngredient(ItemID.LihzahrdBrick, 20)
			// .AddIngredient(ItemID.RedBrick, 25)
			.AddTile(TileID.LihzahrdFurnace)
			.Register();
	}
}