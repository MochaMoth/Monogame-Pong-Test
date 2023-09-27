using Microsoft.Xna.Framework;
using MochaMothMedia.Pong.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace MochaMothMedia.Pong.ThirdPersonController
{
	internal class DollyController : EntityProcessingSystem
	{
		private ComponentMapper<CameraDolly> _cameraDollyMapper;

		public DollyController() : base(Aspect.All(typeof(CameraDolly))) { }

		public override void Initialize(IComponentMapperService mapperService)
		{
			_cameraDollyMapper = mapperService.GetMapper<CameraDolly>();
		}

		public override void Process(GameTime gameTime, int entityId)
		{
			CameraDolly cameraDolly = _cameraDollyMapper.Get(entityId);

			Vector2 lookVector = InputSystem.CurrentState.LookVector;
			cameraDolly.Pitch += lookVector.Y * cameraDolly.PitchSpeed * gameTime.ElapsedGameTime.Milliseconds;
			cameraDolly.Yaw += lookVector.X * cameraDolly.YawSpeed * gameTime.ElapsedGameTime.Milliseconds;

			if (cameraDolly.Pitch > cameraDolly.UpperPitchLimit)
				cameraDolly.Pitch = cameraDolly.UpperPitchLimit;
			if (cameraDolly.Pitch < cameraDolly.LowerPitchLimit)
				cameraDolly.Pitch = cameraDolly.LowerPitchLimit;
		}
	}
}
