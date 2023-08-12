using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AntiverseMod.Projectiles.Magic; 

public class StoneSpear : ModProjectile
{
	public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ShadowBeamFriendly;

	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Stone Spear");
	}

	public override void SetDefaults()
	{
		Projectile.alpha = 255;
		Projectile.width = 40;
		Projectile.height = 40;
		//Projectile.aiStyle = 91;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.MaxUpdates = 3;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
	}

	public override void AI()
	{
		Vector2 center10 = Projectile.Center;
		Projectile.scale = 1f - Projectile.localAI[0];
		Projectile.width = (int)(20f * Projectile.scale);
		Projectile.height = Projectile.width;
		Projectile.position.X = center10.X - (float)(Projectile.width / 2);
		Projectile.position.Y = center10.Y - (float)(Projectile.height / 2);
		if ((double)Projectile.localAI[0] < 0.1)
		{
			Projectile.localAI[0] += 0.01f;
		}

		else
		{
			Projectile.localAI[0] += 0.025f;
		}
		if (Projectile.localAI[0] >= 0.95f)
		{
			Projectile.Kill();
		}

		Projectile.velocity.X = Projectile.velocity.X + Projectile.ai[0] * 1.5f;
		Projectile.velocity.Y = Projectile.velocity.Y + Projectile.ai[1] * 1.5f;
		if (Projectile.velocity.Length() > 16f)
		{
			Projectile.velocity.Normalize();
			Projectile.velocity *= 16f;
		}
		Projectile.ai[0] *= 1.05f;
		Projectile.ai[1] *= 1.05f;
		if (Projectile.scale < 1f)
		{
			int num892 = 0;
			while ((float)num892 < Projectile.scale * 10f)
			{
				int num893 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Stone, Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), 1.1f);
				Main.dust[num893].position = (Main.dust[num893].position + Projectile.Center) / 2f;
				Main.dust[num893].noGravity = true;
				Dust dust = Main.dust[num893];
				dust.velocity *= 0.1f;
				dust = Main.dust[num893];
				dust.velocity -= Projectile.velocity * (1.3f - Projectile.scale);
				Main.dust[num893].fadeIn = (float)(100 + Projectile.owner);
				dust = Main.dust[num893];
				dust.scale += Projectile.scale * 0.75f;
				int num3 = num892;
				num892 = num3 + 1;

			}
			return;
		}
	}
}