using AntiverseMod.Projectiles.Ranged.BetterBeenades;
using Terraria.ModLoader;

namespace AntiverseMod; 

public class AntiverseModSystem : ModSystem
{
	public override void PostUpdateProjectiles()
	{
		for(int i = 0; i < BeeBase.beeHitCooldown.Length; i++) {
			if(BeeBase.beeHitCooldown[i] != 0) {
				BeeBase.beeHitCooldown[i]--;
			}
		}
	}
}