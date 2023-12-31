﻿using MochaMothMedia.Pong.Components;
using MochaMothMedia.Pong.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using GameTime = Microsoft.Xna.Framework.GameTime;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using Matrix = System.Numerics.Matrix4x4;
using Vector3 = System.Numerics.Vector3;
using Vector2 = System.Numerics.Vector2;
using Quaternion = System.Numerics.Quaternion;
using MochaMoth.Pong.Numerics;

namespace MochaMothMedia.Pong.ThirdPersonController
{
	internal class PlayerController : EntityProcessingSystem
	{
		ComponentMapper<Transform> _transformMapper;
		ComponentMapper<Player> _playerMapper;

		public PlayerController() : base(Aspect.All(typeof(Transform), typeof(Player))) { }

		public override void Initialize(IComponentMapperService mapperService)
		{
			_transformMapper = mapperService.GetMapper<Transform>();
			_playerMapper = mapperService.GetMapper<Player>();
		}

		public override void Process(GameTime gameTime, int entityId)
		{
			Transform transform = _transformMapper.Get(entityId);
			Player player = _playerMapper.Get(entityId);
			CameraDolly dolly = player.CameraDolly.Get<CameraDolly>();

			Vector2 movementInput = InputSystem.CurrentState.XZPlane;
			movementInput = movementInput.SafeNormalize();

			Vector3 movementVector = new Vector3(movementInput.X, 0, movementInput.Y);
			Vector3 worldSpaceMovement = Vector3.Transform(movementVector, Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(dolly.Yaw)));
			Vector3 targetPosition = transform.Position + (worldSpaceMovement * player.RunSpeed * gameTime.ElapsedGameTime.Milliseconds);
			Vector3 newForward = (targetPosition - transform.Position).SafeNormalize();

			if (newForward != Vector3.Zero)
				transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(180f)) * Quaternion.CreateFromRotationMatrix(Matrix.CreateWorld(transform.Position, worldSpaceMovement, Vector3.UnitY));

			transform.Position = targetPosition;
		}
	}
}
