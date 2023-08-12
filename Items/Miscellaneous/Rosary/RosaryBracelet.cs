using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AntiverseMod.UI;

// INFO: Does this need to be a bracelet? Can it not be a typical rosary that one wears around the neck? Using it being holding it?

// TODO: STILL NEED TO MAKE THE BEAD SPRITES SMALLER LMAO THEY'RE LIKE AS BIG AS THE PLAYER

// TODO: Also add mechanism of removing beads

// TODO: Also make sure that the item actually saves its state (beads)

// TODO: Actually just make this a good accessory or something I cba making a complicated weapon like my original plans for this

namespace AntiverseMod.Items.Miscellaneous.Rosary; 

public class RosaryBracelet : ModItem {
	public RosaryBead.BeadInfo[] beads = new RosaryBead.BeadInfo[MAX_BEADS] {
		new RosaryBead.BeadInfo(RosaryBead.BeadType.CULTIST_FIRE),
		new RosaryBead.BeadInfo(RosaryBead.BeadType.CULTIST_LIGHTNING),
		new RosaryBead.BeadInfo(RosaryBead.BeadType.CULTIST_ICE),
		new RosaryBead.BeadInfo(RosaryBead.BeadType.NONE),
		new RosaryBead.BeadInfo(RosaryBead.BeadType.NONE)
	};

	public const int MAX_BEADS = 5;
	private int selected = 0;

	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Rosary Bracelet");
		Tooltip.SetDefault("A rosary bracelet that bears a pendant depicting a full moon\nBEAD ATTACH/DETACH INSTRUCTIONS");
	}

	public override void SetDefaults()
	{
		Item.rare = ItemRarityID.Red; // Dropped from Lunatic Cultist
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.width = 42;
		Item.height = 52;
		Item.noMelee = true;
		Item.DamageType = DamageClass.Generic;

		UpdateBeads();
	}

	public void SelectBead(int index) {
		selected = index;
		UpdateBeads();
		Main.NewText("Selected Bead: " + beads[selected].GetDescription().Item1);
	}

	public void UpdateBeads() {
		if(beads.Length == 0) {
			Item.useTime = 0;
			Item.useAnimation = 0;
			Item.autoReuse = false;
			Item.useTurn = false;
			Item.damage = 0;
			Item.knockBack = 0;
			return;
		}

		if(selected >= beads.Length) {
			selected = 0;
		} else if(selected < 0) {
			selected = beads.Length - 1;
		}

		(int damage, float knockback, int useTime, int useAnimation, bool autoReuse) = beads[selected].GetStats();

		Item.useTime = useTime;
		Item.useAnimation = useAnimation;
		Item.autoReuse = autoReuse;
		Item.useTurn = autoReuse;
		Item.damage = damage;
		Item.knockBack = knockback;

		(string name, string tooltip) = beads[selected].GetDescription();

		Item.SetNameOverride("Rosary Bracelet (" + name + ")");
		Item.RebuildTooltip();
	}

	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		int ttidx = tooltips.FindIndex((line) => line.Name == "Tooltip1");
		if(ttidx == -1) {
			tooltips.Add(new TooltipLine(Mod, "Tooltip1", "" + beads[selected].GetDescription().Item2));
		} else {
			tooltips[ttidx].Text = "" + beads[selected].GetDescription().Item2;
		}
	}

	public override bool AltFunctionUse(Player player)
	{
		return true;
	}

	public override void UseAnimation(Player player)
	{
		if(beads.Length == 0) {
			return;
		}

		if(player.altFunctionUse == 2) {
			if(!RosaryBraceletUI.Visible) {
				RosaryBraceletUI.Show(this);
			} else {
				RosaryBraceletUI.Hide();
			}
		}
	}

	public override bool CanUseItem(Player player)
	{
		return !RosaryBraceletUI.Visible || player.altFunctionUse == 2;
	}

	public override bool? UseItem(Player player)
	{
		if(beads.Length == 0) {
			return false;
		}

		if(player.altFunctionUse != 2) {
			beads[selected].Activate(player);
		}

		return true;
	}
}