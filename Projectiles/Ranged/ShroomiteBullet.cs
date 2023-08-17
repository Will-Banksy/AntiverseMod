using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AntiverseMod.Dusts;
using AntiverseMod.Utils;

namespace AntiverseMod.Projectiles.Ranged;

public class ShroomiteBullet : MainProjBase {
	private bool tangible = false;
	private Vector2 mousePos;
	private Vector2 originPos;
	private float mouseOriginDist;

	public override void SetDefaults() {
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = 3;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 2;
		Projectile.aiStyle = 0;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = -1; // One hit per npc
		Projectile.timeLeft = 300;
		Projectile.alpha = 50;
	}

	public override void InitialAI() {
		if(Main.myPlayer == Projectile.owner) {
			mousePos = Main.MouseWorld;
			originPos = Projectile.Center;
			mouseOriginDist = Vector2.Distance(mousePos, originPos);
		}
	}

	public override void AI() {
		base.AI();

		if(Helper.GoingAwayFrom(mousePos, Projectile.Center, Projectile.velocity) || Vector2.Distance(Projectile.Center, originPos) > mouseOriginDist) {
			tangible = true;
			Projectile.tileCollide = true;
		}

		if(tangible && Projectile.alpha < 255) {
			Projectile.alpha += 15;
			if(Projectile.alpha > 255) {
				Projectile.alpha = 255;
			}
		}

		Projectile.FaceForward();
		Lighting.AddLight(Projectile.Center, new Vector3(0, 0.1f, 0.3f));
	}

	public override Color? GetAlpha(Color lightColor) {
		return new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha);
	}

	public override void OnHit(EntityRef target, EntityRef.EntityHitInfo hitInfo) {
		if(target.type == EntityRef.Type.Npc) {
			target.Npc().immune[Projectile.owner] = 0;
		}

		for(int i = 0; i < 2; i++) {
			Vector2 vel = Helper.RandSpread(Projectile.velocity, 0.4f, 3); //Helper.FromPolar(ang, speed);

			int type = ModContent.DustType<ShroomiteDust>();
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type, vel.X, vel.Y);
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		SoundEngine.PlaySound(SoundID.Item10, Projectile.position); // SoundID.Item10 is the hit tile sound
		return true;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		if(!tangible) {
			return false;
		}

		return base.Colliding(projHitbox, targetHitbox);
	}
}