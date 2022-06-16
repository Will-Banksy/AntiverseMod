using Terraria;
using Terraria.ModLoader;
using AntiverseMod.Utils;

namespace AntiverseMod.Projectiles {
	using EntityUnion = EntityHelper.EntityUnion;

	public abstract class MainProjBase : ModProjectile {
		bool noAI = true; // True if AI() hasn't been called yet. False if it has. If base.AI() is called anyway

		public override void AI() {
			if(noAI) {
				noAI = false;
				InitialAI();
			}
		}

		public virtual void InitialAI() {
		}

		public sealed override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			OnHit(new EntityUnion(target), damage, knockback, crit);
		}

		public sealed override void OnHitPlayer(Player target, int damage, bool crit) {
			OnHit(new EntityUnion(target), damage, null, crit);
		}

		public sealed override void OnHitPvp(Player target, int damage, bool crit) {
			OnHit(new EntityUnion(target), damage, null, crit, true);
		}

		/// <summary>
		/// Allows you to make special effects when this projectile hits an entity. Pvp is true when the player hit is an opponent
		/// </summary>
		public virtual void OnHit(EntityUnion target, int damage, float? knockback, bool crit, bool pvp = false) {
		}

		public sealed override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			ModifyHit(new EntityUnion(target), ref damage, ref knockback, ref crit, ref hitDirection);
		}

		public sealed override void ModifyHitPlayer(Player target, ref int damage, ref bool crit) {
			float knockback = 0;
			int hitDirection = 0;
			ModifyHit(new EntityUnion(target), ref damage, ref knockback, ref crit, ref hitDirection);
		}


		public sealed override void ModifyHitPvp(Player target, ref int damage, ref bool crit) {
			float knockback = 0;
			int hitDirection = 0;
			ModifyHit(new EntityUnion(target), ref damage, ref knockback, ref crit, ref hitDirection, true);
		}

		/// <summary>
		/// Allows modification of damage, crit, etc. when this projectile hits either a player or an npc. Changing either knockback or hitDirection only has an effect if the target is an npc
		/// </summary>
		public virtual void ModifyHit(EntityUnion target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, bool pvp = false) {
		}
	}
}
