using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AntiverseMod.Dusts; 

public class ShroomiteDust : ModDust
{
	public override void OnSpawn(Dust dust) {
		dust.noGravity = true;
		dust.noLight = true;
		dust.scale = 2f;
	}

	public override bool Update(Dust dust) {
		dust.position += dust.velocity;
		dust.rotation += dust.velocity.X;
		dust.scale -= 0.15f;
		if (dust.scale < 0.5f) {
			dust.active = false;
		}
		return false;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return Color.White;
	}
}