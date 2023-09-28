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
		private PBRTextures _pbrWoodTexture;
		private PBRTextures _pbrMetalTexture;
		private PBRTextures _pbrBarkTexture;
		private Effect _boxEffect;
		private World _ecsWorld;
		private bool _skipSetMousePos = false;

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
				.AddSystem(new SpotLightSystem())
				.AddSystem(new ShadowRenderSystem(GraphicsDevice))
				.AddSystem(new SkyboxRenderSystem(GraphicsDevice))
				.AddSystem(new RenderSystem(GraphicsDevice))
				.AddSystem(new UIRenderSystem(GraphicsDevice))
				.Build();

			Components.Add(_ecsWorld);

			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
			GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_boxModel = Content.Load<Model>("box");
			_boxModel2 = Content.Load<Model>("box2");
			_playerModel = Content.Load<Model>("Bean");
			_boxEffect = Content.Load<Effect>("Shaders/TestShader");
			_pbrWoodTexture = new PBRTextures()
			{
				Base = Content.Load<Texture2D>("Wood 03/Wood03_1K_BaseColor"),
				Normal = Content.Load<Texture2D>("Wood 03/Wood03_1K_Normal"),
				Height = Content.Load<Texture2D>("Wood 03/Wood03_1K_Height"),
				Roughness = Content.Load<Texture2D>("Wood 03/Wood03_1K_Roughness")
			};
			_pbrMetalTexture = new PBRTextures()
			{
				Base = Content.Load<Texture2D>("iron/base"),
				Normal = Content.Load<Texture2D>("iron/normal"),
				Height = Content.Load<Texture2D>("iron/height"),
				Roughness = Content.Load<Texture2D>("iron/roughness")
			};
			_pbrBarkTexture = new PBRTextures()
			{
				Base = Content.Load<Texture2D>("bark/base"),
				Normal = Content.Load<Texture2D>("bark/normal"),
				Height = Content.Load<Texture2D>("bark/height"),
				Roughness = Content.Load<Texture2D>("bark/roughness")
			};

			Entity player = _ecsWorld.CreateEntity();
			Entity cameraDolly = _ecsWorld.CreateEntity();
			Statics.Camera.Main = _ecsWorld.CreateEntity();
			Entity box = _ecsWorld.CreateEntity();
			Entity box2 = _ecsWorld.CreateEntity();
			Entity spotLight = _ecsWorld.CreateEntity();

			spotLight.Attach(new Transform(new Vector3(10f, 3f, 3f)));
			spotLight.Attach(new SpotLight(GraphicsDevice));
			Statics.Lights.SpotLights.Add(spotLight);

			player.Attach(new Transform(Vector3.Zero));
			player.Attach(new Mesh(_playerModel, _boxEffect, _pbrBarkTexture));
			player.Attach(new Player(cameraDolly));

			cameraDolly.Attach(new CameraDolly(player));

			Statics.Camera.Main.Attach(new Transform(new Vector3(0, 0, 10f)));
			Statics.Camera.Main.Attach(new Camera(cameraDolly));

			box.Attach(new Transform(new Vector3(0, 0, 0)));
			box.Attach(new Box());
			box.Attach(new Mesh(_boxModel, _boxEffect, _pbrWoodTexture));

			box2.Attach(new Transform(new Vector3(5f, 0, 0)));
			box2.Attach(new Box());
			box2.Attach(new Mesh(_boxModel2, _boxEffect, _pbrMetalTexture));
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (Keyboard.GetState().IsKeyDown(Keys.P))
			{
				IsMouseVisible = true;
				_skipSetMousePos = true;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.O))
			{
				IsMouseVisible = false;
				_skipSetMousePos = false;
			}

			base.Update(gameTime);

			if (!_skipSetMousePos)
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
