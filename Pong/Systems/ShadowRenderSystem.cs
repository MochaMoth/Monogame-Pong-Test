using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MochaMothMedia.Pong.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace MochaMothMedia.Pong.Systems
{
	internal class ShadowRenderSystem : EntityDrawSystem
	{
		private ComponentMapper<Transform> _transformMapper;
		private ComponentMapper<Mesh> _meshMapper;
		private GraphicsDevice _graphicsDevice;
		
		public ShadowRenderSystem(GraphicsDevice graphicsDevice) : base(Aspect.All(typeof(Transform), typeof(Mesh)))
		{
			_graphicsDevice = graphicsDevice;
		}

		public override void Initialize(IComponentMapperService mapperService)
		{
			_transformMapper = mapperService.GetMapper<Transform>();
			_meshMapper = mapperService.GetMapper<Mesh>();
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (Entity spotLightEntity in Statics.Lights.SpotLights)
			{
				SpotLight spotLight = spotLightEntity.Get<SpotLight>();
				_graphicsDevice.SetRenderTarget(spotLight.RenderTarget);
				_graphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

				foreach (int entityId in ActiveEntities)
				{
					Transform transform = _transformMapper.Get(entityId);
					Mesh mesh = _meshMapper.Get(entityId);

					foreach (ModelMesh modelMesh in mesh.Model.Meshes)
					{
						Matrix worldMatrix = Matrix.CreateFromQuaternion(transform.Rotation) * Matrix.CreateTranslation(transform.Position) * modelMesh.ParentBone.Transform;

						foreach (ModelMeshPart part in modelMesh.MeshParts)
						{
							part.Effect.CurrentTechnique = part.Effect.Techniques["ShadowMap"];
							part.Effect.Parameters["World"]?.SetValue(worldMatrix);
							part.Effect.Parameters["LightView"]?.SetValue(spotLight.ViewMatrix);
							part.Effect.Parameters["LightProjection"]?.SetValue(spotLight.ProjectionMatrix);
						}

						modelMesh.Draw();
					}
				}

				spotLight.ShadowMap = spotLight.RenderTarget;
				_graphicsDevice.SetRenderTarget(null);
			}
		}
	}
}
