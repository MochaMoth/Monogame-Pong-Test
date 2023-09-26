using Microsoft.Xna.Framework.Input;
using MochaMothMedia.Pong.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Vector3 = System.Numerics.Vector3;
using Vector2 = System.Numerics.Vector2;
using Quaternion = System.Numerics.Quaternion;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using Matrix = System.Numerics.Matrix4x4;
using GameTime = Microsoft.Xna.Framework.GameTime;
using MochaMothMedia.Pong.Input;
using MochaMoth.Pong.Numerics;
using System.Diagnostics;

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

			// Apply Rotation
			Vector2 lookVector = InputSystem.CurrentState.LookVector;
			Quaternion pitchRotation = Quaternion.CreateFromAxisAngle(Vector3.Transform(Vector3.UnitX, transform.Rotation), MathHelper.ToRadians(lookVector.Y * camera.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds));
			Quaternion yawRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(lookVector.X * camera.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds));
			transform.Rotation = Quaternion.Normalize(yawRotation * pitchRotation * transform.Rotation);
			Debug.WriteLine($"Look Vector: {lookVector.X}, {lookVector.Y}");

			// Apply Translation
			Vector3 movementVector = InputSystem.CurrentState.NormalizedTrueVector;
			Vector3 worldSpaceMovement = Vector3.Transform(movementVector, Matrix.CreateFromQuaternion(transform.Rotation));
			transform.Position += worldSpaceMovement * camera.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;

			// Apply WorldViewProjection Matrices
			camera.AspectRatio = (float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight;
			Matrix worldMatrix = Matrix.CreateFromQuaternion(transform.Rotation) * Matrix.CreateTranslation(transform.Position);
			camera.ViewMatrix = worldMatrix.Invert();
			camera.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(camera.FovAngle, camera.AspectRatio, camera.NearClipPlane, camera.FarClipPlane);
		}
	}
}
