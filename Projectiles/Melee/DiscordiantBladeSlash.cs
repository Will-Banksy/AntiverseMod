using AntiverseMod.Items.Weapons.Melee;
using AntiverseMod.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.Projectiles.Melee;

public class DiscordiantBladeSlash : ModProjectile {
	private const int NumFrames = 10;

	private int frameDuration = 0;
	private Vector2 targetPosScreen = new();

	public override void SetStaticDefaults() {
		Main.projFrames[Projectile.type] = NumFrames;
	}

	public override void SetDefaults() {
		Projectile.alpha = 0;
		Projectile.width = 140;
		Projectile.height = 226;
		//Projectile.aiStyle = 91;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.MaxUpdates = 1;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = DiscordiantBlade.TeleportSlashDuration;

		frameDuration = DiscordiantBlade.TeleportSlashDuration / NumFrames;
		Projectile.frameCounter = frameDuration;
	}

	public override void AI() {
		if(Projectile.timeLeft >= DiscordiantBlade.TeleportSlashDuration - 1) {
			targetPosScreen = Main.MouseScreen;
		}

		if(Main.myPlayer == Projectile.owner) {
			Player owner = Main.player[Projectile.owner];
			Vector2 targetPos = Main.screenPosition + targetPosScreen;
			Vector2 toTarget = targetPos - owner.Center;
			toTarget.Normalize();

			Projectile.direction = (int)Helper.DirOf(toTarget).X;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = toTarget.ToRotation() + (Projectile.direction == 1 ? 0 : Helper.PI);

			owner.direction = Projectile.direction;

			Vector2 spriteOrigin = Projectile.Center + new Vector2((-Projectile.spriteDirection) * (Projectile.width / 2), 0);
			Vector2 centreToOrigin = Projectile.Center - spriteOrigin;
			Vector2 rotCentreToOrigin = centreToOrigin.RotatedBy(toTarget.ToRotation());

			Projectile.position = (owner.Center - toTarget * 40) - Projectile.Size / 2;
			Projectile.position += Projectile.direction * rotCentreToOrigin;
		}

		Projectile.frameCounter--;
		if(Projectile.frameCounter == 0) {
			Projectile.frameCounter = frameDuration;
			Projectile.frame = ++Projectile.frame % NumFrames;
		}

		Rectangle hitbox = Projectile.Hitbox;
		ModifyDamageHitbox(ref hitbox);
		Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.TeleportationPotion);
	}

	public override void ModifyDamageHitbox(ref Rectangle hitbox) {
		float arcDir = Projectile.spriteDirection;

		Player owner = Main.player[Projectile.owner];

		Vector2 targetPos = Main.screenPosition + targetPosScreen;
		Vector2 playerToTarget = (targetPos - owner.Center).WithLength(70);
		playerToTarget = playerToTarget.RotatedBy(Helper.Map.Linear(Projectile.timeLeft, DiscordiantBlade.TeleportSlashDuration, 0,
			Helper.HALF_PI * -arcDir, Helper.HALF_PI * arcDir));
		Vector2 hitboxCentre = owner.Center + playerToTarget;

		hitbox.Width = 70;
		hitbox.Height = 70;
		hitbox.X = (int)hitboxCentre.X - hitbox.Width / 2;
		hitbox.Y = (int)hitboxCentre.Y - hitbox.Height / 2;
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo info) {
		target.AddBuff(BuffID.ChaosState, DiscordiantBlade.ChaosStatePvpInflictDuration, false);
	}
}
