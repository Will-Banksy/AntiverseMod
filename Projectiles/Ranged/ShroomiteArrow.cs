using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AntiverseMod.Utils;
using Terraria.ModLoader;
using Terraria.Audio;

namespace AntiverseMod.Projectiles.Ranged
{
	public class ShroomiteArrow : MainProjBase
	{
		// Using ai[0] as the initial speed of the Projectile

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomite Arrow");
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			// Projectile.extraUpdates = 2;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 300;
		}

		public override void AI()
		{
			if(Projectile.timeLeft == 300)
			{
				Projectile.ai[0] = Projectile.velocity.Length();
			}

			Projectile.FaceForward();
			Lighting.AddLight(Projectile.Center, new Vector3(0, 0.1f, 0.3f));

			Projectile.alpha = (int)Helper.Map.ExpoIn(Projectile.timeLeft, 300, 0, 0, 255);

			// Need to do homing part

			NPC target = null;
			float smallestDist = -1;
			foreach(NPC npc in Main.npc)
			{
				if(npc.CanBeChasedBy(Projectile))
				{
					if(Projectile.Center.AngleTo(npc.Center).AngleBetween(Projectile.velocity.ToRotation() - 0.3f, Projectile.velocity.ToRotation() + 0.3f))
					{
						if(Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
						{
							float dist = Vector2.Distance(Projectile.Center, npc.Center);
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
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Helper.VelocityToPoint(Projectile.Center, target.Center, Projectile.ai[0]), 0.1f);
			}
			else
			{
				Projectile.velocity.Y += 0.1f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			int rgba = 255 - Projectile.alpha;
			return new Color(rgba, rgba, rgba, rgba);
		}

		public override void ModifyHit(EntityHelper.EntityRef target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, bool pvp = false)
		{
			crit = false;
			damage = (int)(damage * Helper.Map.Linear(Projectile.timeLeft, 300, 0, 1, 1.8f));
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position); // SoundID.Item10 is the hit tile sound
			return true;
		}
	}
}