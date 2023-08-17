using System;
using AntiverseMod.Dusts;
using AntiverseMod.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.Projectiles.Miscellaneous; 

public class FungicidePowderProj : MainProjBase {
	public override string Texture => $"Terraria/Projectile_{ProjectileID.ShadowBeamFriendly}";

	public override void SetDefaults() {
		Projectile.aiStyle = ProjAIStyleID.Powder;
		Projectile.width = 48;
		Projectile.height = 48;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.penetrate = -1;
		Projectile.alpha = 255;
		Projectile.timeLeft = 180;
	}

	private void PurifyFungi(int i, int j) {
		Tile tile = Main.tile[i, j];
		bool needsUpdate = false;
		
		if(tile.TileType == TileID.MushroomGrass) {
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(i, j, TileID.JungleGrass);
			tile.TileType = TileID.JungleGrass;
			needsUpdate = true;
		}

		if(tile.TileType == TileID.MushroomPlants || tile.TileType == TileID.MushroomVines) {
			WorldGen.KillTile(i, j);
		}

		if(tile.WallType == WallID.MushroomUnsafe) {
			tile.WallType = WallID.Jungle;
			needsUpdate = true;
		}

		if(needsUpdate) {
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
		}
	}

	public override void InitialAI() {
		for(int i = 0; i < 30; i++) {
			Vector2 dustVel = Helper.RandSpread(Projectile.velocity, (float)Math.PI / 8f) * Main.rand.NextFloat(0, 2);
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<FungicideDust>(), dustVel.X, dustVel.Y, 50);
		}
	}

	public override void AI() {
		base.AI();
		if(Projectile.timeLeft % 5 == 0) {
			int startI = (int)(Projectile.position.X / 16f) - 1;
			int startJ = (int)(Projectile.position.Y / 16f) - 1;
			int endI = (int)((Projectile.position.X + Projectile.width) / 16f) + 2;
			int endJ = (int)((Projectile.position.Y + Projectile.height) / 16f) + 2;
			
			if(startI < 0) {
				startI = 0;
			}
			if(endI > Main.maxTilesX) {
				endI = Main.maxTilesX;
			}
			if(startJ < 0) {
				startJ = 0;
			}
			if(endJ > Main.maxTilesY) {
				endJ = Main.maxTilesY;
			}
			
			for(int i = startI; i < endI; i++) {
				for(int j = startJ; j < endJ; j++) {
					PurifyFungi(i, j);
				}
			}
		}
	}
}