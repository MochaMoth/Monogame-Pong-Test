using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Vector3 = System.Numerics.Vector3;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Color = Microsoft.Xna.Framework.Color;

namespace MochaMothMedia.Pong.Systems
{
	internal class DebugToolsRenderSystem : EntityDrawSystem
	{
		private readonly GraphicsDevice _graphicsDevice;

		public DebugToolsRenderSystem(GraphicsDevice graphicsDevice) : base(Aspect.All())
		{
			_graphicsDevice = graphicsDevice;
		}

		public override void Initialize(IComponentMapperService mapperService)
		{
		}

		public override void Draw(GameTime gameTime)
		{
			VertexPositionColor[] vertexPositionsX = new VertexPositionColor[]
			{
				new VertexPositionColor(Vector3.UnitX, Color.Red),
				new VertexPositionColor(-Vector3.UnitX, Color.Red)
			};
			VertexPositionColor[] vertexPositionsY = new VertexPositionColor[]
			{
				new VertexPositionColor(Vector3.UnitY, Color.Red),
				new VertexPositionColor(-Vector3.UnitY, Color.Red)
			};
			VertexPositionColor[] vertexPositionsZ = new VertexPositionColor[]
			{
				new VertexPositionColor(Vector3.UnitZ, Color.Red),
				new VertexPositionColor(-Vector3.UnitZ, Color.Red)
			};
			_graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionsX, 0, 1);
			_graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionsY, 0, 1);
			_graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionsZ, 0, 1);
		}
	}
}
