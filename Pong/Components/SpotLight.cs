using Microsoft.Xna.Framework.Graphics;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace MochaMothMedia.Pong.Components
{
	internal class SpotLight
	{
		public SpotLight(GraphicsDevice graphicsDevice)
		{
			PresentationParameters presentationParameters = graphicsDevice.PresentationParameters;
			RenderTarget = new RenderTarget2D(
				graphicsDevice,
				presentationParameters.BackBufferWidth,
				presentationParameters.BackBufferHeight,
				true,
				graphicsDevice.DisplayMode.Format,
				DepthFormat.Depth24);
		}

		public Matrix ViewMatrix { get; set; }
		public Matrix ProjectionMatrix { get; set; }
		public RenderTarget2D RenderTarget { get; set; }
		public Texture2D ShadowMap { get; set; }
		public float Power { get; set; } = 2f;
		public float NearClipPlane { get; set; } = 1f;
		public float FarClipPlane { get; set; } = 100f;
	}
}
