using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MochaMothMedia.Pong
{
    public class FastShaderTestGame : Game
    {
        private GraphicsDeviceManager _graphics;

        private Effect _effect;
        private Model _box;

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
            _box = Content.Load<Model>("box");
            _effect = Content.Load<Effect>("Shaders/TestShader");

            foreach (ModelMesh modelMesh in _box.Meshes)
            {
                foreach (ModelMeshPart part in modelMesh.MeshParts)
                {
                    part.Effect = _effect;
                    part.Effect.CurrentTechnique = part.Effect.Techniques["Ambient"];
                    part.Effect.CurrentTechnique.Passes[0].Apply();
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix worldMatrix = Matrix.CreateFromQuaternion(Quaternion.Identity) * Matrix.CreateTranslation(new Vector3(0, 0, 10f));
            Matrix viewMatrix = Matrix.Invert(worldMatrix);
            Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f),
                (float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight,
                0.01f,
                1000f);

            foreach (ModelMesh modelMesh in _box.Meshes)
            {
				foreach (ModelMeshPart part in modelMesh.MeshParts)
				{
                    _effect.Parameters["World"].SetValue(Matrix.CreateFromQuaternion(Quaternion.Identity) * Matrix.CreateTranslation(Vector3.Zero) * modelMesh.ParentBone.Transform);
                    _effect.Parameters["View"].SetValue(viewMatrix);
                    _effect.Parameters["Projection"].SetValue(projectionMatrix);
                    part.Effect = _effect;
				}

                //foreach (BasicEffect effect in modelMesh.Effects)
                //{
                //    effect.World = Matrix.CreateFromQuaternion(Quaternion.Identity) * Matrix.CreateTranslation(Vector3.Zero) * modelMesh.ParentBone.Transform;
                //    effect.View = viewMatrix;
                //    effect.Projection = projectionMatrix;
                //}

                modelMesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
