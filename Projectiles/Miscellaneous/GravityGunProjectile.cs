using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using AntiverseMod.Dusts;
using AntiverseMod.Networking;
using AntiverseMod.Utils;

namespace AntiverseMod.Projectiles.Miscellaneous;

public class GravityGunProjectile : ModProjectile {
	private Vector2 prevMousePos = new(-1, -1);
	private Vector2 origin = new(-1, -1);

	Texture2D glowmask = ModContent.Request<Texture2D>("AntiverseMod/Projectiles/Miscellaneous/GravityGunProjectile_Glowmask").Value;

	enum Mode {
		GRAVITATING,
		FIRING_BOLTS
	}

	Mode mode = Mode.GRAVITATING;

	const int fireTime = 20;
	int fireTimer = fireTime;
	bool canFire = true;

	public override void SetDefaults() {
		Projectile.damage = 0;
		Projectile.knockBack = 0;
		Projectile.timeLeft = 99999999;
		Projectile.width = 50;
		Projectile.height = 218;
		Projectile.tileCollide = false;
	}

	// Potential solution to the networking problem, ModPackets: https://github.com/tModLoader/tModLoader/wiki/intermediate-netcode
	public override void AI() {
		#region Holdout Code

		Player player = Main.player[Projectile.owner];

		if(Main.mouseRight && !player.noItems && !player.CCed) {
			player.channel = true;
		}

		Vector2 vector13 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter);
		if(Main.myPlayer == Projectile.owner) {
			if(Main.player[Projectile.owner].channel) {
				float num164 = Main.player[Projectile.owner].inventory[Main.player[Projectile.owner].selectedItem].shootSpeed * Projectile.scale;
				Vector2 vector14 = vector13;
				float num165 = (float)Main.mouseX + Main.screenPosition.X - vector14.X;
				float num166 = (float)Main.mouseY + Main.screenPosition.Y - vector14.Y;
				if(Main.player[Projectile.owner].gravDir == -1f) {
					num166 = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector14.Y;
				}

				float num167 = (float)Math.Sqrt(num165 * num165 + num166 * num166);
				num167 = num164 / num167;
				num165 *= num167;
				num166 *= num167;
				if(num165 != Projectile.velocity.X || num166 != Projectile.velocity.Y) {
					Projectile.netUpdate = true;
				}

				Projectile.velocity.X = num165;
				Projectile.velocity.Y = num166;
			} else {
				Projectile.Kill();
			}
		}

		if(Projectile.velocity.X > 0f) {
			Main.player[Projectile.owner].ChangeDir(1);
		} else if(Projectile.velocity.X < 0f) {
			Main.player[Projectile.owner].ChangeDir(-1);
		}

		Projectile.spriteDirection = Projectile.direction;
		Main.player[Projectile.owner].ChangeDir(Projectile.direction);
		Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
		// Main.player[Projectile.owner].SetDummyItemTime(2);
		Main.player[Projectile.owner].itemAnimation = 2;
		Main.player[Projectile.owner].itemTime = 2;
		// Main.player[Projectile.owner].itemTimeMax = 3;
		Projectile.position.X = vector13.X - (Projectile.width / 2f);
		Projectile.position.Y = vector13.Y - (Projectile.height / 2f);
		Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.5700000524520874);
		if(Main.player[Projectile.owner].direction == 1) {
			Main.player[Projectile.owner].itemRotation = (float)Math.Atan2(Projectile.velocity.Y * (float)Projectile.direction, Projectile.velocity.X * (float)Projectile.direction);
		} else {
			Main.player[Projectile.owner].itemRotation = (float)Math.Atan2(Projectile.velocity.Y * (float)Projectile.direction, Projectile.velocity.X * (float)Projectile.direction);
		}

		Projectile.velocity.X *= 1f + Main.rand.Next(-3, 4) * 0.01f;

		#endregion

		#region Actual Gravity Gun stuff

		Vector2 dustCentre = Helper.FromPolar(player.Center.AngleToTarget(Main.MouseWorld), 128, player.Center);

		if(Main.mouseRight) {
			mode = Mode.GRAVITATING;
		} else if(Main.mouseLeft) {
			mode = Mode.FIRING_BOLTS;
		}

		prevMousePos = origin;
		origin = Main.MouseWorld;

		fireTimer++;
		if(fireTimer >= fireTime) {
			canFire = true;
			fireTimer = fireTime;
		}

		if(mode == Mode.GRAVITATING) {
			if(Main.myPlayer == Projectile.owner) {
				Dust.NewDust(dustCentre - new Vector2(20, 20), 40, 40, ModContent.DustType<GravityGunDust>());

				float maxDist = 100;
				int maxNPCArea = 1000;

				foreach(Item item in Main.item) {
					if(item.active) {
						if(Vector2.Distance(item.Center, origin) < maxDist || Vector2.Distance(item.Center, prevMousePos) < Math.Min(Vector2.Distance(origin, prevMousePos) + 100, 200)) {
							item.position += (origin - item.Center) / 3;
							item.velocity = Vector2.Zero;

							if(Main.netMode == NetmodeID.MultiplayerClient) NetHandler.SendPacket(Mod.GetPacket(), PacketID.ItemSetPosVel, new object[] { (ushort)item.whoAmI, (int)item.position.X, (int)item.position.Y, (short)item.velocity.X, (short)item.velocity.Y });
						}
					}
				}

				foreach(NPC npc in Main.npc) {
					if(npc.active && npc.type != NPCID.TargetDummy) {
						if(Vector2.Distance(npc.Center, origin) < maxDist || Vector2.Distance(npc.Center, prevMousePos) < Math.Min(Vector2.Distance(origin, prevMousePos) + 100, 200)) {
							if(npc.width * npc.height < maxNPCArea && !npc.boss && npc.life < 2000) {
								npc.position += (origin - npc.Center) / 3;
								npc.velocity = Vector2.Zero;
								// npc.netUpdate = true;

								if(Main.netMode == NetmodeID.MultiplayerClient) NetHandler.SendPacket(Mod.GetPacket(), PacketID.NPCSetPosVel, new object[] { (ushort)npc.whoAmI, (int)npc.position.X, (int)npc.position.Y, (short)npc.velocity.X, (short)npc.velocity.Y });
							}
						}
					}
				}
			}
		} else if(mode == Mode.FIRING_BOLTS) {
			if(Main.myPlayer == Projectile.owner) {
				if(canFire) {
					int damage = 40;
					float knockBack = 20;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), dustCentre, Vector2.Zero, ModContent.ProjectileType<GravityGunProjectileBolt>(), damage, knockBack, Projectile.owner);

					canFire = false;
					fireTimer = 0;
				}
			}
		}

		#endregion
	}

	public override void PostDraw(Color lightColor) {
		Main.EntitySpriteDraw
		(
			glowmask,
			new Vector2
			(
				Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f,
				Projectile.position.Y - Main.screenPosition.Y + Projectile.height - Projectile.height * 0.5f
			),
			new Rectangle(0, 0, glowmask.Width, glowmask.Height),
			Color.White,
			Projectile.rotation,
			new Vector2(Projectile.width, Projectile.height) * 0.5f, //glowmask.Size() * 0.5f,
			Projectile.scale,
			Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
			0
		);
	}
}