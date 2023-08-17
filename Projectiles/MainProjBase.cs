using System.Diagnostics;
using Terraria;
using Terraria.ModLoader;
using AntiverseMod.Utils;

namespace AntiverseMod.Projectiles; 

public abstract class MainProjBase : ModProjectile {
	private bool noAI = true; // True if AI() hasn't been called yet. False if it has. If base.AI() is called anyway

	public override void AI() {
		if(noAI) {
			noAI = false;
			InitialAI();
		}
	}

	/// <summary>
	/// Called at the start of the first <c>AI()</c> call, and not on subsequent ones.
	/// If you override <c>AI()</c> then you must call <c>base.AI()</c> preferably first in order for this method to be called.
	/// </summary>
	public virtual void InitialAI() {
	}

	public sealed override void OnHitNPC(NPC target, NPC.HitInfo hitInfo, int damageDone) {
		OnHit(new EntityRef(target), new EntityRef.EntityHitInfo(hitInfo));
	}

	public sealed override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
		OnHit(new EntityRef(target), new EntityRef.EntityHitInfo(hurtInfo));
	}

	/// <summary>
	/// Allows you to create special effects when this projectile hits an NPC or player.
	/// <br /><br />
	/// For NPC hits, this method is only called for the owner of the projectile - in multiplayer, projectiles owned by a player
	/// call this method on that client, and projectiles owned by the server (e.g. enemy projectiles) call this method
	/// on the server.
	/// <br /><br />
	/// For player hits, this method is called only on the player's client in multiplayer.
	/// <br /><br />
	/// Use <c>hitInfo.Match(npcHitInfo => {}, plrHurtInfo => {})</c> to handle both cases, if necessary.
	/// </summary>
	public virtual void OnHit(EntityRef target, EntityRef.EntityHitInfo hitInfo) {
	}

	public sealed override void ModifyHitNPC(NPC target, ref NPC.HitModifiers hitModifiers) {
		EntityRef.EntityHitModifiers mods = new EntityRef.EntityHitModifiers(hitModifiers);
		ModifyHit(new EntityRef(target), ref mods);
		Debug.Assert(mods.Npc != null, "mods.Npc != null");
		hitModifiers = mods.Npc.Value;
	}

	public sealed override void ModifyHitPlayer(Player target, ref Player.HurtModifiers hurtModifiers) {
		EntityRef.EntityHitModifiers mods = new EntityRef.EntityHitModifiers(hurtModifiers);
		ModifyHit(new EntityRef(target), ref mods);
		Debug.Assert(mods.Player != null, "mods.Player != null");
		hurtModifiers = mods.Player.Value;
	}

	/// <summary>
	/// Allows modification of damage, knockback, etc., that this projectile does to an NPC or player.
	/// <br /><br />
	/// For NPC hits, this method is only called for the owner of the projectile - in multiplayer, projectiles owned by a player
	/// call this method on that client, and projectiles owned by the server (e.g. enemy projectiles) call this method
	/// on the server.
	/// <br /><br />
	/// For player hits, this method is called only on the player's client in multiplayer.
	/// <br /><br />
	/// Use <c>hitModifiers.Match(npcHitModifiers => {}, plrHurtModifiers => {})</c> to handle both cases, if necessary.
	/// </summary>
	public virtual void ModifyHit(EntityRef target, ref EntityRef.EntityHitModifiers hitModifiers) {// ref NPC.HitModifiers? npcHitModifiers, ref Player.HurtModifiers? plrHurtModifiers) {
	}
}