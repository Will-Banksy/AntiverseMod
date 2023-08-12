using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AntiverseMod.Tiles;
using AntiverseMod.Items.Materials;
using AntiverseMod.Tiles.Crafting;

// TODO: Use this for things. Armour maybe. A shield maybe. Maybe it makes an explosion-proof block.

namespace AntiverseMod.Items.Placeables; 

public class BoronCarbide : ModItem {
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Boron Carbide");
		ItemID.Sets.SortingPriorityMaterials[Item.type] = 64; // TODO: 59 is Platinum bar, higher is more valuable. Settle on a score for this
	}

	public override void SetDefaults()
	{
		Item.width = 52;
		Item.height = 42;

		Item.maxStack = 99;
		// Item.consumable = true;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useTurn = true;
		Item.autoReuse = true;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(silver: 4);

		// Item.createTile = ModContent.TileType<MetalBars>();
		// Item.placeStyle = 0;
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient<Boron>(2)
			.AddIngredient(ItemID.AshBlock, 5)
			.AddTile<Forgery>()
			.Register();
	}
}