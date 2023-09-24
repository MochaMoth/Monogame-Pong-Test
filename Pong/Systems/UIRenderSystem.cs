using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MochaMothMedia.Pong.Systems
{
	internal class UIRenderSystem : EntityDrawSystem
	{
		private readonly GraphicsDevice _graphicsDevice;
		private SpriteBatch _spriteBatch;
		private Texture2D _sampleTexture;

		public UIRenderSystem(GraphicsDevice graphicsDevice) : base(Aspect.All())
		{
			_graphicsDevice = graphicsDevice;
		}

		public override void Initialize(IComponentMapperService mapperService)
		{
			_spriteBatch = new SpriteBatch(_graphicsDevice);

			_sampleTexture = new Texture2D(_graphicsDevice, 1, 1);
			_sampleTexture.SetData(new Color[] { Color.DarkSlateGray });
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch.Begin();

			_spriteBatch.Draw(_sampleTexture, new Rectangle(5, 5, 25, 25), Color.White);

			_spriteBatch.End();
		}
	}
}
