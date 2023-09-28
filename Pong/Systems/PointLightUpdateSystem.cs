using MochaMothMedia.Pong.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Pong.Systems
{
	internal class PointLightUpdateSystem : EntityDrawSystem
	{
		ComponentMapper<Transform> _transformMapper;
		ComponentMapper<PointLight> _pointLightMapper;

		public PointLightUpdateSystem() : base(Aspect.All(typeof(Transform), typeof(PointLight))) { }

		public override void Initialize(IComponentMapperService mapperService)
		{
			_transformMapper = mapperService.GetMapper<Transform>();
			_pointLightMapper = mapperService.GetMapper<PointLight>();
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (int entityId in ActiveEntities)
			{
				PointLight pointLight = _pointLightMapper.Get(entityId);
				Transform transform = _transformMapper.Get(entityId);
			}
		}
	}
}
