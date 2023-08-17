using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace AntiverseMod.Items.Materials; 

public class TempleKeyMold : ModItem {
	public override void SetDefaults() {
		Item.width = 34;
		Item.height = 48;

		Item.maxStack = Item.CommonMaxStack;
		Item.value = 0;
		Item.rare = ItemRarityID.Lime;
	}
}