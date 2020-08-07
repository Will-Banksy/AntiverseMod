using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AntiverseMod.Projectiles.Ranged;

namespace AntiverseMod.Items.Ammo
{
    public class ShroomiteBulletItem : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 12;
			item.ranged = true;
			item.width = 8;
			item.maxStack = 999;
			item.consumable = true;
			item.height = 16;
			item.shoot = ModContent.ProjectileType<ShroomiteBullet>();
			item.shootSpeed = 10f;
			item.knockBack = 4;
			item.value = Item.sellPrice(silver: 1);
			item.rare = 7;
			item.ammo = AmmoID.Bullet;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomite Bullet");
			Tooltip.SetDefault("Pierces enemies and emits mushrooms");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShroomiteBar, 1);
			recipe.SetResult(this, 100);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
}