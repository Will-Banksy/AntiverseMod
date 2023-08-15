using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using AntiverseMod.Items.Miscellaneous;

namespace AntiverseMod.Common; 

public class AntiverseGlobalNPC : GlobalNPC {
	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		switch(npc.type) {
			case NPCID.Golem: // Add gravity gun to Golem loot table
				npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<GravityGun>(), 1)); // TODO: Review this
				break;

			case NPCID.Plantera: // Remove temple key from Plantera's loot table
				// TODO: Add an alternative way to get the temple key
				npcLoot.Get().ForEach(
					rule => {
						if(rule is LeadingConditionRule dropPool) {
							int i = dropPool.ChainedRules.FindIndex(attempt => attempt.RuleToChain is CommonDrop drop && drop.itemId == ItemID.TempleKey);
							if(i != -1) {
								dropPool.ChainedRules.RemoveAt(i);
							}
						}
					}
				);
				break;
		}
	}

	public override void ModifyShop(NPCShop shop) {
		shop.Add(ItemID.Beenade, Condition.DownedQueenBee);
	}
}