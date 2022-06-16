using Terraria.ModLoader;
using System.IO;
using Terraria;
using Terraria.ID;
using AntiverseMod.Networking;

// Area for general todos
// TODO: Move all the beenade stuff to respective Ranged namespaces/folders

namespace AntiverseMod
{
	public class AntiverseMod : Mod
	{
		public override void Load()
		{
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