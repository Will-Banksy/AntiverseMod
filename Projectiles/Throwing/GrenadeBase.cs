using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;
using AntiverseMod.Utils;

namespace AntiverseMod.Projectiles.Throwing
{
	public abstract class GrenadeBase : MainProjBase
	{
		private bool exploding = false;
		protected int explosionRadius = 20;
		protected bool explodeOnContact = true;
		private bool projDying = false;

		public override void SetDefaults()
		{
			Projectile.aiStyle = 14; // Same as
		}

		public override void AI()
		{
			if(Projectile.owner == Main.myPlayer && Projectile.timeLeft == 1)
			{
				exploding = true;

				// Expand the Projectile centred around the Projectile centre
				Projectile.position = Projectile.Center;
				Projectile.width = explosionRadius * 2;
				Projectile.height = explosionRadius * 2;
				Projectile.Center = Projectile.position;

				OnExplode();
			}

			if(!exploding)
			{
				base.AI();
			}
		}

		public override bool? CanDamage()
		{
			return exploding | explodeOnContact;
		}

		public override bool? CanCutTiles() {
			return exploding;
		}

		public override void OnHit(EntityHelper.EntityRef target, int damage, float? knockback, bool crit, bool pvp = false) {
			if(!projDying) {
				Projectile.timeLeft = 2;
				projDying = true;
			}
		}

		protected virtual void OnExplode()
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 20; i++)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
			}
			int goreId = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64), 1f);
			Gore gore = Main.gore[goreId];
			gore.velocity.X++;
			gore.velocity.Y++;
			gore.velocity *= 0.3f;

			goreId = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64), 1f);
			gore = Main.gore[goreId];
			gore.velocity.X--;
			gore.velocity.Y++;
			gore.velocity *= 0.3f;

			goreId = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64), 1f);
			gore = Main.gore[goreId];
			gore.velocity.X++;
			gore.velocity.Y--;
			gore.velocity *= 0.3f;

			goreId = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(61, 64), 1f);
			gore = Main.gore[goreId];
			gore.velocity.X--;
			gore.velocity.Y--;
			gore.velocity *= 0.3f;
		}
	}
}
