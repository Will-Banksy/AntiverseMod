using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using AntiverseMod.Projectiles.Magic;

namespace AntiverseMod.Items.Weapons.Magic
{
	public class StoneIdol : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.crit = 3;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 4;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 20;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
			Item.shoot = ModContent.ProjectileType<StoneSpear>();
			Item.shootSpeed = 8f;
			Item.value = Item.sellPrice(silver: 7, copper: 50);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item103;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Idol");
			Tooltip.SetDefault("Summons curving stone spears to strike your foes");
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
		{
			velocity.Normalize();
			Vector2 value5 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
			value5.Normalize();
			velocity = velocity * 4f + value5;
			velocity.Normalize();
			velocity *= Item.shootSpeed;
			float num152 = (float)Main.rand.Next(10, 80) * 0.001f;
			if (Main.rand.NextBool(2))
			{
				num152 *= -1f;
			}
			float num153 = (float)Main.rand.Next(10, 80) * 0.001f;
			if (Main.rand.NextBool(2))
			{
				num153 *= -1f;
			}
			Projectile.NewProjectile(source, player.Center.X, player.Center.Y, velocity.X, velocity.Y, type, damage, knockBack, Main.myPlayer, num153, num152);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StoneBlock, 30);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}