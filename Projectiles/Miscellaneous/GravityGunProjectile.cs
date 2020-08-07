using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using AntiverseMod.Dusts;
using AntiverseMod.Networking;

namespace AntiverseMod.Projectiles.Miscellaneous
{
	public class GravityGunProjectile : ModProjectile
	{
		// public override string Texture => "AntiverseMod/Items/Miscellaneous/GravityGun";
		// public override string Texture => "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly;

		private Vector2 prevMousePos = new Vector2(-1, -1);
		private Vector2 origin = new Vector2(-1, -1);

		Texture2D glowmask = ModContent.GetTexture("AntiverseMod/Projectiles/Miscellaneous/GravityGunProjectile_Glowmask");

		enum Mode
		{
			GRAVITATING,
			FIRING_BOLTS
		}

		Mode mode = Mode.GRAVITATING;

		const int fireTime = 20;
		int fireTimer = fireTime;
		bool canFire = true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Gun Projectile");
		}

		public override void SetDefaults()
		{
			projectile.damage = 0;
			projectile.knockBack = 0;
			projectile.timeLeft = 99999999;
			projectile.width = 50;
			projectile.height = 218;
			projectile.tileCollide = false;
		}

		// Potential solution to the networking problem, ModPackets: https://github.com/tModLoader/tModLoader/wiki/intermediate-netcode
		public override void AI()
		{
			#region Holdout Code

			Player player = Main.player[projectile.owner];

			if(Main.mouseRight && !player.noItems && !player.CCed)
			{
				player.channel = true;
			}

			Vector2 vector13 = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter);
			if (Main.myPlayer == projectile.owner)
			{
				if (Main.player[projectile.owner].channel)
				{
					float num164 = Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].shootSpeed * projectile.scale;
					Vector2 vector14 = vector13;
					float num165 = (float)Main.mouseX + Main.screenPosition.X - vector14.X;
					float num166 = (float)Main.mouseY + Main.screenPosition.Y - vector14.Y;
					if (Main.player[projectile.owner].gravDir == -1f)
					{
						num166 = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector14.Y;
					}
					float num167 = (float)Math.Sqrt(num165 * num165 + num166 * num166);
					num167 = (float)Math.Sqrt(num165 * num165 + num166 * num166);
					num167 = num164 / num167;
					num165 *= num167;
					num166 *= num167;
					if (num165 != projectile.velocity.X || num166 != projectile.velocity.Y)
					{
						projectile.netUpdate = true;
					}
					projectile.velocity.X = num165;
					projectile.velocity.Y = num166;
				}
				else
				{
					projectile.Kill();
				}
			}
			if (projectile.velocity.X > 0f)
			{
				Main.player[projectile.owner].ChangeDir(1);
			}
			else if (projectile.velocity.X < 0f)
			{
				Main.player[projectile.owner].ChangeDir(-1);
			}
			projectile.spriteDirection = projectile.direction;
			Main.player[projectile.owner].ChangeDir(projectile.direction);
			Main.player[projectile.owner].heldProj = projectile.whoAmI;
			// Main.player[projectile.owner].SetDummyItemTime(2);
			Main.player[projectile.owner].itemAnimation = 2;
			Main.player[projectile.owner].itemTime = 2;
			// Main.player[projectile.owner].itemTimeMax = 3;
			projectile.position.X = vector13.X - (float)(projectile.width / 2);
			projectile.position.Y = vector13.Y - (float)(projectile.height / 2);
			projectile.rotation = (float)(Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.5700000524520874);
			if (Main.player[projectile.owner].direction == 1)
			{
				Main.player[projectile.owner].itemRotation = (float)Math.Atan2(projectile.velocity.Y * (float)projectile.direction, projectile.velocity.X * (float)projectile.direction);
			}
			else
			{
				Main.player[projectile.owner].itemRotation = (float)Math.Atan2(projectile.velocity.Y * (float)projectile.direction, projectile.velocity.X * (float)projectile.direction);
			}
			projectile.velocity.X *= 1f + (float)Main.rand.Next(-3, 4) * 0.01f;

			#endregion
			
			#region Actual Gravity Gun stuff

			Vector2 dustCentre = Helper.FromPolar(player.Center.AngleTo(Main.MouseWorld), 128, player.Center);

			if(Main.mouseRight)
			{
				mode = Mode.GRAVITATING;
			}
			else
			{
				mode = Mode.FIRING_BOLTS;
			}

			prevMousePos = origin;
			origin = Main.MouseWorld;

			fireTimer++;
			if(fireTimer >= fireTime)
			{
				canFire = true;
				fireTimer = fireTime;
			}

			if(mode == Mode.GRAVITATING)
			{
				if(Main.myPlayer == projectile.owner)
				{
					Dust.NewDust(dustCentre - new Vector2(20, 20), 40, 40, ModContent.DustType<GravityGunDust>());

					float maxDist = 100;
					int maxNPCArea = 1000;

					foreach(Item item in Main.item)
					{
						if(item.active)
						{
							if(Vector2.Distance(item.Center, origin) < maxDist || Vector2.Distance(item.Center, prevMousePos) < Math.Min(Vector2.Distance(origin, prevMousePos) + 100, 200))
							{
								item.position += (origin - item.Center) / 3;
								item.velocity = Vector2.Zero;
								
								if(Main.netMode == NetmodeID.MultiplayerClient)
									NetHandler.SendPacket(mod.GetPacket(), PacketID.ItemSetPosVel, new object[] { (ushort)item.whoAmI, (int)item.position.X, (int)item.position.Y, (short)item.velocity.X, (short)item.velocity.Y });
							}
						}
					}

					foreach(NPC npc in Main.npc)
					{
						if(npc.active && npc.type != NPCID.TargetDummy)
						{
							if(Vector2.Distance(npc.Center, origin) < maxDist || Vector2.Distance(npc.Center, prevMousePos) < Math.Min(Vector2.Distance(origin, prevMousePos) + 100, 200))
							{
								if(npc.width * npc.height < maxNPCArea && !npc.boss && npc.life < 2000)
								{
									npc.position += (origin - npc.Center) / 3;
									npc.velocity = Vector2.Zero;
									// npc.netUpdate = true;
									
									if(Main.netMode == NetmodeID.MultiplayerClient)
										NetHandler.SendPacket(mod.GetPacket(), PacketID.NPCSetPosVel, new object[] { (ushort)npc.whoAmI, (int)npc.position.X, (int)npc.position.Y, (short)npc.velocity.X, (short)npc.velocity.Y });
								}
							}
						}
					}
				}
			}
			else if(mode == Mode.FIRING_BOLTS)
			{
				if(Main.myPlayer == projectile.owner)
				{
					if(canFire)
					{
						int damage = 40;
						float knockBack = 20;
						Projectile.NewProjectile(dustCentre, Vector2.Zero, ModContent.ProjectileType<GravityGunProjectileBolt>(), damage, knockBack, projectile.owner);

						canFire = false;
						fireTimer = 0;
					}
				}
			}

			#endregion
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw
			(
				glowmask,
				new Vector2
				(
					projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f,
					projectile.position.Y - Main.screenPosition.Y + projectile.height - projectile.height * 0.5f
				),
				new Rectangle(0, 0, glowmask.Width, glowmask.Height),
				Color.White,
				projectile.rotation,
				new Vector2(projectile.width, projectile.height) * 0.5f,//glowmask.Size() * 0.5f,
				projectile.scale, 
				projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 
				0f
			);
		}
	}
}