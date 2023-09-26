using Microsoft.Xna.Framework;

namespace MochaMothMedia.Pong.Components
{
	internal class Camera
	{
		public Matrix ViewMatrix { get; set; }
		public Matrix ProjectionMatrix { get; set; }
		public float FovAngle => MathHelper.ToRadians(60f);
		public float AspectRatio { get; set; }
		public float NearClipPlane => 0.001f;
		public float FarClipPlane => 1000f;
		public float MoveSpeed => 0.005f;
		public float RotationSpeed => 0.005f;
	}
}
