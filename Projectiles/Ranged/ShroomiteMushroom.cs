using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AntiverseMod.Dusts;

namespace AntiverseMod.Projectiles.Ranged
{
    public class ShroomiteMushroom : ModProjectile
	{
        public override string Texture => "Terraria/Projectile_" + ProjectileID.Mushroom;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomite Mushroom");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1; // One hit per npc
            projectile.timeLeft = 60;
		}
		
		public override void AI()
		{
            projectile.rotation += 0.1f;
            Lighting.AddLight(projectile.Center, new Vector3(0, 0.1f, 0.3f));
            projectile.alpha = (int)Helper.Map.Linear(projectile.timeLeft, 60, 0, 0, 255);
            projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Zero, 0.15f);
		}

        public override Color? GetAlpha(Color lightColor)
        {
            int rgba = 255 - projectile.alpha;
            return new Color(2 * rgba, 2 * rgba, 2 * rgba, rgba);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
        }
	}
}