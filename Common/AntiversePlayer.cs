using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using AntiverseMod.Items.Miscellaneous;
using AntiverseMod.Config;

namespace AntiverseMod.Common; 

public class AntiversePlayer : ModPlayer {
	public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
		if (ModContent.GetInstance<AntiverseConfig>().NecromanticMirrorBreak) {
			foreach (Item item in Player.inventory) {
				if (item.type == ModContent.ItemType<NecromanticMirror>()) {
					item.SetDefaults(ModContent.ItemType<BrokenNecromanticMirror>());
					break;
				}
			}
		}

		base.Kill(damage, hitDirection, pvp, damageSource);
	}
}