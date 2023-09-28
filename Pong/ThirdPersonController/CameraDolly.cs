using MonoGame.Extended.Entities;

namespace MochaMothMedia.Pong.ThirdPersonController
{
	internal class CameraDolly
	{
		public Entity Target { get; set; }
		public float Height { get; set; } = 2f;
		public float Depth { get; set; } = 5f;
		public float Offset { get; set; } = 0f;
		public float Yaw { get; set; }
		public float Pitch { get; set; }
		public float UpperPitchLimit { get; set; } = 80f;
		public float LowerPitchLimit { get; set; } = -80f;
		public float YawSpeed { get; set; } = 0.01f;
		public float PitchSpeed { get; set; } = 0.01f;

		public CameraDolly(Entity target)
		{
			Target = target;
		}
	}
}
