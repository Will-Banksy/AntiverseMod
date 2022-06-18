using System.Globalization;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using AntiverseMod.Items.Miscellaneous.Rosary;
using AntiverseMod.Utils;
using Terraria.UI.Chat;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;

namespace AntiverseMod.UI
{
	public class RosaryBraceletUI : UIState
	{
		private class BeadButton : UIPanel {
			private int idx = -1;

			public Vector2 Centre {
				get {
					return new Vector2(Left.Pixels + Width.Pixels / 2, Top.Pixels + Height.Pixels / 2);
				}
				set {
					Left.Set(value.X - Width.Pixels / 2, 0f);
					Top.Set(value.Y - Height.Pixels / 2, 0f);
				}
			}

			public BeadButton(int idx) {
				this.idx = idx;
			}

			public override void Click(UIMouseEvent evt)
			{
				if(!Visible) {
					return;
				}

				if(Main.mouseItem.ModItem is RosaryBead) {
					// Switch items
					RosaryBead.BeadType mouseBeadType = ((RosaryBead)Main.mouseItem.ModItem).beadType;

					Main.mouseItem.TurnToAir();

					if(controllingBracelet.beads[idx].type != RosaryBead.BeadType.NONE) {
						Main.mouseItem = new Item(controllingBracelet.beads[idx].ItemType());
					}

					controllingBracelet.beads[idx] = new RosaryBead.BeadInfo(mouseBeadType);
					controllingBracelet.UpdateBeads();
				} else if(controllingBracelet.beads[idx].type != RosaryBead.BeadType.NONE) {
					// Switch to hovered bead
					controllingBracelet.SelectBead(idx);
					Hide();
				}

				base.Click(evt);
			}

			/// TODO: Draw the rope of the rosary and the pendant
			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				Texture2D tex = controllingBracelet.beads[idx].GetTexture();

				Vector2 centre = Centre;
				Width.Set(tex.Width, 0f);
				Height.Set(tex.Height, 0f);
				Centre = centre;

				// FIXME: AAAAARRRRRRGGGGGGHHHHHH WHY WON'T THE FUCKING SPRITES DRAW WITHOUT BEING SLIGHTLY SCALED DOWN AND BLURRED????!??!?!??!
				spriteBatch.Draw(
					tex,
					new Vector2((int)Left.Pixels, (int)Top.Pixels),
					new Rectangle(0, 0, tex.Width, tex.Height),
					Color.White,
					0f,
					Vector2.Zero,
					1f,
					SpriteEffects.None,
					0f
				);
				// Item item = idx >= controllingBracelet.beads.Length ? new Item() : new Item(controllingBracelet.beads[idx].ItemType());
				// ItemSlot.Draw(spriteBatch, ref item, ItemSlot.Context.MouseItem, new Vector2(Left.Pixels, Top.Pixels));
				if(IsMouseHovering) {
					if(controllingBracelet.beads[idx].type == RosaryBead.BeadType.NONE) {
						Main.hoverItemName = "Empty Slot";
					} else {
						Main.hoverItemName = controllingBracelet.beads[idx].GetDescription().Item1;
					}
				}
			}
		}

		public static bool Visible { get; private set; }

		private static RosaryBracelet controllingBracelet = null;

		private static BeadButton[] beadButtons = new BeadButton[RosaryBracelet.MAX_BEADS];

		public override void OnInitialize()
		{
			for (int i = 0; i < beadButtons.Length; i++)
			{
				beadButtons[i] = new BeadButton(i);
				beadButtons[i].Width.Set(32f, 0f);
				beadButtons[i].Height.Set(32f, 0f);
				beadButtons[i].Left.Set(0f, 0f);
				beadButtons[i].Top.Set(0f, 0f);
				Append(beadButtons[i]);
			}
		}

		public static void Show(RosaryBracelet bracelet) {
			controllingBracelet = bracelet;
			Visible = true;
		}

		public static void Hide() {
			controllingBracelet = null;
			Visible = false;
		}

		public override void Update(GameTime gameTime)
		{
			if(!Visible) {
				return;
			}

			float angle = Helper.HALF_PI + Helper.PI;
			Vector2 origin = Main.player[Main.myPlayer].Center.ToScreenPosition();

			for(int i = 0; i < beadButtons.Length; i++) {
				Vector2 toPos = Helper.FromPolar(angle + MathHelper.ToRadians(360 / beadButtons.Length) * i, 120, origin);
				beadButtons[i].Centre = toPos;
				// beadButtons[i].Left.Set(toPos.X - beadButtons[i].Width.Pixels / 2, 0f);
				// beadButtons[i].Top.Set(toPos.Y - beadButtons[i].Height.Pixels / 2, 0f);
			}

			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if(!Visible) {
				return;
			}

			for(int i = 0; i < beadButtons.Length; i++) {
				beadButtons[i].Draw(spriteBatch);
			}
		}
	}
}