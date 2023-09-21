using Microsoft.Xna.Framework.Graphics;

namespace MochaMothMedia.Pong.Components
{
	internal class Mesh
	{
		public Model Model { get; set; }

		public Mesh(Model model)
		{
			Model = model;
		}
	}
}
