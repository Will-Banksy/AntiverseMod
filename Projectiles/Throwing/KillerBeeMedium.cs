using Terraria;
using AntiverseMod.Utils;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.Projectiles.Throwing
{
	public class KillerBeeMedium : BeeBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Killer Bee");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();

			Projectile.width = 16;
			Projectile.height = 18;
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

		public override void ModifyHit(EntityHelper.EntityUnion target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, bool pvp = false) {
			damage = (int)(damage * 1.08f);
		}

		public override void OnHit(EntityHelper.EntityUnion target, int damage, float? knockback, bool crit, bool pvp = false) {
			base.OnHit(target, damage, knockback, crit, pvp);
			if(target.type == EntityHelper.EntityUnion.Type.NPC) {
				target.NPC().AddBuff(BuffID.Poisoned, 360);
			}
		}
	}
}
