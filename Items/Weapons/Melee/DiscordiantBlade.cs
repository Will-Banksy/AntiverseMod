using AntiverseMod.Common;
using AntiverseMod.Projectiles.Melee;
using AntiverseMod.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.Items.Weapons.Melee;

// TODO: Introduce uncommon "Spectral Elemental" (or something along those lines) that replaces skeleton archer/armoured skeleton spawns in the hallow after a certain boss as the NPC that drops this weapon
//       Alternative - rare Entropic Spirit that drops some form of "Chaos Essence" that is used to craft Discordiant Blade & other chaos-themed items like Entropy Bombs

public class DiscordiantBlade : ModItem {
	public const int TeleportSlashDuration = 20;
	public const int TeleportSlashRange = 400;
	public const int ChaosStateSelfInflictDuration = 240;
	public const int ChaosStatePvpInflictDuration = 240;

	private Vector2 teleportPos = new Vector2();
	private Vector2 returnPos = new Vector2();
	private bool teleported = false;

	public override void SetDefaults() {
		Item.damage = 100;
		Item.crit = 3;
		Item.DamageType = DamageClass.Melee;
		Item.mana = 0;
		Item.width = 56;
		Item.height = 70;
		Item.useTime = 10;
		Item.useAnimation = 10;
		Item.reuseDelay = 0;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.knockBack = 4;
		Item.shoot = ProjectileID.None;
		Item.shootSpeed = 0f;
		Item.value = Item.sellPrice(gold: 8);
		Item.rare = ItemRarityID.Lime;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.useTurn = true;
		Item.noMelee = false;
		Item.noUseGraphic = false;
	}

	public override bool AltFunctionUse(Player player) {
		return true;
	}

	public override bool CanUseItem(Player player) {
		if(player.altFunctionUse == 2) {
			if(player.HasBuff(BuffID.ChaosState)) {
				return false;
			}

			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		} else {
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.noMelee = false;
			Item.noUseGraphic = false;
		}
		return true;
	}

	public override bool? UseItem(Player player) {
		if(player.altFunctionUse == 2) {
			Vector2 destination = player.Center + (Main.MouseWorld - player.Center).WithLength(TeleportSlashRange);
			teleportPos = destination - player.Size / 2;
			int tpStyle = TeleportationStyleID.QueenSlimeHook;
			returnPos = player.position;
			player.direction = player.Center.X > destination.X ? -1 : 1;

			Helper.TeleportPlayer(player, teleportPos, tpStyle);
			teleported = true;
			player.GetModPlayer<AntiversePlayer>().noGravity = true;

			Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.position, Vector2.Zero,
				ModContent.ProjectileType<DiscordiantBladeSlash>(), Item.damage * 8, Item.knockBack * 2, player.whoAmI);

			player.AddBuff(BuffID.ChaosState, ChaosStateSelfInflictDuration, false);
		}
		return true;
	}

	public override void UseStyle(Player player, Rectangle heldItemFrame) {
		if(player.altFunctionUse == 2 && teleported && player.itemTime == 1) {
			Vector2 movedFromTpPos = player.position - teleportPos;
			Vector2 finalPos = returnPos + movedFromTpPos;

			if(Collision.SolidTiles(finalPos + player.Size / 2, 1, 1, true)) {
				finalPos = returnPos;
			}

			Helper.TeleportPlayer(player, finalPos, TeleportationStyleID.QueenSlimeHook);
			teleported = false;
			player.GetModPlayer<AntiversePlayer>().noGravity = false;
		}
		base.UseStyle(player, heldItemFrame);
	}

	public override void MeleeEffects(Player player, Rectangle hitbox) {
		if(Main.rand.NextBool(4)) {
			Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.TeleportationPotion);
		}
	}

	public override void ModifyWeaponDamage(Player player, ref StatModifier damage) {
		if(player.HasBuff(BuffID.ChaosState)) {
			damage *= 1.15f;
		}
	}

	public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) {
		target.AddBuff(BuffID.ChaosState, ChaosStatePvpInflictDuration, false);
	}

	public override bool MeleePrefix() {
		return true;
	}
}
