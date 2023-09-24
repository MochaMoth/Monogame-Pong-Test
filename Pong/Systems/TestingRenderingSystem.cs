using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Vector3 = System.Numerics.Vector3;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Color = Microsoft.Xna.Framework.Color;
using Content = MochaMothMedia.Pong.Statics.Content;
using MochaMothMedia.Pong.Components;

namespace MochaMothMedia.Pong.Systems
{
	struct MyOwnVertexFormat
	{
		public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
			new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0)
		);

		private Vector3 _position;
		private Color _color;

		public MyOwnVertexFormat(Vector3 position, Color color)
		{
			_position = position;
			_color = color;
		}
	}

	internal class TestingRenderingSystem : EntityDrawSystem
	{
		private readonly GraphicsDevice _graphicsDevice;

		VertexBuffer _vertexBuffer;

		public TestingRenderingSystem(GraphicsDevice graphicsDevice) : base(Aspect.All())
		{
			_graphicsDevice = graphicsDevice;
		}

		public override void Initialize(IComponentMapperService mapperService)
		{
			SetUpVertices();
		}

		public override void Draw(GameTime gameTime)
		{
			Camera camera = Statics.Camera.Main.Get<Camera>();

			//_graphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
			_graphicsDevice.Clear(Color.Black);

			//Content.TestingEffect.CurrentTechnique = Content.TestingEffect.Techniques["Simplest"];
			//Content.TestingEffect.Parameters["xWorld"].SetValue(Matrix.Identity);
			BasicEffect effect = new BasicEffect(_graphicsDevice);
			effect.CurrentTechnique = effect.Techniques[0];
			effect.View = camera.ViewMatrix;
			effect.Projection = camera.ProjectionMatrix;

			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();

				_graphicsDevice.SetVertexBuffer(_vertexBuffer);
				_graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
			}

			_graphicsDevice.Present();
		}

		private void SetUpVertices()
		{
			MyOwnVertexFormat[] vertices = new MyOwnVertexFormat[3];

			vertices[0] = new MyOwnVertexFormat(new Vector3(-2, 2, 0), Color.Red);
			vertices[1] = new MyOwnVertexFormat(new Vector3(2, -2, -2), Color.Green);
			vertices[2] = new MyOwnVertexFormat(new Vector3(0, 0, 2), Color.Yellow);

			_vertexBuffer = new VertexBuffer(_graphicsDevice, MyOwnVertexFormat.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
			_vertexBuffer.SetData(vertices);
		}
	}
}
