using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AntiverseMod.Items.Miscellaneous
{
    public class NecromanticMirror : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necromantic Mirror");
			Tooltip.SetDefault("Teleports you to your last death position");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 46;
			item.useTime = 50;
			item.useAnimation = 50;
			item.useStyle = 4;
			item.value = Item.sellPrice(gold: 1, silver: 80);
			item.rare = 4;
			item.autoReuse = false;
			item.useTurn = false;
			item.UseSound = SoundID.Item6;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(50, 1); //1 magic mirror
			recipe.AddIngredient(520, 10); //10 souls of light
			recipe.AddIngredient(521, 10); //10 souls of night
			recipe.AddIngredient(154, 20); //20 bones
			recipe.AddTile(TileID.Anvils); // Mythril Anvil
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceMirror, 1); //1 magic mirror
			recipe.AddIngredient(520, 10); //10 souls of light
			recipe.AddIngredient(521, 10); //10 souls of night
			recipe.AddIngredient(154, 20); //20 bones
			recipe.AddTile(TileID.Anvils); // Mythril Anvil
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override bool CanUseItem(Player player)
		{
			if(player.lastDeathPostion.X == 0 && player.lastDeathPostion.Y == 0)
			{
				return false;
			}
			else
			{
				return base.CanUseItem(player);
			}
		}
		
		public override bool UseItem(Player player)
		{
            if (player.lastDeathPostion != null)
            {
                if (player.lastDeathPostion.X > 0 && player.lastDeathPostion.Y > 0)
                {
                    if (Main.rand.Next(2) == 0)
                        Dust.NewDust(player.position, player.width, player.height, 15, 0.0f, 0.0f, 150, Color.Purple, 1.1f);
                    if (player.itemAnimation == item.useAnimation / 2)
                    {
                        for (int index = 0; index < 70; ++index)
                            Dust.NewDust(player.position, player.width, player.height, 15, (float)(player.velocity.X * 0.5), (float)(player.velocity.Y * 0.5), 150, Color.Purple, 1.5f);
                        player.Teleport(player.lastDeathPostion, -69);
                        player.Center = player.lastDeathPostion;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(65, -1, -1, null, 0, (float)player.whoAmI, player.lastDeathPostion.X, player.lastDeathPostion.Y, 3);

                        for (int index = 0; index < 70; ++index)
                            Dust.NewDust(player.position, player.width, player.height, 15, 0.0f, 0.0f, 150, Color.Purple, 1.5f);
                        return true;
                    }
                }
            }
			return false;
		}
	}
}