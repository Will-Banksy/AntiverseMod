using Terraria;
using Microsoft.Xna.Framework;
using System;

public static class Helper
{
	public const float HALF_PI = 1.57080f; // Equivalent to 90 degrees
	public const float PI = 3.14159f; // Equivalent to 180 degrees
	public const float TWO_PI = 6.28318f; // Equivalent to 360 degrees

	public static class Map
	{
		public delegate float MapFunction(float value, float start1, float stop1, float start2, float stop2);

		public static float Linear(float value, float start1, float stop1, float start2, float stop2)
		{
			return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
		}

		public static float ExpoIn(float value, float start1, float stop1, float start2, float stop2)
		{
			float x = Linear(value, start1, stop1, 0, 1);
			float normalised = x == 0 ? 0 : (float)Math.Pow(2, 10 * x - 10);
			return Linear(normalised, 0, 1, start2, stop2);
		}

		public static float ExpoOut(float value, float start1, float stop1, float start2, float stop2)
		{
			float x = Linear(value, start1, stop1, 0, 1);
			float normalised = x == 1 ? 1 : 1 - (float)Math.Pow(2, -10 * x);
			return Linear(normalised, 0, 1, start2, stop2);
		}

		public static float QuadIn(float value, float start1, float stop1, float start2, float stop2)
		{
			float x = Linear(value, start1, stop1, 0, 1);
			float normalised = x * x;
			return Linear(normalised, 0, 1, start2, stop2);
		}
	}

	public static void ApplyMappingFunction(this Vector2 vector, Map.MapFunction function, float start1, float stop1, float start2, float stop2)
	{
		vector.X = function(vector.X, start1, stop1, start2, stop2);
		vector.Y = function(vector.Y, start1, stop1, start2, stop2);
	}

	public static void FaceForward(this Projectile projectile)
	{
		projectile.rotation = projectile.velocity.ToRotation() + HALF_PI;
	}

	public static Vector2 VelocityToPoint(Vector2 a, Vector2 b, float speed = 1)
	{
		Vector2 ab = b - a;
		return ab * (speed / ab.Length());
	}

	public static Vector2 FromPolar(float ang, float r, Vector2 offset = default)
	{
		return new Vector2((float)Math.Cos(ang) * r + offset.X, (float)Math.Sin(ang) * r + offset.Y);
	}

	public static float AngleTo(this Vector2 origin, Vector2 target)
	{
		Vector2 ab = target - origin;
		return ab.ToRotation();
	}

	public static void AngleNorm(ref float angle)
	{
		while (angle < 0)
			angle += TWO_PI;
		while (angle >= TWO_PI)
			angle -= TWO_PI;
	}

	public static bool AngleBetween(this float angle, float a, float b)
	{
		AngleNorm(ref angle);
		AngleNorm(ref a);
		AngleNorm(ref b);

		if (a < b)
			return a <= angle && angle <= b;
		return a <= angle || angle <= b;
	}

	public static void SetLength(this Vector2 vector, float len)
	{
		vector.Normalize();
		vector *= len;
	}

	public static void SetCentre(this Entity entity, Vector2 pos)
	{
		entity.position = new Vector2(pos.X - entity.width / 2, pos.Y - entity.height / 2);
	}

	public static Vector2 DirTo(Vector2 start, Vector2 end)
	{
		int dirX = start.X < end.X ? 1 : -1;
		int dirY = start.Y < end.Y ? 1 : -1;
		return new Vector2(dirX, dirY);
	}
}