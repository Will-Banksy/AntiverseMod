using System;
using MonoMod.Utils;
using Terraria;

namespace AntiverseMod.Utils;

public readonly struct EntityRef {
	/// <summary>
	/// Container for either NPC <see cref="Terraria.NPC.HitInfo"/> or Player <see cref="Terraria.Player.HurtInfo"/>
	/// </summary>
	public struct EntityHitInfo {
		private NPC.HitInfo npc;
		private Player.HurtInfo plr;
		private bool plrInfo;
		
		/// <summary>
		/// Provided two lambda functions, executes the npcHandler if this EntityHitInfo contains an NPC.HitInfo,
		/// and calls plrHandler if this contains a Player.HurtInfo
		/// </summary>
		/// <param name="npcHandler"></param>
		/// <param name="plrHandler"></param>
		/// <typeparam name="T">The type returned by both passed-in lambda functions</typeparam>
		/// <returns>The value returned by the executed lambda argument</returns>
		public T Match<T>(Func<NPC.HitInfo, T> npcHandler, Func<Player.HurtInfo, T> plrHandler) {
			if(plrInfo) {
				return plrHandler(plr);
			}
			return npcHandler(npc);
		}

		// public NPC.HitInfo Npc {
		// 	get {
		// 		if(plrInfo) {
		// 			throw new AssertionFailedException("This EntityHitInfo contains player hit info, cannot access npc hit info");
		// 		}
		// 		return npc;
		// 	}
		// }
		//
		// public Player.HurtInfo Plr {
		// 	get {
		// 		if(!plrInfo) {
		// 			throw new AssertionFailedException("This EntityHitInfo contains npc hit info, cannot access player hit info");
		// 		}
		// 		return plr;
		// 	}
		// }
		
		public EntityHitInfo(NPC.HitInfo npc) {
			this.npc = npc;
			plr = new Player.HurtInfo();
			plrInfo = false;
		}

		public EntityHitInfo(Player.HurtInfo plr) {
			this.plr = plr;
			npc = new NPC.HitInfo();
			plrInfo = true;
		}
	}

	/// <summary>
	/// Container for either NPC <see cref="Terraria.NPC.HitModifiers"/> or Player <see cref="Terraria.Player.HurtModifiers"/>
	/// </summary>
	public struct EntityHitModifiers {
		private NPC.HitModifiers npc;
		private Player.HurtModifiers plr;
		private bool plrMods;

		/// <summary>
		/// Provided two lambda functions, executes the npcHandler if this EntityHitModifiers contains an NPC.HitModifiers,
		/// and calls plrHandler if this contains a Player.HurtModifiers
		/// </summary>
		/// <param name="npcHandler"></param>
		/// <param name="plrHandler"></param>
		/// <typeparam name="T">The type returned by both passed-in lambda functions</typeparam>
		/// <returns>The value returned by the executed lambda argument</returns>
		public T Match<T>(Func<NPC.HitModifiers, T> npcHandler, Func<Player.HurtModifiers, T> plrHandler) {
			if(plrMods) {
				return plrHandler(plr);
			}
			return npcHandler(npc);
		}

		public EntityHitModifiers(NPC.HitModifiers npc) {
			this.npc = npc;
			plr = new Player.HurtModifiers();
			plrMods = false;
		}

		public EntityHitModifiers(Player.HurtModifiers plr) {
			this.plr = plr;
			npc = new NPC.HitModifiers();
			plrMods = true;
		}

		// public ref NPC.HitModifiers Npc {
		// 	get {
		// 		if(plrMods) {
		// 			throw new AssertionFailedException("This EntityHitInfo contains player hit modifiers, cannot access npc hit modifiers");
		// 		}
		// 		return ref npc;
		// 	}
		// }
	}
	
	public enum Type {
		NONE,
		NPC,
		PLAYER
	}

	public readonly int whoAmI;
	public readonly Type type;
	
	// TODO: Match function?

	public EntityRef(NPC npc) {
		//this.npc = npc;
		//plr = null;
		whoAmI = npc.whoAmI;
		type = Type.NPC;
	}

	public EntityRef(Player plr) {
		//npc = null;
		//this.plr = plr;
		whoAmI = plr.whoAmI;
		type = Type.PLAYER;
	}

	public EntityRef(Type type, int whoAmI) {
		this.type = type;
		this.whoAmI = whoAmI;
	}

	public NPC NPC() {
		if (type != Type.NPC) {
			throw new AssertionFailedException("Incorrect access of NPC() in EntityRef");
		}

		return Main.npc[whoAmI];
	}

	public Player Player() {
		if (type != Type.PLAYER) {
			throw new AssertionFailedException("Incorrect access of Player() in EntityRef");
		}

		return Main.player[whoAmI];
	}

	public Entity Generic() {
		return type switch {
			Type.NPC => Main.npc[whoAmI],
			Type.PLAYER => Main.player[whoAmI],
			_ => null
		};
	}
}