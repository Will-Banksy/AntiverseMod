using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AntiverseMod.NPCs.HellOnEarth
{
	public class SwordDemon : ModNPC
	{
		private byte frame = 0;
		private byte frameCounter = 0;
		private const byte frameCounterMax = 20;

		private Rectangle hitboxFacingLeft = new Rectangle(52, 22, 42, 98);
		private Rectangle hitboxFacingRight = new Rectangle(20, 22, 42, 98);

		private const float maxVelX = 20;
		private const float accelX = 0.1f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword Demon");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 114;
			npc.height = 120;
			npc.aiStyle = -1;
			//npc.aiStyle = 3; // Fighter AI
			//aiType = NPCID.Fritz;
			npc.damage = 7;
			npc.defense = 2;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.sellPrice(silver: 20);
			npc.buffImmune[BuffID.OnFire] = true;
		}

		public override bool PreAI()
		{
			return true;
		}

		public override void AI()
		{
			// Urgh can't find a suitable AI type
			// Fine, I'll do it myself

			// Aquire target (stored in npc.target)
			npc.TargetClosestUpgraded(false, null);
			Player target = Main.player[npc.target];

			npc.velocity.Y += 0.1f;

			float toTargetX = Helper.DirTo(npc.Center, Main.player[npc.target].Center).X * accelX;
			npc.velocity.X += toTargetX;

			if(npc.velocity.X > maxVelX)
			{
				npc.velocity.X = maxVelX;
			}
			else if(npc.velocity.X < -maxVelX)
			{
				npc.velocity.X = -maxVelX;
			}

			Main.NewText(npc.velocity.X);
		}

		public override void FindFrame(int frameHeight)
		{
			byte frameCounterIncrement = (byte)Math.Ceiling(npc.velocity.X / 2);
			frameCounter += frameCounterIncrement;
			if (frameCounter > frameCounterMax)
			{
				frame++;
				frameCounter = 0;
			}
			if (frame >= 4)
			{
				frame = 0;
			}
			npc.frame.Y = frameHeight * frame;
		}
	}
}