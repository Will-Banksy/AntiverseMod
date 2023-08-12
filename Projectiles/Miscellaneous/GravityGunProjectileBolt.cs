using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using AntiverseMod.Dusts;
using AntiverseMod.Networking;
using AntiverseMod.Utils;
using Terraria.GameContent;

namespace AntiverseMod.Projectiles.Miscellaneous; 

public class GravityGunProjectileBolt : ModProjectile
{
	public class BoltPoint
	{
		public float rotation;
		public Vector2 position;

		public Rectangle Rect
		{
			get
			{
				return new Rectangle((int)position.X - 24, (int)position.Y - 24, 48, 48);
			}
		}

		public BoltPoint(Vector2 position, float rotation)
		{
			this.rotation = rotation;
			this.position = position;
		}
	}

	public List<BoltPoint> bolt = null;
	private Texture2D texture = null;

	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Gravity Gun Bolt");
	}

	public override void SetDefaults()
	{
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 30;
		Projectile.friendly = true;
		Projectile.hostile = false;
	}

	public override void AI()
	{
		if(texture == null)
			texture = TextureAssets.Projectile[Projectile.type].Value;

		// projectile.velocity = Vector2.Zero;

		// If the client that is attempting to run this code is not owned by the player that owns this projectile. Don't want the server to run it either
		if(bolt == null && Projectile.owner == Main.myPlayer && Main.netMode != NetmodeID.Server)
		{
			bolt = new List<BoltPoint>();

			// bolt = PlotBoltLine(Main.player[projectile.owner].Center, Main.MouseWorld);
			Vector2 start = Projectile.Center;//Main.player[projectile.owner].Center;
			Vector2 end = Main.MouseWorld;
			Vector2 current = start;
			Vector2 unit = Helper.VelocityToPoint(start, end);
			Projectile.velocity = unit;
			float timer = 0;

			while(true)
			{
				float amt = Helper.Map.QuadIn(timer, 0, 1, 0, 1);

				float randomness = 1 - timer;
				float dist = Main.rand.Next(100);
				dist *= randomness;

				Vector2 controlPoint = Vector2.Lerp(start, end, amt);//unit * amt + Projectile.Center;

				Vector2 point = Helper.FromPolar(Main.rand.NextFloat(Helper.TWO_PI), dist, controlPoint);

				bolt.AddRange(PlotBoltLine(current, point));

				current = point;

				if(timer >= 1)
				{
					break;
				}
				timer += 0.2f;
			}

			foreach(BoltPoint point in bolt)
			{
				int i = Dust.NewDust(point.position + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), 1, 1, ModContent.DustType<GravityGunDust>());
				Main.dust[i].noGravity = false;
			}

			Projectile.ai[0] = Projectile.timeLeft;

			if(Main.netMode == NetmodeID.MultiplayerClient)
			{
				// TODO: Change to send ID of packet and use methods in NetHandler and shit
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)PacketID.GravityGunBolt);
				packet.Write((ushort)Projectile.whoAmI);
				packet.Write((ushort)bolt.Count);
				foreach(BoltPoint point in bolt)
				{
					packet.WriteVector2(point.position);
					packet.Write((ushort)MathHelper.ToDegrees(point.rotation));
				}
				packet.Send(); // Send data to server
			}
		}
		else
		{
			Projectile.alpha = (int)Helper.Map.ExpoIn(Projectile.timeLeft, Projectile.ai[0], 0, 255, 0);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		if(bolt != null)
		{
			foreach(BoltPoint point in bolt)
			{
				Main.EntitySpriteDraw
				(
					texture,
					point.position - Main.screenPosition,
					null,
					new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha),
					point.rotation + Helper.HALF_PI,
					texture.Size() * 0.5f,
					1,
					SpriteEffects.None,
					1
				);
			}
		}

		return false;
	}

	private List<BoltPoint> PlotBoltLine(Vector2 start, Vector2 end)
	{
		List<BoltPoint> plot = new List<BoltPoint>();

		int step = texture.Height;
		Vector2 unit = Helper.VelocityToPoint(start, end);
		float angle = unit.ToRotation();
		Vector2 current = start;
		Vector2 dirToEnd = Helper.DirTo(start, end);

		int safetyNet_MaxIterations = 1000;
		int safetyNet_Iteration = 0;

		while(true)
		{
			plot.Add(new BoltPoint(current, angle));
			current += unit * step;

			if(dirToEnd != Helper.DirTo(current, end) || safetyNet_Iteration >= safetyNet_MaxIterations)
			{
				break;
			}

			safetyNet_Iteration++;
		}

		return plot;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		for(int i = 0; i < bolt.Count; i++)
		{
			if(bolt != null)
				if(bolt[i].Rect.Intersects(targetHitbox))
					return true;
		}
		return false;
	}
}