using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.Items.Weapons.Melee; 

public class ScrapSword : ModItem {
	public override void SetStaticDefaults() {
		Tooltip.SetDefault("Made from hastily bundled together scrap metal");
	}

	public override void SetDefaults() {
		Item.damage = 19;
		Item.DamageType = DamageClass.Melee;
		Item.width = 50;
		Item.height = 56;
		Item.useTime = 21; // Copied from Plat Broadsword
		Item.useAnimation = 19; // Copied from Plat Broadsword
		Item.useStyle = ItemUseStyleID.Swing;
		Item.knockBack = 5; // Copied from Plat Broadsword
		Item.value = Terraria.Item.sellPrice(platinum: 0, gold: 2, silver: 50, copper: 25);
		Item.rare = ItemRarityID.Blue;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.useTurn = true;
	}

	public override void AddRecipes() {
		const int amt = 5;
			
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ItemID.TinOre, amt);
		recipe.AddIngredient(ItemID.CopperOre, amt);
		recipe.AddIngredient(ItemID.IronOre, amt);
		recipe.AddIngredient(ItemID.LeadOre, amt);
		recipe.AddIngredient(ItemID.TungstenOre, amt);
		recipe.AddIngredient(ItemID.SilverOre, amt);
		recipe.AddIngredient(ItemID.GoldOre, amt);
		recipe.AddIngredient(ItemID.PlatinumOre, amt);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}