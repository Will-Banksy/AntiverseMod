using AntiverseMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.Common;

public class AntiverseGlobalProjectile : GlobalProjectile {
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		switch(projectile.type) {
			case ProjectileID.RuneBlast: {
				if(projectile.friendly && !projectile.hostile && projectile.owner == Main.myPlayer) {
					target.AddBuff(ModContent.BuffType<RuneMarked>(), RuneMarked.RuneBaseDuration, false);
					target.GetGlobalNPC<RuneMarkedGlobalNPC>().playerSource = projectile.owner;
				}
				break;
			}
		}
	}
}
