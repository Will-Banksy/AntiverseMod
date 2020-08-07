using Terraria.ModLoader;
using System.IO;
using Terraria;
using Terraria.ID;
using AntiverseMod.Networking;

namespace AntiverseMod
{
	public class AntiverseMod : Mod
	{
		public override void Load()
		{
			Main.inventoryScale = -1f;
		}

		public override void Unload()
		{
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			NetHandler.RecievePacket(reader, whoAmI);
		}
	}
}