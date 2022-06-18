using Terraria.ModLoader;
using System.IO;
using Terraria;
using Terraria.ID;
using AntiverseMod.Networking;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

// General todos
// TODO: Move all the beenade stuff to respective Ranged namespaces/folders
// TODO: Add journey mode researching support

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

		public static Asset<T> RequestAsset<T>(string name) where T: class {
			return ModContent.Request<T>(name, AssetRequestMode.ImmediateLoad);
		}

		public static Asset<T> RequestAssetAsync<T>(string name) where T: class {
			return ModContent.Request<T>(name, AssetRequestMode.AsyncLoad);
		}
	}
}