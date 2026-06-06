using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AntiverseMod.Items.Materials;
using AntiverseMod.Utils;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace AntiverseMod.Common;

public class AntiverseGlobalItem : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type is ItemID.AshBlock
			or ItemID.Katana;
	}

	public override void SetDefaults(Item item) {
		if(item.type == ItemID.AshBlock) {
			ItemID.Sets.ExtractinatorMode[item.type] = item.type;
		}
	}

	public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack) {
		if(extractType == ItemID.AshBlock) {
			resultType = 0;
			resultStack = 1;

			// Previously I had it so that extractinating ash could yield hellstone and obsidian - but that'd allow
			// skipping corruption/crimson boss in terms of tier progression so was removed

			// TODO: A nice helper function to help with percent chances and stuff would be nice
			float rand = Main.rand.NextFloat(100);
			if(rand > 2) { // ~2% chance of getting sassolite
				resultType = ModContent.ItemType<Sassolite>();
			}
		}
	}

	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if(item.type == ItemID.Katana) {
			Helper.InsertStandardTooltip(Mod, tooltips, Mod.GetLocalization("Additional.Katana.AltFunctionTooltip").Value);
		}
	}

	public override bool AltFunctionUse(Item item, Player player) {
		if(item.type == ItemID.Katana) {
			return true;
		}
		return base.AltFunctionUse(item, player);
	}

	public override bool? UseItem(Item item, Player player) {
		// Add a right click use to the Katana to invert the player's X velocity
		if(item.type == ItemID.Katana) {
			if(player.whoAmI == Main.myPlayer && player.altFunctionUse == 2) {
				player.velocity.X = -player.velocity.X;
				player.direction = (int)Helper.DirOf(player.velocity).X;

				return true;
			}
		}

		return base.UseItem(item, player);
	}
}
