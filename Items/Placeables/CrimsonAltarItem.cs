using Terraria.ID;
using Terraria.ModLoader;
using AntiverseMod.Tiles.Crafting;
using Terraria;

namespace AntiverseMod.Items.Placeables
{
    public class CrimsonAltarItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crimson Altar");
			//Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = ModContent.TileType<CrimsonAltar>();
			item.rare = 1;
			item.value = Item.sellPrice(silver: 5);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Vertebrae, 12); //12 rotten chunks
			recipe.AddIngredient(ItemID.CrimstoneBlock, 12); //12 ebonstone blocks
			recipe.AddIngredient(ItemID.ViciousPowder, 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}		
	}
}
