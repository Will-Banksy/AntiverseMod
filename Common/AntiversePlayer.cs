using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using AntiverseMod.Items.Miscellaneous;
using AntiverseMod.Config;

namespace AntiverseMod.Common;

public class AntiversePlayer : ModPlayer {
	public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
		if(ModContent.GetInstance<AntiverseConfig>().NecromanticMirrorBreaksOnDeath) {
			for(int i = 0; i < Player.inventory.Length; i++) {
				if(Player.inventory[i].type == ModContent.ItemType<NecromanticMirror>()) {
					Player.inventory[i].TurnToAir();
					Player.inventory[i] = new Item(ModContent.ItemType<BrokenNecromanticMirror>());
					break;
				}
			}
		}

		base.Kill(damage, hitDirection, pvp, damageSource);
	}
}