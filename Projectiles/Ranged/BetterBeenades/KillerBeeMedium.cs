using Terraria;
using AntiverseMod.Utils;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.Projectiles.Ranged.BetterBeenades;

public class KillerBeeMedium : BeeBase {
	public override void SetStaticDefaults() {
		Main.projFrames[Projectile.type] = 4;
	}

	public override void SetDefaults() {
		base.SetDefaults();

		Projectile.width = 16;
		Projectile.height = 18;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = 3;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 360; // 600 is bees from beenade timeLeft
	}

	public override void AI() {
		Helper.StepAnim(Projectile, 10);
		base.AI();
	}

	public override void ModifyHit(EntityRef target, ref EntityRef.EntityHitModifiers hitModifiers) {
		hitModifiers.Match(
			npcHitModifiers => npcHitModifiers.Copy(sourceDamage: npcHitModifiers.SourceDamage * 1.08f),
			plrHurtModifiers => plrHurtModifiers.Copy(sourceDamage: plrHurtModifiers.SourceDamage * 1.08f)
		);
	}

	public override void OnHit(EntityRef target, EntityRef.EntityHitInfo hitInfo) {
		base.OnHit(target, hitInfo);
		if(target.type == EntityRef.Type.Npc) {
			target.Npc().AddBuff(BuffID.Poisoned, 360);
		}
	}
}