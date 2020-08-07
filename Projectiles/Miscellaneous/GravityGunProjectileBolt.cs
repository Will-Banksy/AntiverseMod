using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using AntiverseMod.Dusts;
using AntiverseMod.Networking;

namespace AntiverseMod.Projectiles.Miscellaneous
{
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
			projectile.width = 2;
			projectile.height = 2;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 30;
			projectile.friendly = true;
			projectile.hostile = false;
		}
		
		public override void AI()
		{
			if(texture == null)
				texture = Main.projectileTexture[projectile.type];

			// projectile.velocity = Vector2.Zero;
			
			// If the client that is attempting to run this code is not owned by the player that owns this projectile. Don't want the server to run it either
			if(bolt == null && projectile.owner == Main.myPlayer && Main.netMode != NetmodeID.Server)
			{
				bolt = new List<BoltPoint>();

				// bolt = PlotBoltLine(Main.player[projectile.owner].Center, Main.MouseWorld);
				Vector2 start = projectile.Center;//Main.player[projectile.owner].Center;
				Vector2 end = Main.MouseWorld;
				Vector2 current = start;
				Vector2 unit = Helper.VelocityToPoint(start, end);
				projectile.velocity = unit;
				float timer = 0;

				while(true)
				{
					float amt = Helper.Map.QuadIn(timer, 0, 1, 0, 1);

					float randomness = 1 - timer;
					float dist = Main.rand.Next(100);
					dist *= randomness;

					Vector2 controlPoint = Vector2.Lerp(start, end, amt);//unit * amt + projectile.Center;

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

				projectile.ai[0] = projectile.timeLeft;

				if(Main.netMode == NetmodeID.MultiplayerClient)
				{
					ModPacket packet = mod.GetPacket();
					packet.Write((ushort)projectile.whoAmI);
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
				projectile.alpha = (int)Helper.Map.ExpoIn(projectile.timeLeft, projectile.ai[0], 0, 255, 0);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if(bolt != null)
			{
				foreach(BoltPoint point in bolt)
				{
					spriteBatch.Draw
					(
						texture,
						point.position - Main.screenPosition,
						null,
						new Color(projectile.alpha, projectile.alpha, projectile.alpha, projectile.alpha),
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
				if(bolt[i].Rect.Intersects(targetHitbox))
					return true;
			}
			return false;
		}
	}
}