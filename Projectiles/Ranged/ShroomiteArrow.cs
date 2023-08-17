using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AntiverseMod.Utils;
using Terraria.ModLoader;
using Terraria.Audio;

namespace AntiverseMod.Projectiles.Ranged;

public class ShroomiteArrow : MainProjBase {
	// Using ai[0] as the initial speed of the Projectile

	public override void SetDefaults() {
		Projectile.width = 14;
		Projectile.height = 14;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = 1;
		Projectile.tileCollide = true;
		// Projectile.extraUpdates = 2;
		Projectile.aiStyle = 0;
		Projectile.timeLeft = 300;
	}

	public override void AI() {
		if(Projectile.timeLeft == 300) {
			Projectile.ai[0] = Projectile.velocity.Length();
		}

		Projectile.FaceForward();
		Lighting.AddLight(Projectile.Center, new Vector3(0, 0.1f, 0.3f));

		Projectile.alpha = (int)Helper.Map.ExpoIn(Projectile.timeLeft, 300, 0, 0, 255);

		EntityRef myPlayerRef = EntityRef.Player(Projectile.owner);
		EntityRef target = EntityHelper.AcquireTarget(Projectile, myPlayerRef, null, (entity => {
			return Projectile.Center.AngleTo(entity.Generic().Center).AngleBetween(Projectile.velocity.ToRotation() - 0.3f, Projectile.velocity.ToRotation() + 0.3f);
		}) + EntityHelper.EntityFilterActive()
		   + EntityHelper.EntityFilterEnemy(myPlayerRef)
		   + EntityHelper.EntityFilterNpcCanBeChased(Projectile)
		   + EntityHelper.EntityFilterCollisionCanHit(Projectile.Center, Projectile.width, Projectile.height)
		);

		if(target.type != EntityRef.Type.None) {
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Helper.VelocityToPoint(Projectile.Center, target.Generic().Center, Projectile.ai[0]), 0.1f);
		} else {
			Projectile.velocity.Y += 0.1f;
		}
	}

	public override Color? GetAlpha(Color lightColor) {
		int rgba = 255 - Projectile.alpha;
		return new Color(rgba, rgba, rgba, rgba);
	}

	public override void ModifyHit(EntityRef target, ref EntityRef.EntityHitModifiers hitModifiers) {
		float multiplier = Helper.Map.Linear(Projectile.timeLeft, 300, 0, 1, 1.8f);
		hitModifiers.Match(
			npcMods => npcMods.Copy(sourceDamage: npcMods.SourceDamage * multiplier),
			plrMods => plrMods.Copy(sourceDamage: plrMods.SourceDamage * multiplier)
		);
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		SoundEngine.PlaySound(SoundID.Item10, Projectile.position); // SoundID.Item10 is the hit tile sound
		return true;
	}
}