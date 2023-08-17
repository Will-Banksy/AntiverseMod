using Terraria;
using Terraria.ModLoader;

namespace AntiverseMod.Dusts; 

public class FungicideDust : ModDust {
	public override void OnSpawn(Dust dust) {
		dust.noGravity = true;
		dust.noLightEmittence = false;
	}

	public override bool Update(Dust dust) {
		dust.scale += 0.045f;
		dust.velocity *= 0.99f;

		if(!dust.noLightEmittence) {
			float intensity = dust.scale * 0.4f;
			Lighting.AddLight(dust.position, intensity * 0.8f, intensity * 0.8f, intensity * 0.5f);
		}

		if(dust.scale < 0.006f) {
			dust.active = false;
		}

		return true;
	}
}