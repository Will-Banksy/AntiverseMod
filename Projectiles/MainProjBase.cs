using System;
using Terraria;
using Terraria.ModLoader;
using AntiverseMod.Utils;
using MonoMod.Utils;

namespace AntiverseMod.Projectiles; 

public abstract class MainProjBase : ModProjectile {
	private bool noAI = true; // True if AI() hasn't been called yet. False if it has. If base.AI() is called anyway

	public override void AI() {
		if(noAI) {
			noAI = false;
			InitialAI();
		}
	}

	public virtual void InitialAI() {
	}

	public sealed override void OnHitNPC(NPC target, NPC.HitInfo hitInfo, int damageDone) {
		OnHit(new EntityRef(target), new EntityRef.EntityHitInfo(hitInfo));
	}

	public sealed override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
		OnHit(new EntityRef(target), new EntityRef.EntityHitInfo(hurtInfo));
	}

	/// <summary>
	/// Allows you to make special effects when this projectile hits an entity.
	/// <c>npcHitInfo</c> is null when the hit entity is a Player, and <c>plrHurtInfo</c> is null when the hit entity is an NPC
	/// </summary>
	public virtual void OnHit(EntityRef target, EntityRef.EntityHitInfo hitInfo) {
	}

	public sealed override void ModifyHitNPC(NPC target, ref NPC.HitModifiers hitModifiers) {
		// NPC.HitModifiers? npcHitModifiers = hitModifiers;
		// Player.HurtModifiers? plrHurtModifiers = null;
		// ModifyHit(new EntityRef(target), ref npcHitModifiers, ref plrHurtModifiers);
		// if(npcHitModifiers.HasValue) {
		// 	hitModifiers = npcHitModifiers.Value;
		// }
		ModifyHit(new EntityRef(target), new EntityRef.EntityHitModifiers(hitModifiers));
	}

	public sealed override void ModifyHitPlayer(Player target, ref Player.HurtModifiers hurtModifiers) {
		// NPC.HitModifiers? npcHitModifiers = null;
		// Player.HurtModifiers? plrHurtModifiers = hurtModifiers;
		// ModifyHit(new EntityRef(target), ref npcHitModifiers, ref plrHurtModifiers);
		// if(plrHurtModifiers.HasValue) {
		// 	hurtModifiers = plrHurtModifiers.Value;
		// }
		ModifyHit(new EntityRef(target), new EntityRef.EntityHitModifiers(hurtModifiers));
	}

	/// <summary>
	/// Allows modification of damage, crit, etc. when this projectile hits either a player or an npc. Changing either knockback or hitDirection only has an effect if the target is an npc
	/// </summary>
	public virtual void ModifyHit(EntityRef target, EntityRef.EntityHitModifiers hitModifiers) {// ref NPC.HitModifiers? npcHitModifiers, ref Player.HurtModifiers? plrHurtModifiers) {
	}
}