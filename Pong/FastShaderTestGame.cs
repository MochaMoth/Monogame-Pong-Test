using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MochaMothMedia.Pong
{
	internal class FastShaderTestGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private Model _boxModel;
		private Effect _boxEffect;

		public FastShaderTestGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			_boxModel = Content.Load<Model>("box");
			_boxEffect = Content.Load<Effect>("Shaders/TestShader");

			foreach (ModelMesh mesh in _boxModel.Meshes)
			{
				foreach (ModelMeshPart part in mesh.MeshParts)
				{
					part.Effect = _boxEffect;
					part.Effect.CurrentTechnique = part.Effect.Techniques["Ambient"];
					part.Effect.CurrentTechnique.Passes[0].Apply();
				}
			}

			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			foreach (ModelMesh modelMesh in _boxModel.Meshes)
			{
				foreach (ModelMeshPart part in modelMesh.MeshParts)
				{
					part.Effect.Parameters["World"].SetValue(Matrix.Identity);
					part.Effect.Parameters["View"].SetValue(Matrix.CreateLookAt(Vector3.Backward * 10f, Vector3.Zero, Vector3.Up));
					part.Effect.Parameters["Projection"].SetValue(Matrix.CreatePerspectiveFieldOfView(
						MathHelper.ToRadians(60f),
						(float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight,
						0.001f,
						1000f));
				}

				//foreach (BasicEffect effect in modelMesh.Effects)
				//{
				//	effect.World = Matrix.Identity;
				//	effect.View = Matrix.CreateLookAt(Vector3.Backward * 10f, Vector3.Zero, Vector3.Up);
				//	effect.Projection = Matrix.CreatePerspectiveFieldOfView(
				//		MathHelper.ToRadians(60f),
				//		(float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight,
				//		0.001f,
				//		1000f);
				//}

				modelMesh.Draw();
			}

			base.Draw(gameTime);
		}
	}
}
