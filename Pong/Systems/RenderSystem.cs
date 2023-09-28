using Microsoft.Xna.Framework.Graphics;
using MochaMothMedia.Pong.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Matrix = Microsoft.Xna.Framework.Matrix;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Vector3 = System.Numerics.Vector3;

namespace MochaMothMedia.Pong.Systems
{
	internal class RenderSystem : EntityDrawSystem
	{
		private ComponentMapper<Transform> _transformMapper;
		private ComponentMapper<Mesh> _meshMapper;
		private GraphicsDevice _graphicsDevice;

		public RenderSystem(GraphicsDevice graphicsDevice) : base(Aspect.All(typeof(Transform), typeof(Mesh)))
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
			Camera camera = Statics.Camera.Main.Get<Camera>();
			Vector3 cameraPosition = Statics.Camera.Main.Get<Transform>().Position;

			foreach (Entity spotLightEntity in Statics.Lights.SpotLights)
			{
				SpotLight spotLight = spotLightEntity.Get<SpotLight>();
				Transform spotLightTransform = spotLightEntity.Get<Transform>();

				foreach (int entityId in ActiveEntities)
				{
					Transform transform = _transformMapper.Get(entityId);
					Mesh mesh = _meshMapper.Get(entityId);
					mesh.ApplyRenderSettings();

					foreach (ModelMesh modelMesh in mesh.Model.Meshes)
					{
						Matrix worldMatrix = Matrix.CreateFromQuaternion(transform.Rotation) * Matrix.CreateTranslation(transform.Position) * modelMesh.ParentBone.Transform;

						foreach (ModelMeshPart part in modelMesh.MeshParts)
						{
							part.Effect.CurrentTechnique = part.Effect.Techniques["Ambient"];
							part.Effect.Parameters["LightPosition"]?.SetValue(spotLightTransform.Position);
							part.Effect.Parameters["LightPower"]?.SetValue(spotLight.Power);
							part.Effect.Parameters["World"]?.SetValue(worldMatrix);
							part.Effect.Parameters["LightView"]?.SetValue(spotLight.ViewMatrix);
							part.Effect.Parameters["LightProjection"]?.SetValue(spotLight.ProjectionMatrix);
							part.Effect.Parameters["View"]?.SetValue(camera.ViewMatrix);
							part.Effect.Parameters["ViewPosition"]?.SetValue(cameraPosition);
							part.Effect.Parameters["Projection"]?.SetValue(camera.ProjectionMatrix);
							part.Effect.Parameters["ShadowMap"]?.SetValue(spotLight.ShadowMap);
						}

						modelMesh.Draw();
					}
				}
			}
		}
	}
}
