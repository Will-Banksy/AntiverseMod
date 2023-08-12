using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AntiverseMod.Utils;
using System;
using AntiverseMod.Networking;
using static AntiverseMod.Utils.EntityHelper;

namespace AntiverseMod.Projectiles.Throwing; 

public abstract class BeeBase : MainProjBase {
	public static sbyte[] beeHitCooldown = new sbyte[Main.npc.Length];

	public EntityRef target = default;

	/// The max speed the bee can reach
	public float maxSpeed = 8f;

	/// The amount to lerp the current velocity towards the target velocity
	public float turningSpeed = 0.05f;

	// ai[0] - 1 if a target needs to be acquired, 0 otherwise
	// ai[1] - stagger for targeting (to reduce lag)

	public override void SetDefaults() {
		//Projectile.aiStyle = 36;
		Projectile.ai[0] = 1;
	}

	public override void Kill(int timeLeft) {
		int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bee, Projectile.velocity.X, Projectile.velocity.Y, 50, default(Color), 1f);
		Main.dust[dustId].noGravity = true;
		Main.dust[dustId].scale = 1f;
		base.Kill(timeLeft);
	}

	public override void InitialAI() {
		Projectile.ai[1] = Main.rand.Next(20);

		// In case bee is shot at higher than max speed
		if(Projectile.velocity.Length() > maxSpeed) {
			Projectile.velocity.Normalize();
			Projectile.velocity *= maxSpeed;
		}
	}

	public override void AI() {
		base.AI();

		// Check if target is dead. If so, we need to acquire another target
		if(Projectile.owner == Main.myPlayer && (target.type == EntityRef.Type.NONE || !target.Generic().active) && (Projectile.timeLeft + Projectile.ai[1]) % 10 == 0) {
			Projectile.ai[0] = 1;
		} else {
			Projectile.ai[0] = 0;
		}

		// Acquire a target if one has not already been found
		if((int)Projectile.ai[0] == 1) {
			EntityRef plr = new EntityRef(EntityRef.Type.PLAYER, Projectile.owner);
			target = AcquireTarget(Projectile, plr, 1000f, EntityFilterAll(plr, Projectile, Projectile.position, Projectile.width, Projectile.height));
			if(target.type != EntityRef.Type.NONE) {
				Projectile.ai[0] = 0;
				if(Main.netMode == NetmodeID.MultiplayerClient) {
					NetHandler.SendPacket(Mod.GetPacket(), PacketID.BeeSyncTarget, new object[] { (ushort)Projectile.whoAmI, (byte)target.type, (ushort)target.whoAmI });
				}
			}
		}

		// Set the rotation and direction of the sprite
		if(Projectile.velocity.X > 0f) {
			Projectile.spriteDirection = 1;
		}
		else if(Projectile.velocity.X < 0f) {
			Projectile.spriteDirection = -1;
		}
		Projectile.rotation = Projectile.velocity.X * 0.1f;

		// If there is no target, do not continue
		if(target.type != EntityRef.Type.NONE) {
			// Home in on the enemy
			Vector2 toTarget = target.Generic().Center - Projectile.Center;
			if(toTarget.Length() > maxSpeed) {
				//toTarget.SetLength(maxSpeed);
				toTarget.Normalize();
				toTarget *= maxSpeed;
			}
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, toTarget, turningSpeed);
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.penetrate--;
		if(Projectile.penetrate <= 0) {
			Projectile.Kill();
		}
		if(Projectile.velocity.X != oldVelocity.X) {
			Projectile.velocity.X = -oldVelocity.X;
		}
		if(Projectile.velocity.Y != oldVelocity.Y) {
			Projectile.velocity.Y = -oldVelocity.Y;
		}
		return false;
	}

	public override void OnHit(EntityRef target, int damage, float? knockback, bool crit, bool pvp = false) {
		if(target.type == EntityRef.Type.NPC) {
			target.NPC().immune[Projectile.owner] = 0;
			beeHitCooldown[target.NPC().whoAmI] = 10;
		}
	}

	public override bool? CanHitNPC(NPC target) {
		if(beeHitCooldown[target.whoAmI] == 0) {
			return null;
		}
		return false;
	}
}