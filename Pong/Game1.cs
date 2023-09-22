using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MochaMothMedia.Pong.Components;
using MochaMothMedia.Pong.Systems;
using MonoGame.Extended.Entities;
using System;

namespace MochaMothMedia.Pong
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private Model _boxModel;
		private Effect _boxEffect;
		private World _ecsWorld;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += HandleResize;
		}

		protected override void Initialize()
		{
			_ecsWorld = new WorldBuilder()
				.AddSystem(new BoxSystem())
				.AddSystem(new CameraSystem(_graphics))
				.AddSystem(new RenderSystem(GraphicsDevice))
				.AddSystem(new UIRenderSystem(GraphicsDevice))
				.Build();

			Components.Add(_ecsWorld);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_boxModel = Content.Load<Model>("box");
			_boxEffect = Content.Load<Effect>("Shaders/TestShader");

			Entity box = _ecsWorld.CreateEntity();
			box.Attach(new Transform(new Vector3(0, 0, 0)));
			box.Attach(new Box());
			box.Attach(new Mesh(_boxModel, _boxEffect));
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}

		void HandleResize(object sender, EventArgs eventArgs)
		{
			_graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
			_graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
			_graphics.ApplyChanges();
		}
	}
}