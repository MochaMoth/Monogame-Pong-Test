using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities.Systems;

namespace MochaMothMedia.Pong.Input
{
	internal class InputSystem : UpdateSystem
	{
		public static InputState PreviousState { get; private set; }
		public static InputState CurrentState { get; private set; }

		private Vector2 _previousMousePosition = Vector2.Zero;

		public InputSystem() : base()
		{
			PreviousState = new InputState();
			CurrentState = new InputState();

			_previousMousePosition = Mouse.GetState().Position.ToVector2();
		}

		public override void Update(GameTime gameTime)
		{
			PreviousState = CurrentState;
			KeyboardState keyboard = Keyboard.GetState();
			MouseState mouse = Mouse.GetState();

			CurrentState.Lateral = (keyboard.IsKeyDown(Keys.W) ? -1 : 0) + (keyboard.IsKeyDown(Keys.S) ? 1 : 0);
			CurrentState.Horizontal = (keyboard.IsKeyDown(Keys.A) ? -1 : 0) + (keyboard.IsKeyDown(Keys.D) ? 1 : 0);
			CurrentState.Vertical = (keyboard.IsKeyDown(Keys.Q) ? -1 : 0) + (keyboard.IsKeyDown(Keys.E) ? 1 : 0);

			CurrentState.LookHorizontal = _previousMousePosition.X - mouse.X;
			CurrentState.LookVertical = _previousMousePosition.Y - mouse.Y;
		}
	}
}
