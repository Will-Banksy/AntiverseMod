using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AntiverseMod.Projectiles.Miscellaneous;
using ReLogic.Content;

namespace AntiverseMod.Items.Miscellaneous;

public class GravityGun : ModItem {
	Texture2D glowmask = AntiverseMod.RequestAsset<Texture2D>("AntiverseMod/Items/Miscellaneous/GravityGun_Glowmask").Value;

	public override void SetDefaults() {
		Item.useTime = 20;
		Item.useAnimation = 20;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTurn = true;
		Item.autoReuse = false;
		Item.channel = true;
		Item.width = 132;
		Item.height = 50;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.master = true;

		Item.shoot = ModContent.ProjectileType<GravityGunProjectile>();
		Item.shootSpeed = 8f;
	}

	public override bool AltFunctionUse(Player player) {
		return true;
	}

	public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
		// Draw the glowmask
		spriteBatch.Draw
		(
			glowmask,
			new Vector2
			(
				Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
				Item.position.Y - Main.screenPosition.Y + Item.height - glowmask.Height * 0.5f + 1f
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