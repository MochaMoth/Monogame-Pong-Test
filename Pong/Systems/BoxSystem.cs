using MochaMothMedia.Pong.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Vector3 = System.Numerics.Vector3;
using Quaternion = System.Numerics.Quaternion;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace MochaMothMedia.Pong.Systems
{
	internal class BoxSystem : EntityProcessingSystem
	{
		private ComponentMapper<Transform> _transformMapper;
		private ComponentMapper<Box> _boxMapper;

		public BoxSystem() : base(Aspect.All(typeof(Transform), typeof(Box))) { }

		public override void Initialize(IComponentMapperService mapperService)
		{
			_transformMapper = mapperService.GetMapper<Transform>();
			_boxMapper = mapperService.GetMapper<Box>();
		}

		public override void Process(GameTime gameTime, int entityId)
		{
			Transform transform = _transformMapper.Get(entityId);
			Box box = _boxMapper.Get(entityId);

			transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(gameTime.ElapsedGameTime.Milliseconds * box.RotationSpeed));
		}
	}
}
