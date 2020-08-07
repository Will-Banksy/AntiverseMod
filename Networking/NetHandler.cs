using System.IO;
using Terraria;
using AntiverseMod.Networking;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AntiverseMod.Networking
{
	public static class NetHandler
	{
		public static void RecievePacket(BinaryReader reader, int playerWhoAmI)
		{
			byte id = reader.ReadByte();

			switch(id)
			{
				case PacketID.NPCSetPosVel:
					PacketHandler.HandleMoveEntity(Main.npc, reader, playerWhoAmI);
					break;

				case PacketID.ItemSetPosVel:
					PacketHandler.HandleMoveEntity(Main.item, reader, playerWhoAmI);
					break;

				case PacketID.GravityGunBolt:
					PacketHandler.HandleGravityBeam(reader, playerWhoAmI);
					break;
			}
		}

		public static void SendPacket(ModPacket writer, byte id, object[] data, int toClient = -1, int excludeClient = -1)
		{
			writer.Write(id);
			for(int i = 0; i < data.Length; i++)
			{
				if(data[i] is byte) {
					writer.Write((byte)data[i]);
				} else if(data[i] is short) {
					writer.Write((short)data[i]);
				} else if(data[i] is ushort) {
					writer.Write((ushort)data[i]);
				} else if(data[i] is int) {
					writer.Write((int)data[i]);
				} else if(data[i] is uint) {
					writer.Write((uint)data[i]);
				} else if(data[i] is float) {
					writer.Write((float)data[i]);
				} else if(data[i] is Vector2) {
					writer.WriteVector2((Vector2)data[i]);
				} else if(data[i] is Color) {
					writer.WriteRGB((Color)data[i]);
				} else if(data[i] is double) {
					writer.Write((double)data[i]);
				} else if(data[i] is long) {
					writer.Write((long)data[i]);
				}
			}
			writer.Send(toClient, excludeClient);
		}
	}
}