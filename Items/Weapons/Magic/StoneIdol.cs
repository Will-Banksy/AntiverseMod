using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AntiverseMod.Projectiles.Magic;

namespace AntiverseMod.Items.Weapons.Magic
{
    public class StoneIdol : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 10;
			item.magic = true;
			item.mana = 4;
			item.width = 50;
			item.height = 50;
			item.useTime = 20;
			item.useAnimation = 40;
			item.useStyle = 5;
			item.knockBack = 4;
			item.shoot = ModContent.ProjectileType<StoneSpear>();
			item.shootSpeed = 8f;
			item.value = Item.sellPrice(silver: 7, copper: 50);
			item.rare = 1;
			item.UseSound = SoundID.Item103;
			item.autoReuse = true;
			item.useTurn = true;
			item.noMelee = true;
			item.noUseGraphic = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Idol");
			Tooltip.SetDefault("Summons curving stone spears to strike your foes");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 vector11 = new Vector2(speedX, speedY);
			vector11.Normalize();
			Vector2 value5 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
			value5.Normalize();
			vector11 = vector11 * 4f + value5;
			vector11.Normalize();
			vector11 *= item.shootSpeed;
			float num152 = (float)Main.rand.Next(10, 80) * 0.001f;
			if (Main.rand.Next(2) == 0)
			{
				num152 *= -1f;
			}
			float num153 = (float)Main.rand.Next(10, 80) * 0.001f;
			if (Main.rand.Next(2) == 0)
			{
				num153 *= -1f;
			}
            Projectile.NewProjectile(player.Center.X, player.Center.Y, vector11.X, vector11.Y, type, damage, knockBack, Main.myPlayer, num153, num152);
			return false;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(3, 30);
			recipe.AddIngredient(75, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}