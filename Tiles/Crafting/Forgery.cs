using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using AntiverseMod.Items.Placeables;

// TODO: Add an adamantite/titanium upgrade

namespace AntiverseMod.Tiles.Crafting; 

public class Forgery : ModTile {
	public override void SetStaticDefaults() {
		Main.tileFrameImportant[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoAttach[Type] = true; // We do not want this tile to attach to anything
		Main.tileSolidTop[Type] = true; // The top is like a table

		ModTranslation name = CreateMapEntryName();
		name.SetDefault("Forgery");
		AddMapEntry(new Color(238, 85, 70), name); // Same colour as hellforge

		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 }; // Height of each "piece" - bottom piece is 18px to intersect with the ground a little to show behind like grass
		TileObjectData.addTile(Type);

		AdjTiles = new int[] { TileID.Hellforge, TileID.Anvils };
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY) {
		Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, Width: 48, Height: 32, ModContent.ItemType<ForgeryItem>());
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = fail ? 3 : 9;
	}

	public override bool CreateDust(int i, int j, ref int type) {
		if (Main.rand.NextBool()) {
			type = DustID.Torch;
		} else {
			type = 25; // Hellforge dust // TODO: Change to Lihzahrd Brick dust
		}

		return true;
	}
}