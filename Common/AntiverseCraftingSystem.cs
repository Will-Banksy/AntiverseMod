using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AntiverseMod.Items.Materials;
using AntiverseMod.Tiles.Crafting;

namespace AntiverseMod.Common;

public class AntiverseCraftingSystem : ModSystem {
	public override void AddRecipes() {
		Recipe.Create(ItemID.FiberglassFishingPole)
			.AddIngredient(ItemID.Glass, 50)
			.AddIngredient<Sassolite>(8)
			.AddTile(TileID.GlassKiln)
			.Register();

		Recipe.Create(ItemID.TempleKey)
			.AddIngredient<TempleKeyMold>()
			.AddIngredient(ItemID.GoldenKey)
			.AddIngredient(ItemID.Ectoplasm, 15)
			.AddIngredient(ItemID.SoulofFright, 5)
			.AddIngredient(ItemID.SoulofMight, 5)
			.AddIngredient(ItemID.SoulofSight, 5)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}

	public override void AddRecipeGroups() {
		RecipeGroup hardmodeForges = new RecipeGroup(
			() => "Adamantite/Titanium Forge",
			new int[] { ItemID.TitaniumForge, ItemID.AdamantiteForge });
		RecipeGroup.RegisterGroup("Hardmode Forges", hardmodeForges);

		RecipeGroup hardmodeAnvils = new RecipeGroup(
			() => "Mythril/Orichalcum Anvil",
			new int[] { ItemID.MythrilAnvil, ItemID.OrichalcumAnvil });
		RecipeGroup.RegisterGroup("Hardmode Anvils", hardmodeAnvils);
	}
}
