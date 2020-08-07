using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AntiverseMod.Dusts
{
	public class GravityGunDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true;
			dust.noLight = true;

			// Need this in or the frame will be chosen the vanilla way - assming all dusts are 10x30
            dust.frame = new Rectangle(0, Main.rand.Next(3) * 20, 20, 20);
		}

		public override bool Update(Dust dust) {
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X;
			if(dust.noGravity)
			{
				dust.scale -= 0.1f;
				if (dust.scale < 0.5f) {
					dust.active = false;
				}
			}
			else
			{
				dust.scale -= 0.02f;
				if (dust.scale < 0.2f) {
					dust.active = false;
				}
			}
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return Color.White;
		}
	}
}