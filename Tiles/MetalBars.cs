using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using AntiverseMod.Dusts;

namespace AntiverseMod.Tiles;

public class MetalBars : ModTile {
	public override void SetStaticDefaults() {
		Main.tileShine[Type] = 1100;
		Main.tileSolid[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileFrameImportant[Type] = true;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.MetalBar")); // Localised text for "metal bar"
	}

	// TODO: Check that item drops are automatically registered
	// public override bool Drop(int i, int j) {
	// 	Tile t = Main.tile[i, j];
	// 	int style = t.TileFrameX / 18;
	//
	// 	switch(style) {
	// 		case 0:
	// 			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<BoronCarbide>());
	// 			break;
	// 	}
	//
	// 	return base.Drop(i, j);
	// }

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = fail ? 3 : 9;
	}

	public override bool CreateDust(int i, int j, ref int type) {
		Tile t = Main.tile[i, j];
		int style = t.TileFrameX / 18;

		switch(style) {
			case 0:
				type = ModContent.DustType<BoronDust>();
				break;
		}

		return true;
	}
}