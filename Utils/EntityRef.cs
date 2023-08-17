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
		/// <param name="npcHandler">Must return the modified NPC.HitModifiers for it to be applied</param>
		/// <param name="plrHandler">Must return the modified Player.HurtModifiers for it to be applied</param>
		public void Match(Func<NPC.HitModifiers, NPC.HitModifiers> npcHandler, Func<Player.HurtModifiers, Player.HurtModifiers> plrHandler) {
			if(plrMods) {
				plr = plrHandler(plr);
			}
			npc = npcHandler(npc);
		}

		public NPC.HitModifiers? Npc => plrMods ? null : npc;

		public Player.HurtModifiers? Player => plrMods ? plr : null;

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
	}
	
	public enum Type {
		None,
		Npc,
		Player
	}

	public readonly int whoAmI;
	public readonly Type type;

	public T Match<T>(Func<NPC, T> npcHandler, Func<Player, T> plrHandler, Func<T> defaultHandler) {
		switch(type) {
			case Type.Npc:
				return npcHandler(Main.npc[whoAmI]);
			
			case Type.Player:
				return plrHandler(Main.player[whoAmI]);
			
			default:
				return defaultHandler();
		}
	}

	public T Match<T>(Func<NPC, T> npcHandler, Func<Player, T> plrHandler) {
		switch(type) {
			case Type.Npc:
				return npcHandler(Main.npc[whoAmI]);
			
			case Type.Player:
				return plrHandler(Main.player[whoAmI]);
			
			default:
				return default;
		}
	}

	public EntityRef(NPC npc) {
		//this.npc = npc;
		//plr = null;
		whoAmI = npc.whoAmI;
		type = Type.Npc;
	}

	public EntityRef(Player plr) {
		//npc = null;
		//this.plr = plr;
		whoAmI = plr.whoAmI;
		type = Type.Player;
	}

	public EntityRef(Type type, int whoAmI) {
		this.type = type;
		this.whoAmI = whoAmI;
	}

	public static EntityRef Npc(int whoAmI) {
		return new EntityRef(Type.Npc, whoAmI);
	}

	public static EntityRef Player(int whoAmI) {
		return new EntityRef(Type.Player, whoAmI);
	}

	public NPC Npc() {
		if (type != Type.Npc) {
			throw new AssertionFailedException("Incorrect access of NPC() in EntityRef");
		}

		return Main.npc[whoAmI];
	}

	public Player Player() {
		if (type != Type.Player) {
			throw new AssertionFailedException("Incorrect access of Player() in EntityRef");
		}

		return Main.player[whoAmI];
	}

	public Entity Generic() {
		return type switch {
			Type.Npc => Main.npc[whoAmI],
			Type.Player => Main.player[whoAmI],
			_ => null
		};
	}
}