using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace AntiverseMod.Config {
	public class AntiverseConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("Miscellaneous")]
		[Label("Necromantic Mirror breaks")]
		[Tooltip("Set to true for the Necromantic Mirror to break upon death; False for it to not")]
		[DefaultValue(true)]
		public bool NecromanticMirrorBreak;
	}
}