using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MochaMothMedia.Pong.Components
{
	internal class Mesh
	{
		public Model Model { get; set; }

		public Mesh(Model model, Effect effect)
		{
			Model = model;

			foreach (ModelMesh mesh in Model.Meshes)
			{
				foreach (ModelMeshPart part in mesh.MeshParts)
				{
					part.Effect = effect;
					part.Effect.CurrentTechnique = part.Effect.Techniques["Ambient"];
					part.Effect.CurrentTechnique.Passes[0].Apply();

					part.Effect.Parameters["AmbientColor"].SetValue(Color.White.ToVector3());
					part.Effect.Parameters["AmbientIntensity"].SetValue(1);
				}
			}
		}
	}
}
