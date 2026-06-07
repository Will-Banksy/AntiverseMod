using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AntiverseMod.Items;

public class TestItem : ModItem {
	public override void SetDefaults() {
		Item.width = 32;
		Item.height = 32;
		Item.useTime = 10;
		Item.useAnimation = 10;
		Item.useStyle = ItemUseStyleID.Swing;
	}

	public override bool? UseItem(Player player) {
		if(player.whoAmI == Main.myPlayer) {
			foreach(Player plr in Main.player) {
				if(plr.active) {
					Main.NewText("Player \"" + plr.name + "\" hostility: " + plr.hostile + ", team: " + plr.team);
				}
			}

			Vector2 ab = Main.MouseWorld - player.position;
			Main.NewText($"Player pos: ({player.position.X}, {player.position.Y}), mouse pos: ({Main.MouseWorld.X}, {Main.MouseWorld.Y}), ab: ({ab.X}, {ab.Y})");
		}

		return true;
	}
}
