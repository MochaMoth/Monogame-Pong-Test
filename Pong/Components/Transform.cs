using Microsoft.Xna.Framework;

namespace MochaMothMedia.Pong.Components
{
	internal class Transform
	{
		public Vector3 Position { get; set; } = Vector3.Zero;
		public Quaternion Rotation { get; set; } = Quaternion.Identity;
		public Vector3 Scale { get; set; } = Vector3.One;

		public Transform() { }

		public Transform(Vector3 position)
		{
			Position = position;
		}

		public Transform(Vector3 position, Quaternion rotation)
		{
			Position = position;
			Rotation = rotation;
		}

		public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			Position = position;
			Rotation = rotation;
			Scale = scale;
		}
	}
}
