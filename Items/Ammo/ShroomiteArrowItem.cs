using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AntiverseMod.Projectiles.Ranged;

namespace AntiverseMod.Items.Ammo;

public class ShroomiteArrowItem : ModItem {
	public override void SetDefaults() {
		Item.damage = 12;
		Item.DamageType = DamageClass.Ranged;
		Item.width = 8;
		Item.maxStack = 999;
		Item.consumable = true;
		Item.height = 16;
		Item.shoot = ModContent.ProjectileType<ShroomiteArrow>();
		Item.shootSpeed = 10f;
		Item.knockBack = 4;
		Item.value = Item.sellPrice(silver: 1);
		Item.rare = ItemRarityID.Lime;
		Item.ammo = AmmoID.Arrow;
	}

	public override void AddRecipes() {
		Recipe recipe = CreateRecipe(100);
		recipe.AddIngredient(ItemID.ShroomiteBar, 1);
		// recipe.SetResult(this, 100);
		recipe.AddTile(TileID.MythrilAnvil);
		recipe.Register();
	}
}