using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;

namespace AntiverseMod.Items.Miscellaneous.Rosary; 

public abstract partial class RosaryBead : ModItem {
	public enum BeadType {
		NONE,
		CULTIST_FIRE,
		CULTIST_LIGHTNING,
		CULTIST_ICE
	}

	public struct BeadInfo {
		public readonly BeadType type;

		public BeadInfo(BeadType type) {
			this.type = type;
		}

		public int DropItem(Player player) {
			return player.QuickSpawnItem(player.GetSource_DropAsItem(), ItemType(), 1);
		}

		public int ItemType() {
			switch(type) {
				case BeadType.CULTIST_FIRE: return ModContent.ItemType<CultistFireBead>();
				case BeadType.CULTIST_LIGHTNING: return ModContent.ItemType<CultistLightningBead>();
				case BeadType.CULTIST_ICE: return ModContent.ItemType<CultistIceBead>();
				default: return -1;
			}
		}


		/// <summary>
		/// Returns (damage, knockback, useTime, useAnimation, autoReuse &amp; useTurn)
		/// </summary>
		public (int, float, int, int, bool) GetStats() {
			switch(type) {
				case BeadType.CULTIST_FIRE: return (200, 3, 8, 24, true);
				case BeadType.CULTIST_LIGHTNING: return (140, 1, 60, 60, false);
				case BeadType.CULTIST_ICE: return (160, 1, 30, 30, false);
				default: return (0, 0, 0, 0, false);
			}
		}

		/// <summary>
		/// Returns (bead name, bead tooltip)
		/// </summary>
		public (string, string) GetDescription() {
			switch(type) {
				case BeadType.CULTIST_FIRE: return ("Cultist Fire Bead", "Fires homing fireballs");
				case BeadType.CULTIST_LIGHTNING: return ("Cultist Lightning Bead", "Summons a ball of lightning to shoot lightning bolts at your enemies");
				case BeadType.CULTIST_ICE: return ("Cultist Ice Bead", "Summons an ice spike ball to shoot spikes of ice around it");
				default: return ("", "");
			}
		}

		public Texture2D GetTexture() {
			if(type != BeadType.NONE) {
				return TextureAssets.Item[ItemType()].Value;
			} else {
				return AntiverseMod.RequestAsset<Texture2D>("AntiverseMod/Items/Miscellaneous/Rosary/NoBead").Value;
			}
		}

		/// <summary>
		/// When the bead is used
		/// </summary>
		public void Activate(Player player) {
			switch(type) {
				case BeadType.CULTIST_FIRE: {
					Vector2 velocity = Main.MouseWorld - player.Center;
					velocity.Normalize();
					velocity *= 8f;
					(int damage, float knockback, _, _, _) = GetStats();
					Projectile.NewProjectile(new EntitySource_ItemUse(player, player.HeldItem), player.Center, velocity, ProjectileID.BallofFire, damage, knockback, player.whoAmI);
					break;
				}

				case BeadType.CULTIST_LIGHTNING: {
					Vector2 velocity = Main.MouseWorld - player.Center;
					velocity.Normalize();
					velocity *= 8f;
					(int damage, float knockback, _, _, _) = GetStats();
					Projectile.NewProjectile(new EntitySource_ItemUse(player, player.HeldItem), player.Center, velocity, ProjectileID.LightDisc, damage, knockback, player.whoAmI);
					break;
				}

				case BeadType.CULTIST_ICE: {
					Vector2 velocity = Main.MouseWorld - player.Center;
					velocity.Normalize();
					velocity *= 8f;
					(int damage, float knockback, _, _, _) = GetStats();
					Projectile.NewProjectile(new EntitySource_ItemUse(player, player.HeldItem), player.Center, velocity, ProjectileID.IceBolt, damage, knockback, player.whoAmI);
					break;
				}
			}
		}
	}

	public BeadType beadType;

	public RosaryBead(BeadType beadType) {
		this.beadType = beadType;
	}

	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Rosary Bead");
		Tooltip.SetDefault("Rosary Bead");
	}
}