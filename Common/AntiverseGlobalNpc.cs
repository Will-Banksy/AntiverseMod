using AntiverseMod.Items.Materials;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using AntiverseMod.Items.Miscellaneous;

namespace AntiverseMod.Common; 

public class AntiverseGlobalNpc : GlobalNPC {
	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		switch(npc.type) {
			case NPCID.Golem: // Add gravity gun to Golem loot table with a 1/5 drop chance
				npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<GravityGun>(), 5));
				break;

			case NPCID.Plantera: // Replace temple key in Plantera's loot table with the temple key mold
				npcLoot.Get().ForEach(
					rule => {
						if(rule is LeadingConditionRule dropPool) {
							int i = dropPool.ChainedRules.FindIndex(attempt => attempt.RuleToChain is CommonDrop drop && drop.itemId == ItemID.TempleKey);
							if(i != -1) {
								((CommonDrop)dropPool.ChainedRules[i].RuleToChain).itemId = ModContent.ItemType<TempleKeyMold>();
							}
						}
					}
				);
				break;
		}
	}

	public override void ModifyShop(NPCShop shop) {
		shop.Add(ItemID.Beenade, Condition.DownedQueenBee, Condition.InJungle, Condition.PlayerCarriesItem(ItemID.Beenade));
	}
}