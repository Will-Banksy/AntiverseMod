using Terraria.ID;
using Terraria.ModLoader;
using AntiverseMod.Tiles.Crafting;
using Terraria;

namespace AntiverseMod.Items.Placeables;

public class CrimsonAltarItem : ModItem {
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
		Item.createTile = ModContent.TileType<CrimsonAltar>();
		Item.rare = ItemRarityID.Blue;
		Item.value = Item.sellPrice(silver: 5);
	}

	public override void AddRecipes() {
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ItemID.Vertebrae, 12); //12 rotten chunks
		recipe.AddIngredient(ItemID.CrimstoneBlock, 12); //12 ebonstone blocks
		recipe.AddIngredient(ItemID.ViciousPowder, 15);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}