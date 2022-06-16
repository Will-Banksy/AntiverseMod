using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AntiverseMod.Dusts;
using AntiverseMod.Utils;

namespace AntiverseMod.Projectiles.Ranged
{
	public class ShroomiteBullet : MainProjBase
	{
		private bool tangible = false;
		private Vector2 mousePos;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomite Bullet");
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 3;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 2;
			Projectile.aiStyle = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1; // One hit per npc
			Projectile.timeLeft = 300;
			Projectile.alpha = 50;
		}

		// BUG: Sometimes when firing horizontally or vertically some bullets will still be intangible past the cursor
		// To debug slow bullets down to sluggish and probably use draw function to draw debug text to diagnose the issue
		public override void AI()
		{
			if(Projectile.timeLeft == 300)
			{
				if(Main.myPlayer == Projectile.owner)
				{
					mousePos = Main.MouseWorld;
				}
			}
			if(Helper.GoingAwayFrom(mousePos, Projectile.Center, Projectile.velocity))
			{
				tangible = true;
				Projectile.tileCollide = true;
			}
			if(tangible && Projectile.alpha < 255)
			{
				Projectile.alpha += 15;
				if(Projectile.alpha > 255)
				{
					Projectile.alpha = 255;
				}
			}
			Projectile.FaceForward();
			Lighting.AddLight(Projectile.Center, new Vector3(0, 0.1f, 0.3f));
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(Projectile.alpha, Projectile.alpha, Projectile.alpha, Projectile.alpha);
		}

		public override void OnHit(EntityHelper.EntityUnion target, int damage, float? knockback, bool crit, bool pvp = false)
		{
			if(target.type == EntityHelper.EntityUnion.Type.NPC)
			{
				target.NPC().immune[Projectile.owner] = 0;
			}

			for (int i = 0; i < 2; i++)
			{
				//float ang = Projectile.velocity.ToRotation() + Main.rand.NextFloat(-0.4f, 0.4f);
				//float speed = Main.rand.NextFloat(2, 8);
				Vector2 vel = Helper.RandSpread(Projectile.velocity, 0.4f, 3);//Helper.FromPolar(ang, speed);

				int type = ModContent.DustType<ShroomiteDust>();
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type, vel.X, vel.Y);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position); // SoundID.Item10 is the hit tile sound
			return true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(!tangible)
			{
				return false;
			}
			return base.Colliding(projHitbox, targetHitbox);
		}
	}
}