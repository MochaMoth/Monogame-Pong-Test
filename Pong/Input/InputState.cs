using Microsoft.Xna.Framework.Input;
using MochaMoth.Pong.Numerics;
using System.Numerics;

namespace MochaMothMedia.Pong.Input
{
	internal class InputState
	{
		/// <summary>
		/// Forwards - Backwards plane movement
		/// </summary>
		public float Lateral { get; set; }

		/// <summary>
		/// Left - Right plane movement
		/// </summary>
		public float Horizontal { get; set; }

		/// <summary>
		/// Up - Down plane movement
		/// </summary>
		public float Vertical { get; set; }

		/// <summary>
		/// Mouse Left - Right movement
		/// </summary>
		public float LookHorizontal { get; set; }

		/// <summary>
		/// Mouse Up - Down movement
		/// </summary>
		public float LookVertical { get; set; }

		/// <summary>
		/// Left - Right and Forward - Backward vector
		/// </summary>
		public Vector2 XZPlane => new Vector2(Horizontal, Lateral);

		/// <summary>
		/// Left - Right and Up - Down vector
		/// </summary>
		public Vector2 XYPlane => new Vector2(Horizontal, Vertical);

		/// <summary>
		/// Up - Down and Forward - Backward vector
		/// </summary>
		public Vector2 YZPlane => new Vector2(Vertical, Lateral);

		/// <summary>
		/// X, Y, and Z vector
		/// </summary>
		public Vector3 TrueVector => new Vector3(Horizontal, Vertical, Lateral);

		/// <summary>
		/// Normalized X, Y, and Z vector
		/// </summary>
		public Vector3 NormalizedTrueVector => new Vector3(Horizontal, Vertical, Lateral).SafeNormalize();

		/// <summary>
		/// Mouse look vector
		/// </summary>
		public Vector2 LookVector => new Vector2(LookHorizontal, LookVertical);
	}
}
