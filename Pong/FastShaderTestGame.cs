using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MochaMothMedia.Pong
{
    public class FastShaderTestGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _image;
        private Effect _firstShader;
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _image = Content.Load<Texture2D>("image");
            _box = Content.Load<Model>("box");
            _firstShader = Content.Load<Effect>("Shaders/TestShader");
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

            Matrix view = Matrix.Identity;

            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);

            // This fails to render anything.
            foreach (ModelMesh mesh in _box.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = _firstShader;
                    _firstShader.Parameters["World"].SetValue(Matrix.Identity);
                    _firstShader.Parameters["View"].SetValue(view);
                    _firstShader.Parameters["Projection"].SetValue(projection);
                }
                mesh.Draw();
            }

            // This works correctly
            //_firstShader.Parameters["view_projection"].SetValue(view * projection);

            //_spriteBatch.Begin(effect: _firstShader);
            //_spriteBatch.Draw(_image, new Vector2(0, 0), Color.White);
            //_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}