using MochaMothMedia.Pong.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Vector3 = System.Numerics.Vector3;
using Quaternion = System.Numerics.Quaternion;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using Matrix = System.Numerics.Matrix4x4;
using GameTime = Microsoft.Xna.Framework.GameTime;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;
using MochaMoth.Pong.Numerics;

namespace MochaMothMedia.Pong.ThirdPersonController
{
	internal class CameraController : EntityProcessingSystem
	{
		private ComponentMapper<Transform> _transformMapper;
		private ComponentMapper<Camera> _cameraMapper;
		private readonly GraphicsDeviceManager _graphics;

		public CameraController(GraphicsDeviceManager graphics) : base(Aspect.All(typeof(Transform), typeof(Camera)))
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
			CameraDolly dolly = camera.Dolly.Get<CameraDolly>();
			Transform targetTransform = dolly.Target.Get<Transform>();

			// Apply Rotation
			Quaternion pitchRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(dolly.Pitch));
			Quaternion yawRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(dolly.Yaw));
			transform.Rotation = Quaternion.Normalize(yawRotation * pitchRotation);

			// Apply Translation
			Vector3 localRight = Vector3.Transform(Vector3.UnitX, transform.Rotation);
			Vector3 newPosition = targetTransform.Position;
			newPosition += Vector3.UnitY * dolly.Height;
			newPosition += Vector3.Transform(Vector3.UnitZ, transform.Rotation) * dolly.Depth;
			newPosition += localRight * dolly.Offset;
			transform.Position = newPosition;

			// Apply WorldViewProjection Matrices
			camera.AspectRatio = (float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight;
			Matrix worldMatrix = Matrix.CreateFromQuaternion(transform.Rotation) * Matrix.CreateTranslation(transform.Position);
			camera.ViewMatrix = worldMatrix.Invert();
			camera.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(camera.FovAngle, camera.AspectRatio, camera.NearClipPlane, camera.FarClipPlane);
		}
	}
}
