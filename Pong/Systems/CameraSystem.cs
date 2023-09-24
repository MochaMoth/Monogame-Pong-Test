using Microsoft.Xna.Framework.Input;
using MochaMothMedia.Pong.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Vector3 = System.Numerics.Vector3;
using Quaternion = System.Numerics.Quaternion;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using Matrix = Microsoft.Xna.Framework.Matrix;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace MochaMothMedia.Pong.Systems
{
	internal class CameraSystem : EntityProcessingSystem
	{
		private ComponentMapper<Transform> _transformMapper;
		private ComponentMapper<Camera> _cameraMapper;
		private readonly Microsoft.Xna.Framework.GraphicsDeviceManager _graphics;

		public CameraSystem(Microsoft.Xna.Framework.GraphicsDeviceManager graphics) : base(Aspect.All(typeof(Transform), typeof(Camera)))
		{
			_graphics = graphics;
		}

		public override void Initialize(IComponentMapperService mapperService)
		{
			_transformMapper = mapperService.GetMapper<Transform>();
			_cameraMapper = mapperService.GetMapper<Camera>();
		}

		public override void Process(GameTime gameTime, int entityId)
		{
			Transform transform = _transformMapper.Get(entityId);
			Camera camera = _cameraMapper.Get(entityId);

			camera.AspectRatio = (float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight;

			KeyboardState keyboard = Keyboard.GetState();

			float translationAmount = camera.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
			float rotationAmount = camera.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds;

			if (keyboard.IsKeyDown(Keys.A))
				transform.Position = transform.Position with { X = transform.Position.X - translationAmount };
			if (keyboard.IsKeyDown(Keys.D))
				transform.Position = transform.Position with { X = transform.Position.X + translationAmount };
			if (keyboard.IsKeyDown(Keys.W))
				transform.Position = transform.Position with { Y = transform.Position.Y + translationAmount };
			if (keyboard.IsKeyDown(Keys.S))
				transform.Position = transform.Position with { Y = transform.Position.Y - translationAmount };

			if (keyboard.IsKeyDown(Keys.Q))
				transform.Position = transform.Position with { Z = transform.Position.Z - translationAmount };
			if (keyboard.IsKeyDown(Keys.E))
				transform.Position = transform.Position with { Z = transform.Position.Z + translationAmount };

			if (keyboard.IsKeyDown(Keys.Left))
				transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(rotationAmount));
			if (keyboard.IsKeyDown(Keys.Right))
				transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(-rotationAmount));
			if (keyboard.IsKeyDown(Keys.Up))
				transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(rotationAmount));
			if (keyboard.IsKeyDown(Keys.Down))
				transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-rotationAmount));

			Matrix worldMatrix = Matrix.CreateFromQuaternion(transform.Rotation) * Matrix.CreateTranslation(transform.Position);
			camera.ViewMatrix = Matrix.Invert(worldMatrix);
			camera.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(camera.FovAngle, camera.AspectRatio, camera.NearClipPlane, camera.FarClipPlane);
		}
	}
}
