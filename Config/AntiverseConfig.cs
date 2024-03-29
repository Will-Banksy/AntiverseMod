using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace AntiverseMod.Config; 

public class AntiverseConfig : ModConfig {
	public override ConfigScope Mode => ConfigScope.ServerSide;

	[Header("Miscellaneous")]
	[DefaultValue(false)]
	public bool NecromanticMirrorBreaksOnDeath;

	[DefaultValue(true)]
	public bool NecromanticMirrorBreaksOnUse;
}