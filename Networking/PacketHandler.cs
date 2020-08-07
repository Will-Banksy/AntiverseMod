using System.IO;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using AntiverseMod.Projectiles.Miscellaneous;
using Microsoft.Xna.Framework;

namespace AntiverseMod.Networking
{
    public static class PacketHandler
    {
		// double, long - 8 bytes
		// float, int - 4 bytes
		// short - 2 bytes

        public static void HandleMoveEntity<T>(T[] entityArray, BinaryReader reader, int playerWhoAmI) where T : Entity
		{
			ushort entityId = reader.ReadUInt16();
			int posX = reader.ReadInt32();
			int posY = reader.ReadInt32();
			short velX = reader.ReadInt16();
			short velY = reader.ReadInt16();
			entityArray[entityId].position.X = posX;
			entityArray[entityId].position.Y = posY;
			entityArray[entityId].velocity.X = velX;
			entityArray[entityId].velocity.Y = velX;
		}

		public static void HandleGravityBeam(BinaryReader reader, int playerWhoAmI)
		{
			ushort projectileID = reader.ReadUInt16();
			if(Main.projectile[projectileID].type == ModContent.ProjectileType<GravityGunProjectileBolt>())
			{
				Main.NewText("PACKET ERROR - PROJECTILE INCORRECT TYPE");
				return;
			}

			ushort beamLen = reader.ReadUInt16();
			List<GravityGunProjectileBolt.BoltPoint> bolt = new List<GravityGunProjectileBolt.BoltPoint>();
			for(int i = 0; i < beamLen; i++)
			{
				Vector2 position = reader.ReadVector2();
				float rotation = MathHelper.ToRadians(reader.ReadUInt16()); // Store rotation as degrees, in ushort, to use less bytes than a float
				bolt.Add(new GravityGunProjectileBolt.BoltPoint(position, rotation));
			}
			GravityGunProjectileBolt modProjectile = (GravityGunProjectileBolt)Main.projectile[projectileID].modProjectile;
			modProjectile.bolt = bolt;
		}
    }
}