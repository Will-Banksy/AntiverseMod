using Terraria.ID;
using Terraria.ModLoader;
using AntiverseMod.Tiles.Crafting;
using Terraria;

namespace AntiverseMod.Items.Placeables;

public class DemonicAltarItem : ModItem {
	public override void SetDefaults() {
		Item.width = 12;
		Item.height = 12;
		Item.maxStack = Item.CommonMaxStack;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.createTile = ModContent.TileType<DemonicAltar>();
		Item.rare = ItemRarityID.Blue;
		Item.value = Item.sellPrice(silver: 5);
	}

	public override void AddRecipes() {
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ItemID.RottenChunk, 12);
		recipe.AddIngredient(ItemID.EbonstoneBlock, 12);
		recipe.AddIngredient(ItemID.VilePowder, 15);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}