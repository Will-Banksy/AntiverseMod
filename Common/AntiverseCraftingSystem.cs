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
			.AddIngredient<Boron>(8) // TODO: Fibreglass is typically used to reinforce plastic - Do I think that the fibreglass fishing pole uses plastic? Probably. Maybe get plastic from plastic bags dropped by slimes or dolphins
			.AddTile(TileID.GlassKiln)
			.Register();
	}

	public override void AddRecipeGroups() {
		RecipeGroup hardmodeForges = new RecipeGroup(
			() => "Adamantite/Titanium Forge",
			new int[] {
				ItemID.TitaniumForge,
				ItemID.AdamantiteForge
			});
		RecipeGroup.RegisterGroup("Hardmode Forges", hardmodeForges);

		RecipeGroup hardmodeAnvils = new RecipeGroup(
			() => "Mythril/Orichalcum Anvil",
			new int[] {
				ItemID.MythrilAnvil,
				ItemID.OrichalcumAnvil
			});
		RecipeGroup.RegisterGroup("Hardmode Anvils", hardmodeAnvils);
	}
}