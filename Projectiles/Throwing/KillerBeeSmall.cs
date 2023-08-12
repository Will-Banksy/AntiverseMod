using Terraria;
using AntiverseMod.Utils;
using Terraria.ModLoader;

namespace AntiverseMod.Projectiles.Throwing; 

public class KillerBeeSmall : BeeBase
{
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Killer Bee");
		Main.projFrames[Projectile.type] = 4;
	}

	public override void SetDefaults()
	{
		base.SetDefaults();

		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = 3;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 360; // 600 is bees from beenade timeLeft
	}

	public override void AI()
	{
		Helper.StepAnim(Projectile, 10);
		base.AI();
	}
}