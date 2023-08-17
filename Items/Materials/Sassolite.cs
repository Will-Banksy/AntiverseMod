using Terraria.ModLoader;
using Terraria.ID;

namespace AntiverseMod.Items.Materials;

public class Sassolite : ModItem {
	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 25;
	}

	public override void SetDefaults() {
		Item.width = 26;
		Item.height = 20;

		Item.maxStack = 999;
		Item.value = Terraria.Item.sellPrice(copper: 20);
		Item.rare = ItemRarityID.Blue;
	}
}