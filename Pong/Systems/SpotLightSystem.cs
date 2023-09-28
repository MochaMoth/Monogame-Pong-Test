using MochaMoth.Pong.Numerics;
using MochaMothMedia.Pong.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Matrix = System.Numerics.Matrix4x4;
using Vector3 = System.Numerics.Vector3;
using MathHelper = Microsoft.Xna.Framework.MathHelper;

namespace MochaMothMedia.Pong.Systems
{
	internal class SpotLightSystem : EntityDrawSystem
	{
		private ComponentMapper<Transform> _transformMapper;
		private ComponentMapper<SpotLight> _spotLightMapper;

		public SpotLightSystem() : base(Aspect.All(typeof(Transform), typeof(SpotLight))) { }

		public override void Initialize(IComponentMapperService mapperService)
		{
			_transformMapper = mapperService.GetMapper<Transform>();
			_spotLightMapper = mapperService.GetMapper<SpotLight>();
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (int entityId in ActiveEntities)
			{
				Transform transform = _transformMapper.Get(entityId);
				SpotLight spotLight = _spotLightMapper.Get(entityId);

				//Matrix worldMatrix = Matrix.CreateFromQuaternion(transform.Rotation) * Matrix.CreateTranslation(transform.Position);
				//Matrix viewMatrix = worldMatrix.Invert();
				Matrix viewMatrix = Matrix.CreateLookAt(transform.Position, Vector3.Zero, Vector3.UnitY);
				Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, spotLight.NearClipPlane, spotLight.FarClipPlane);

				spotLight.ViewMatrix = viewMatrix;
				spotLight.ProjectionMatrix = projectionMatrix;
			}
		}
	}
}
