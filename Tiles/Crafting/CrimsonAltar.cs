using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.ObjectData;
using Terraria.DataStructures;
using AntiverseMod.Items.Placeables;
using Terraria.Localization;

namespace AntiverseMod.Tiles.Crafting;

public class CrimsonAltar : ModTile // Multi Tile
{
	public override void SetStaticDefaults() {
		Main.tileFrameImportant[Type] = true;
		Main.tileSolid[Type] = false; // You'll probably want to be able to walk through the object, since that's possible with all altars.
		Main.tileNoAttach[Type] = true; // We do not want this tile to attach to anything.
		MineResist = 1.2f;

		AddMapEntry(new Color(119, 101, 125), Language.GetText("Mods.AntiverseMod.Tiles.CrimsonAltar.MapEntry"));

		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.Width = 3; // A width of 3 'pieces'.
		TileObjectData.newTile.Height = 2; // A height of 2 'pieces'.
		TileObjectData.newTile.Origin = new Point16(0, 0); // The origin of the tile. You might want to change the Y value of this Point16, since most tiles are placed from the bottom left corner in Terraria (which would make it (0, 1) probably).
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0); // Make sure that we need to place this altar on solid tiles, based on the width (which is 3, as set before).
		// TileObjectData.newTile.UsesCustomCanPlace = true; // Yeah, we use a custom Can Place.
		TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 }; // Allright, so here we get to the point where the topper piece is 16 pixels in height and the bottom piece is 18 pixels.
		// You start with the top most tile when assigning these values, so make sure you work from top to bottom.
		TileObjectData.newTile.CoordinateWidth = 16; // Each of the tile 'pieces' is 16 pixels in width.
		TileObjectData.newTile.CoordinatePadding = 2; // And a padding of 2 pixels.
		TileObjectData.addTile(Type); // And make sure you finish it off, so that this data is actually used.

		AdjTiles = new int[] { TileID.DemonAltar }; //Makes this tile act as the tile you enter, in this case a demon altar. Useful for making custom tiles crafting stations

		// We do not need to set the 'drop', since when using tiles that conist of multiple pieces, you'll want to override the KillMultiTile function.
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = fail ? 1 : 3;
	}

	public override bool CreateDust(int i, int j, ref int type) {
		type = 5;
		// Dust.NewDust(new Vector2(i * 16f, j * 16f), 16, 16, type);
		return true;
	}
}