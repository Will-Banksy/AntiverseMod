using AntiverseMod.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.Buffs;

/// <summary>
/// Damages the enemy with an explosion when either that explosion would kill them or the debuff runs out. Multiple
/// applications increase damage.
/// </summary>
public class RuneMarked : ModBuff {
	public const int RuneBaseDamage = 20;
	public const int RuneBaseDuration = 120;

	public override void SetStaticDefaults() {
		Main.pvpBuff[Type] = true;
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
		BuffID.Sets.CanBeRemovedByNetMessage[Type] = true;
	}

	public override bool ReApply(NPC npc, int time, int buffIndex) {
		RuneMarkedGlobalNPC global = npc.GetGlobalNPC<RuneMarkedGlobalNPC>();
		global.runeDamage += RuneBaseDamage;
		global.runeDamage = (int)((float)global.runeDamage * 1.1);
		return false;
	}

	public override void Update(NPC npc, ref int buffIndex) {
		RuneMarkedGlobalNPC global = npc.GetGlobalNPC<RuneMarkedGlobalNPC>();
		int damage = global.runeDamage;
		int playerSource = global.playerSource;
		NPC.HitInfo hitInfo = npc.CalculateHitInfo(damage, 0, false, 0, DamageClass.Magic, false, 0);

		if(npc.buffTime[buffIndex] == 0 || hitInfo.Damage >= npc.life) {
			Main.player[playerSource].StrikeNPCDirect(npc, hitInfo);
			npc.RequestBuffRemoval(ModContent.BuffType<RuneMarked>());
			buffIndex--;
			for(int i = 0; i < 16; i++) {
				Vector2 vel = Helper.FromPolar(Main.rand.NextFloat(Helper.TWO_PI), Main.rand.NextFloat(6));
				Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, DustID.RuneWizard, vel.X, vel.Y)].noGravity = true;
			}
		}
	}

	public override void Update(Player player, ref int buffIndex) {
		// TODO: Implement Rune Marked for players
	}
}

class RuneMarkedGlobalNPC : GlobalNPC {
	public int runeDamage = RuneMarked.RuneBaseDamage;
	public int playerSource = -1;

	public override bool InstancePerEntity => true;

	public override void ResetEffects(NPC npc) {
		if(!npc.HasBuff<RuneMarked>()) {
			runeDamage = RuneMarked.RuneBaseDamage;
			playerSource = -1;
		}
	}
}
