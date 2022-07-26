using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AntiverseMod.Dusts {
	public class BoronDust : ModDust {
		public override void OnSpawn(Dust dust)
		{
			dust.noLight = true;
		}
	}
}