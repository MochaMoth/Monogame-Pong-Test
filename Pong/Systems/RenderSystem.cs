using Microsoft.Xna.Framework.Graphics;
using MochaMothMedia.Pong.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Vector3 = System.Numerics.Vector3;
using Matrix = Microsoft.Xna.Framework.Matrix;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Color = Microsoft.Xna.Framework.Color;

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
			_graphicsDevice.Clear(Color.Black);

			Camera camera = Statics.Camera.Main.Get<Camera>();

			foreach (int entityId in ActiveEntities)
			{
				Transform transform = _transformMapper.Get(entityId);
				Mesh mesh = _meshMapper.Get(entityId);

				foreach (ModelMesh modelMesh in mesh.Model.Meshes)
				{
					//foreach (ModelMeshPart part in modelMesh.MeshParts)
					//{
					//	part.Effect.Parameters["World"].SetValue(Matrix.CreateFromQuaternion(transform.Rotation) * Matrix.CreateTranslation(transform.Position) * modelMesh.ParentBone.Transform);
					//	part.Effect.Parameters["View"].SetValue(camera.ViewMatrix);
					//	part.Effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
					//}
					foreach (BasicEffect effect in modelMesh.Effects)
					{
						effect.World = Matrix.CreateFromQuaternion(transform.Rotation) * Matrix.CreateTranslation(transform.Position) * modelMesh.ParentBone.Transform;
						effect.View = camera.ViewMatrix;
						effect.Projection = camera.ProjectionMatrix;
					}

					modelMesh.Draw();
				}
			}
		}
	}
}
