using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace AntiverseMod.Items.Miscellaneous; 

public class NecromanticMirror : ModItem
{
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Necromantic Mirror");
		Tooltip.SetDefault("Teleports you to your last death position");
	}

	public override void SetDefaults()
	{
		Item.width = 26;
		Item.height = 46;
		Item.useTime = 50;
		Item.useAnimation = 50;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.value = Item.sellPrice(gold: 1, silver: 80);
		Item.rare = ItemRarityID.LightRed;
		Item.autoReuse = false;
		Item.useTurn = false;
		Item.UseSound = SoundID.Item6;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ItemID.MagicMirror, 1);
		recipe.AddIngredient(ItemID.SoulofLight, 10);
		recipe.AddIngredient(ItemID.SoulofNight, 10);
		recipe.AddIngredient(ItemID.Bone, 20);
		recipe.AddTile(TileID.MythrilAnvil); // Mythril Anvil
		recipe.Register();

		recipe = CreateRecipe();
		recipe.AddIngredient(ItemID.IceMirror, 1);
		recipe.AddIngredient(ItemID.SoulofLight, 10);
		recipe.AddIngredient(ItemID.SoulofNight, 10);
		recipe.AddIngredient(ItemID.Bone, 20);
		recipe.AddTile(TileID.MythrilAnvil); // Mythril Anvil
		recipe.Register();

		recipe = CreateRecipe();
		recipe.AddIngredient<BrokenNecromanticMirror>();
		recipe.AddIngredient(ItemID.SoulofLight, 10);
		recipe.AddIngredient(ItemID.SoulofNight, 10);
		recipe.AddTile(TileID.MythrilAnvil);
		recipe.Register();
	}

	public override bool CanUseItem(Player player)
	{
		if (player.lastDeathPostion.X == 0 && player.lastDeathPostion.Y == 0)
		{
			return false;
		}
		else
		{
			return base.CanUseItem(player);
		}
	}

	public override bool? UseItem(Player player)
	{
		if (Main.myPlayer == player.whoAmI)
		{
			if (player.lastDeathPostion.X > 0 && player.lastDeathPostion.Y > 0)
			{
				if (Main.rand.NextBool(2))
					Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0.0f, 0.0f, 150, Color.Purple, 1.1f);
				if (player.itemAnimation == Item.useAnimation / 2)
				{
					for (int index = 0; index < 70; ++index)
						Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, (float)(player.velocity.X * 0.5), (float)(player.velocity.Y * 0.5), 150, Color.Purple, 1.5f);
					player.Teleport(player.lastDeathPostion, -69);
					player.Center = player.lastDeathPostion;
					if (Main.netMode == NetmodeID.MultiplayerClient)
						NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, (float)player.whoAmI, player.lastDeathPostion.X, player.lastDeathPostion.Y, 3);

					for (int index = 0; index < 70; ++index)
						Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0.0f, 0.0f, 150, Color.Purple, 1.5f);
					return true;
				}
			}
		}
		return false;
	}
}