using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AntiverseMod.Dusts;

namespace AntiverseMod.Projectiles.Ranged
{
    public class ShroomiteBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomite Bullet");
		}

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = 3;
			projectile.tileCollide = true;
            projectile.extraUpdates = 2;
            projectile.aiStyle = 0;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1; // One hit per npc
            projectile.timeLeft = 300;
		}
		
		public override void AI()
		{
            projectile.FaceForward();
            Lighting.AddLight(projectile.Center, new Vector3(0, 0.1f, 0.3f));
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for(int i = 0; i < 15; i++)
            {
                float ang = projectile.velocity.ToRotation() + Main.rand.NextFloat(-0.4f, 0.4f);
                float speed = Main.rand.NextFloat(2, 8);
                Vector2 vel = Helper.FromPolar(ang, speed);

                int type = ModContent.DustType<ShroomiteDust>();
                Dust.NewDust(projectile.position, projectile.width, projectile.height, type, vel.X, vel.Y);

                if(i == 0 || Main.rand.Next(20) == 0)
                {
                    int proj = Projectile.NewProjectile(projectile.Center, vel * -2, ModContent.ProjectileType<ShroomiteMushroom>(), (int)(projectile.damage * 0.75f), 1, projectile.owner);
                    Main.projectile[proj].localNPCImmunity[target.whoAmI] = 30;
                }
            }

            target.immune[projectile.owner] = 0;
        }

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundID.Item10, projectile.position); // SoundID.Item10 is the hit tile sound
			return true;
		}
	}
}