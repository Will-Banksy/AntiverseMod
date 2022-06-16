using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using AntiverseMod.Items.Miscellaneous;

namespace AntiverseMod.Globals
{
	public class AntiverseGlobalNPC : GlobalNPC
	{
		// public override bool InstancePerEntity {
		// 	get {
		// 		return true;
		// 	}
		// }

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.Golem)
			{
				npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<GravityGun>(), 1)); // TODO: Review this
			}
		}

		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == NPCID.Demolitionist)
			{
				Item beenade = new Item();
				beenade.SetDefaults(ItemID.Beenade);
				shop.item[nextSlot] = beenade;
				nextSlot++;
			}
		}
	}
}