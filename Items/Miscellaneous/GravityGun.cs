using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AntiverseMod.Projectiles.Miscellaneous;

namespace AntiverseMod.Items.Miscellaneous
{
	public class GravityGun : ModItem // TODO: Make some way of getting this
	{
		Texture2D glowmask = ModContent.GetTexture("AntiverseMod/Items/Miscellaneous/GravityGun_Glowmask");

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zero-Point Energy Field Manipulator");
			Tooltip.SetDefault("...Or you can just call it a gravity gun");
		}

		public override void SetDefaults()
		{
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.useTurn = true;
			item.autoReuse = false;
			item.channel = true;
			item.width = 132;
			item.height = 50;
			item.noMelee = true;
			item.noUseGraphic = true;

			item.shoot = ModContent.ProjectileType<GravityGunProjectile>();
			item.shootSpeed = 8f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
			// Draw the glowmask
			spriteBatch.Draw
			(
				glowmask,
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - glowmask.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, glowmask.Width, glowmask.Height),
				Color.White,
				rotation,
				glowmask.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
	}
}