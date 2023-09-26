using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities.Systems;

namespace MochaMothMedia.Pong.Systems
{
	internal class SkyboxRenderSystem : DrawSystem
	{
		private readonly GraphicsDevice _graphicsDevice;

		public SkyboxRenderSystem(GraphicsDevice graphicsDevice) : base()
		{
			_graphicsDevice = graphicsDevice;
		}

		public override void Draw(GameTime gameTime)
		{
			_graphicsDevice.Clear(Color.CornflowerBlue);
		}
	}
}
