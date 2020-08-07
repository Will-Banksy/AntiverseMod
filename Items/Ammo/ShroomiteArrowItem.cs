using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AntiverseMod.Projectiles.Ranged;

namespace AntiverseMod.Items.Ammo
{
    public class ShroomiteArrowItem : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 12;
			item.ranged = true;
			item.width = 8;
			item.maxStack = 999;
			item.consumable = true;
			item.height = 16;
			item.shoot = ModContent.ProjectileType<ShroomiteArrow>();
			item.shootSpeed = 10f;
			item.knockBack = 4;
			item.value = Item.sellPrice(silver: 1);
			item.rare = 7;
			item.ammo = AmmoID.Arrow;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomite Arrow");
			Tooltip.SetDefault("Slightly homing and does more damage the longer it's in the air");
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