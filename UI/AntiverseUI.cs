using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using Terraria;

namespace AntiverseMod.UI; 

public class AntiverseUI : ModSystem {
	private UserInterface ui;

	public override void Load() {
		if (!Main.dedServ) {
			// TODO: Figure out what Main.dedServ means. Dedicated Server?
			ui = new UserInterface();
		}
	}

	public override void Unload() {
		ui = null;
	}

	public override void UpdateUI(GameTime gameTime) {
		ui?.Update(gameTime);
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		// int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
		// if (mouseTextIndex != -1) {
		// 	layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
		// 		"Antiverse Mod: UI Layer Name",
		// 		delegate {
		// 			ui.Draw(Main.spriteBatch, new GameTime());
		// 			return true;
		// 		},
		// 		InterfaceScaleType.UI
		// 	));
		// }
	}
}