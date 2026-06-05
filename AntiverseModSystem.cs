using AntiverseMod.Projectiles.Ranged.BetterBeenades;
using Terraria.ModLoader;

namespace AntiverseMod;

public class AntiverseModSystem : ModSystem {
	public override void PostUpdateProjectiles() {
		for(int i = 0; i < BeeBase.BeeHitCooldown.Length; i++) {
			if(BeeBase.BeeHitCooldown[i] != 0) {
				BeeBase.BeeHitCooldown[i]--;
			}
		}
	}
}
