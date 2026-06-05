using AntiverseMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.Items.Weapons.Magic;

public class RunicTome : ModItem {
	public override void SetDefaults() {
		Item.damage = 100;
		Item.crit = 3;
		Item.DamageType = DamageClass.Magic;
		Item.mana = 24;
		Item.width = 28;
		Item.height = 30;
		Item.useTime = 20;
		Item.useAnimation = 60;
		Item.reuseDelay = 40;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.knockBack = 4;
		Item.shoot = ProjectileID.RuneBlast;
		Item.shootSpeed = 10f;
		Item.value = Item.sellPrice(gold: 8);
		Item.rare = ItemRarityID.Pink;
		Item.UseSound = SoundID.Item28;
		Item.autoReuse = true;
		Item.useTurn = true;
		Item.noMelee = true;
		Item.noUseGraphic = false;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack) {
		Projectile proj = Main.projectile[Projectile.NewProjectile(source, player.Center.X, player.Center.Y, velocity.X, velocity.Y, type, damage, knockBack, Main.myPlayer)];
		proj.friendly = true;
		proj.hostile = false;
		proj.penetrate = 3;
		proj.timeLeft = 300;
		return false;
	}
}
