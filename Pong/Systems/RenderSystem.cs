using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MochaMothMedia.Pong.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace MochaMothMedia.Pong.Systems
{
	internal class RenderSystem : EntityDrawSystem
	{
		private ComponentMapper<Transform> _transformMapper;
		private ComponentMapper<Mesh> _meshMapper;
		private GraphicsDevice _graphicsDevice;
		private Entity _camera;

		public RenderSystem(GraphicsDevice graphicsDevice) : base(Aspect.All(typeof(Transform), typeof(Mesh)))
		{
			_graphicsDevice = graphicsDevice;
		}

		public override void Initialize(IComponentMapperService mapperService)
		{
			_transformMapper = mapperService.GetMapper<Transform>();
			_meshMapper = mapperService.GetMapper<Mesh>();

			_camera = CreateEntity();
			_camera.Attach(new Transform(
				new Vector3(0, 0, -30),
				Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(-20f))));
			_camera.Attach(new Camera());
		}

		public override void Draw(GameTime gameTime)
		{
			_graphicsDevice.Clear(Color.CornflowerBlue);

			Camera camera = _camera.Get<Camera>();

			foreach (int entityId in ActiveEntities)
			{
				Transform transform = _transformMapper.Get(entityId);
				Mesh mesh = _meshMapper.Get(entityId);

				foreach (ModelMesh modelMesh in mesh.Model.Meshes)
				{
					foreach (BasicEffect effect in modelMesh.Effects)
					{
						effect.World = Matrix.CreateTranslation(transform.Position) * Matrix.CreateFromQuaternion(transform.Rotation);
						effect.View = camera.ViewMatrix;
						effect.Projection = camera.ProjectionMatrix;
						effect.LightingEnabled = true;
						effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0);
						effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);
						effect.DirectionalLight0.SpecularColor = new Vector3(0, 1f, 0);
						effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
						effect.EmissiveColor = new Vector3(0, 0, 0);
					}

					modelMesh.Draw();
				}
			}
		}
	}
}
