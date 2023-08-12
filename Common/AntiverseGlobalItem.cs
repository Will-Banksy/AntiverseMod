using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AntiverseMod.Items.Materials;

namespace AntiverseMod.Common; 

public class AntiverseGlobalItem : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.AshBlock;
	}

	public override void SetDefaults(Item item) {
		ItemID.Sets.ExtractinatorMode[item.type] = item.type;
	}

	public override void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack) {
		resultType = 0;
		resultStack = 1;

		// Previously I had it so that extractinating ash could yield hellstone and obsidian - but that'dd allow
		// skipping corruption/crimson boss in terms of tier progression so was removed
			
		// TODO: A nice helper function to help with percent chances and stuff would be nice
		float rand = Main.rand.NextFloat(100);
		if(rand > 2) { // ~2% chance of getting sassolite
			resultType = ModContent.ItemType<Sassolite>();
		}
	}
}