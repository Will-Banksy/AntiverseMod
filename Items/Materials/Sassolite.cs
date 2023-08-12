using Terraria.ModLoader;
using Terraria.ID;

namespace AntiverseMod.Items.Materials; 

public class Sassolite : ModItem {
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Sassolite");
	}

	public override void SetDefaults()
	{
		Item.width = 26;
		Item.height = 20;

		Item.maxStack = 999;
		Item.value = Terraria.Item.buyPrice(silver: 1);
		Item.rare = ItemRarityID.Blue;
	}
}