using System;
using Terraria;

namespace AntiverseMod.Utils;

public readonly struct EntityRef {
	public enum Type {
		NONE,
		NPC,
		PLAYER
	}

	public readonly int whoAmI;
	public readonly Type type;

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
			throw new Exception("Incorrect access of NPC() in EntityUnion");
		}

		return Main.npc[whoAmI];
	}

	public Player Player() {
		if (type != Type.PLAYER) {
			throw new Exception("Incorrect access of Player() in EntityUnion");
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