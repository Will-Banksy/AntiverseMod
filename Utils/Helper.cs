using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace AntiverseMod.Utils
{
	public static class Helper
	{
		public const float HALF_PI = 1.57080f; // Equivalent to 90 degrees
		public const float PI = 3.14159f; // Equivalent to 180 degrees
		public const float TWO_PI = 6.28318f; // Equivalent to 360 degrees

		public static readonly Vector2[] vecArrCross = { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) };
		public static readonly Vector2[] vecArrPlus = { new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1) };

        public static class Map
		{
			public delegate float MapFunction(float value, float start1, float stop1, float start2, float stop2);

			/// <summary>
			/// Maps value from between start1 and stop1 to between start2 and stop2
			/// </summary>
			public static float Linear(float value, float start1, float stop1, float start2, float stop2)
			{
				return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
			}

			/// <summary>
			/// Maps value from between start1 and stop1 to between start2 and stop2
			/// </summary>
			public static float ExpoIn(float value, float start1, float stop1, float start2, float stop2)
			{
				float x = Linear(value, start1, stop1, 0, 1);
				float normalised = x == 0 ? 0 : (float)Math.Pow(2, 10 * x - 10);
				return Linear(normalised, 0, 1, start2, stop2);
			}

			/// <summary>
			/// Maps value from between start1 and stop1 to between start2 and stop2
			/// </summary>
			public static float ExpoOut(float value, float start1, float stop1, float start2, float stop2)
			{
				float x = Linear(value, start1, stop1, 0, 1);
				float normalised = x == 1 ? 1 : 1 - (float)Math.Pow(2, -10 * x);
				return Linear(normalised, 0, 1, start2, stop2);
			}

			/// <summary>
			/// Maps value from between start1 and stop1 to between start2 and stop2
			/// </summary>
			public static float QuadIn(float value, float start1, float stop1, float start2, float stop2)
			{
				float x = Linear(value, start1, stop1, 0, 1);
				float normalised = x * x;
				return Linear(normalised, 0, 1, start2, stop2);
			}
		}

		/// <summary>
		/// Maps vector from between start1 and stop1 to between start2 and stop2
		/// </summary>
		public static void VectorMap(this Vector2 vector, Vector2 start1, Vector2 stop1, Vector2 start2, Vector2 stop2, Map.MapFunction function = null)
		{
			function = function ?? Map.Linear;
			vector.X = function(vector.X, start1.X, stop1.X, start2.X, stop2.X);
			vector.Y = function(vector.Y, start1.Y, stop1.Y, start2.Y, stop2.Y);
		}

		/// <summary>
		/// Maps amt from between 0 and 1 to between value and target
		/// </summary>
		public static float Interpolate(float value, float target, float amt, Map.MapFunction function = null)
		{
			function = function ?? Map.Linear;
			return function(amt, 0, 1, value, target);
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

		public static float AngleToTarget(this Vector2 origin, Vector2 target)
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
			vector.X *= len;
			vector.Y *= len;
		}

		public static Vector2 DirTo(Vector2 start, Vector2 end)
		{
			int dirX = start.X < end.X ? 1 : (start.X == end.X ? 0 : -1);
			int dirY = start.Y < end.Y ? 1 : (start.Y == end.Y ? 0 : -1);
			return new Vector2(dirX, dirY);
		}

		public static Vector2 DirOf(Vector2 vec)
		{
			return DirTo(Vector2.Zero, vec);
		}

		// Returns true if the two values are roughly equal - If the difference between them is less than fuzziness (by default 0.05)
		public static bool FuzzyEquals(this float n1, float n2, float fuzziness = 0.05f)
		{
			return Math.Abs(n1 - n2) < fuzziness;
		}

		// Checks whether the currentPos is 'past' the point, from initialPos. TODO Make this work better
		public static bool HasGonePast(Vector2 initialPos, Vector2 point, Vector2 currentPos)
		{
			Vector2 dirToInit = Helper.DirTo(initialPos, point);
			Vector2 dirToCurr = Helper.DirTo(currentPos, point);
			return ((dirToInit.X != dirToCurr.X) || initialPos.Y.FuzzyEquals(point.Y, 0.1f)) && ((dirToInit.Y != dirToCurr.Y) || initialPos.X.FuzzyEquals(point.X, 0.1f));
		}

		//
		public static bool GoingAwayFrom(Vector2 point, Vector2 position, Vector2 velocity)
		{
			return DirTo(point, position) == DirOf(velocity);
		}

		public static bool GoingTowards(Vector2 point, Vector2 position, Vector2 velocity)
		{
			return DirTo(position, point) == DirOf(velocity);
		}

		public static void StepAnim(Projectile proj, int frameDuration)
        {
			// Loop through the 5 animation frames, spending 15 ticks on each.
			if (++proj.frameCounter >= frameDuration)
			{
				proj.frameCounter = 0;
				if (++proj.frame >= Main.projFrames[proj.type])
				{
					proj.frame = 0;
				}
			}
		}

		// Spread in radians
		public static Vector2 RandSpread(Vector2 velocity, float spread, float? speedVariation = null)
        {
			float ang = velocity.ToRotation() + Main.rand.NextFloat(-spread, spread);
			float speed = speedVariation == null ? velocity.Length() : velocity.Length() + Main.rand.NextFloat(-speedVariation ?? 0, speedVariation ?? 0);
			return FromPolar(ang, speed);
        }

		public static Vector2 FromCentre(this Vector2 centre, float width, float height) {
			return centre - new Vector2(width / 2, height / 2);
		}
	}
}