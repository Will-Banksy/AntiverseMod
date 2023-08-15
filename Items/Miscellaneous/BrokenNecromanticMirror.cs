using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AntiverseMod.Items.Miscellaneous;

public class BrokenNecromanticMirror : ModItem {
	public override void SetDefaults() {
		Item.width = 26;
		Item.height = 46;
		Item.value = Item.sellPrice(gold: 1, silver: 80);
		Item.rare = ItemRarityID.LightRed;
	}
}