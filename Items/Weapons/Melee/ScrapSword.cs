using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.Items.Weapons.Melee;

public class ScrapSword : ModItem {
	public override void SetDefaults() {
		Item.damage = 19;
		Item.DamageType = DamageClass.Melee;
		Item.width = 50;
		Item.height = 56;
		Item.useTime = 21; // Copied from Plat Broadsword
		Item.useAnimation = 19; // Copied from Plat Broadsword
		Item.useStyle = ItemUseStyleID.Swing;
		Item.knockBack = 5; // Copied from Plat Broadsword
		Item.value = Item.sellPrice(platinum: 0, gold: 1, silver: 25, copper: 25);
		Item.rare = ItemRarityID.Blue;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.useTurn = true;
	}

	public override void AddRecipes() {
		const int amt = 2;

		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ItemID.TinBar, amt);
		recipe.AddIngredient(ItemID.CopperBar, amt);
		recipe.AddIngredient(ItemID.IronBar, amt);
		recipe.AddIngredient(ItemID.LeadBar, amt);
		recipe.AddIngredient(ItemID.TungstenBar, amt);
		recipe.AddIngredient(ItemID.SilverBar, amt);
		recipe.AddIngredient(ItemID.GoldBar, amt);
		recipe.AddIngredient(ItemID.PlatinumBar, amt);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}