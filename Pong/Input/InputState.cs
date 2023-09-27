using System.Numerics;

namespace MochaMothMedia.Pong.Input
{
	internal class InputState
	{
		public float Lateral { get; set; }
		public float Horizontal { get; set; }
		public float Vertical { get; set; }
		public float LookHorizontal { get; set; }
		public float LookVertical { get; set; }

		public Vector2 XZPlane => new Vector2(Horizontal, Lateral);
		public Vector2 XYPlane => new Vector2(Horizontal, Vertical);
		public Vector2 YZPlane => new Vector2(Vertical, Lateral);
		public Vector3 TrueVector => new Vector3(Horizontal, Vertical, Lateral);

		public Vector2 LookVector => new Vector2(LookHorizontal, LookVertical);
	}
}
