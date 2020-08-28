using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AntiverseMod.Items.Miscellaneous;

namespace AntiverseMod.Globals {
	public class AntiverseGlobalNPC : GlobalNPC {
		// public override bool InstancePerEntity {
		// 	get {
		// 		return true;
		// 	}
		// }

		public override void NPCLoot(NPC npc) {
			if(npc.type == NPCID.Golem) {
				Item.NewItem(npc.getRect(), ModContent.ItemType<GravityGun>()); // TODO: Review this
			}
		}
	}
}