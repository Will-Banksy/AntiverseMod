using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using Terraria;

namespace AntiverseMod.UI
{
	public class AntiverseUI : ModSystem
	{
		private UserInterface ui = null;
		private UIState rosaryBraceletUI = null;

		public override void Load()
		{
			if(!Main.dedServ) { // TODO: Figure out what Main.dedServ means
				rosaryBraceletUI = new RosaryBraceletUI();
				rosaryBraceletUI.Activate();

				ui = new UserInterface();
				ui.SetState(rosaryBraceletUI);
			}
		}

		public override void Unload()
		{
			ui = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
			ui?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1)
			{
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
					"Antiverse Mod: Rosary Bead UI",
					delegate {
						ui.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}