using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AntiverseMod.Utils;

namespace AntiverseMod.Projectiles.Ranged.BetterBeenades;

public class KillerBeenadeProj : GrenadeBase {
	public override void SetDefaults() {
		base.SetDefaults();

		Projectile.width = 22;
		Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = -1;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 80;

		explosionRadius = 30;
		explodeOnContact = true;
	}

	protected override void OnExplode() {
		base.OnExplode();

		float beeSpeed = 4f;

		foreach(Vector2 vec in Helper.vecArrCross) {
			for(int i = 0; i < 5; i++) {
				int beeType = KillerBeeType(i);

				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Helper.RandSpread(vec * beeSpeed, 0.3f, 1.25f), beeType, Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}
	}

	private int KillerBeeType(int beeNum) {
		if(beeNum <= 1 || (Main.player[Projectile.owner].strongBees && beeNum <= 3)) {
			return ModContent.ProjectileType<KillerBeeMedium>();
		}

		return ModContent.ProjectileType<KillerBeeSmall>();
	}

	public override void OnHit(EntityRef target, EntityRef.EntityHitInfo hitInfo) {
		base.OnHit(target, hitInfo);
		target.Npc().immune[Projectile.owner] = 0;
	}
}