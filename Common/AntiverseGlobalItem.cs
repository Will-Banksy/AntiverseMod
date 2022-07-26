using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AntiverseMod.Items.Materials;

namespace AntiverseMod.Common {
	public class AntiverseGlobalItem : GlobalItem {
		public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
			return entity.type == ItemID.AshBlock;
		}

		public override void SetDefaults(Item item)
		{
			ItemID.Sets.ExtractinatorMode[item.type] = item.type;
		}

		public override void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack)
		{
			resultType = 0;
			resultStack = 1;

			// TODO: A nice helper function to help with percent chances and stuff would be nice
			// TODO: Does this make it too easy to get lots of obsidian and hellstone?
			float rand = Main.rand.NextFloat(100);
			if(rand < 3) { // ~3% chance of getting obsidian
				resultType = ItemID.Obsidian;
			} else if(rand >= 3 && rand < 9) { // ~6% chance of getting hellstone
				resultType = ItemID.Hellstone;

				if(rand < 6) { // ~50% chance of getting 2 hellstone
					resultStack = 2;
				}
			} else if(rand >= 9 && rand < 11) { // ~2% chance of getting sassolite
				resultType = ModContent.ItemType<Sassolite>();
			}
		}
	}
}