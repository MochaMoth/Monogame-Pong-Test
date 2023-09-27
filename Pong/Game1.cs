using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MochaMothMedia.Pong.Components;
using MochaMothMedia.Pong.Systems;
using MonoGame.Extended.Entities;
using System;
using Vector3 = System.Numerics.Vector3;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Game = Microsoft.Xna.Framework.Game;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;
using PlayerIndex = Microsoft.Xna.Framework.PlayerIndex;
using MochaMothMedia.Pong.Input;
using MochaMothMedia.Pong.ThirdPersonController;

namespace MochaMothMedia.Pong
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private Model _boxModel;
		private Model _boxModel2;
		private Model _playerModel;
		private Effect _boxEffect;
		private World _ecsWorld;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this)
			{
				GraphicsProfile = GraphicsProfile.HiDef,
				PreferredBackBufferWidth = 1920,
				PreferredBackBufferHeight = 1080
			};
			Content.RootDirectory = "Content";
			IsMouseVisible = false;
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += HandleResize;
		}

		protected override void Initialize()
		{
			Mouse.SetPosition(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

			_ecsWorld = new WorldBuilder()
				.AddSystem(new InputSystem())
				.AddSystem(new BoxSystem())
				.AddSystem(new DollyController())
				.AddSystem(new PlayerController())
				.AddSystem(new CameraController(_graphics))
				.AddSystem(new SkyboxRenderSystem(GraphicsDevice))
				.AddSystem(new RenderSystem(GraphicsDevice))
				.AddSystem(new UIRenderSystem(GraphicsDevice))
				.Build();

			Components.Add(_ecsWorld);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_boxModel = Content.Load<Model>("box");
			_boxModel2 = Content.Load<Model>("box2");
			_playerModel = Content.Load<Model>("Bean");
			_boxEffect = Content.Load<Effect>("Shaders/TestShader");

			Entity player = _ecsWorld.CreateEntity();
			Entity cameraDolly = _ecsWorld.CreateEntity();
			Statics.Camera.Main = _ecsWorld.CreateEntity();
			Entity box = _ecsWorld.CreateEntity();
			Entity box2 = _ecsWorld.CreateEntity();

			player.Attach(new Transform(Vector3.Zero));
			player.Attach(new Mesh(_playerModel, _boxEffect));
			player.Attach(new Player(cameraDolly));

			cameraDolly.Attach(new CameraDolly(player));

			Statics.Camera.Main.Attach(new Transform(new Vector3(0, 0, 10f)));
			Statics.Camera.Main.Attach(new Camera(cameraDolly));

			box.Attach(new Transform(new Vector3(0, 0, 0)));
			box.Attach(new Box());
			box.Attach(new Mesh(_boxModel, _boxEffect));

			box2.Attach(new Transform(new Vector3(5f, 0, 0)));
			box2.Attach(new Box());
			box2.Attach(new Mesh(_boxModel2, _boxEffect));
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			base.Update(gameTime);

			Mouse.SetPosition(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
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
