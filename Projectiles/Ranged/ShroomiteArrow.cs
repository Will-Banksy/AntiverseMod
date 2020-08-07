using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AntiverseMod.Dusts;

namespace AntiverseMod.Projectiles.Ranged
{
	public class ShroomiteArrow : ModProjectile
	{
		float initSpeed = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomite Arrow");
		}

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			// projectile.extraUpdates = 2;
			projectile.aiStyle = 0;
			projectile.timeLeft = 300;
		}
		
		public override void AI()
		{
			if(projectile.timeLeft == 300)
			{
				initSpeed = projectile.velocity.Length();
			}

			projectile.FaceForward();
			Lighting.AddLight(projectile.Center, new Vector3(0, 0.1f, 0.3f));

			projectile.alpha = (int)Helper.Map.ExpoIn(projectile.timeLeft, 300, 0, 0, 255);

			// Need to do homing part

			NPC target = null;
			float smallestDist = -1;
			foreach(NPC npc in Main.npc)
			{
				if(npc.CanBeChasedBy(projectile))
				{
					if(projectile.Center.AngleTo(npc.Center).AngleBetween(projectile.velocity.ToRotation() - 0.3f, projectile.velocity.ToRotation() + 0.3f))
					{
						if(Collision.CanHit(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
						{
							float dist = Vector2.Distance(projectile.Center, npc.Center);
							if(dist < smallestDist || smallestDist == -1)
							{
								target = npc;
								smallestDist = dist;
							}
						}
					}
				}
			}

			if(target != null)
			{
				projectile.velocity = Vector2.Lerp(projectile.velocity, Helper.VelocityToPoint(projectile.Center, target.Center, initSpeed), 0.1f);
			}
			else
			{
				projectile.velocity.Y += 0.1f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			int rgba = 255 - projectile.alpha;
			return new Color(rgba, rgba, rgba, rgba);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			crit = false;
			damage = (int)(damage * Helper.Map.Linear(projectile.timeLeft, 300, 0, 1, 1.8f));
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundID.Item10, projectile.position); // SoundID.Item10 is the hit tile sound
			return true;
		}
	}
}