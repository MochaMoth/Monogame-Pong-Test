using MonoGame.Extended.Entities;

namespace MochaMothMedia.Pong.ThirdPersonController
{
	internal class Player
	{
		public Player(Entity cameraDolly)
		{
			CameraDolly = cameraDolly;
		}

		public Entity CameraDolly { get; set; }
		public float RunSpeed { get; set; } = 0.01f;
		public float Gravity { get; set; }
		public float TerminalVelocity { get; set; }
	}
}
