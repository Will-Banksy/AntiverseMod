using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AntiverseMod.Tiles.Crafting;

namespace AntiverseMod.Items.Materials {
	public class Boron : ModItem {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boron");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;

			Item.maxStack = 999;
			Item.value = Terraria.Item.buyPrice(silver: 2);
			Item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<Sassolite>()
				// .AddIngredient(ItemID.IronBar) // Kinda unfair to players in a world with lead instead of iron
				.AddRecipeGroup("IronBar")
				.AddTile<Forgery>()
				.Register();
		}
	}
}