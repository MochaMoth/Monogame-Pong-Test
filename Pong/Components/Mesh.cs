using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MochaMothMedia.Pong.Components
{
	internal class Mesh
	{
		public Model Model { get; set; }
		public Effect Effect { get; set; }
		public PBRTextures Textures { get; set; }

		public Mesh(Model model, Effect effect, PBRTextures textures)
		{
			Model = model;
			Effect = effect;
			Textures = textures;

			foreach (ModelMesh mesh in Model.Meshes)
				foreach (ModelMeshPart part in mesh.MeshParts)
					part.Effect = effect;
		}

		public void ApplyRenderSettings()
		{
			Effect.CurrentTechnique = Effect.Techniques["Ambient"];
			Effect.CurrentTechnique.Passes[0].Apply();
			Effect.Parameters["Base"]?.SetValue(Textures.Base);
			Effect.Parameters["Normal"]?.SetValue(Textures.Normal);
			Effect.Parameters["Height"]?.SetValue(Textures.Height);
			Effect.Parameters["Roughness"]?.SetValue(Textures.Roughness);
			Effect.Parameters["AmbientColor"]?.SetValue(Color.White.ToVector3());
			Effect.Parameters["AmbientIntensity"]?.SetValue(0.4f);
			Effect.Parameters["NormalIntensity"]?.SetValue(1f);
			Effect.Parameters["HeightIntensity"]?.SetValue(1f);
			Effect.Parameters["SpecularIntensity"]?.SetValue(1f);
			Effect.Parameters["RoughnessIntensity"]?.SetValue(1f);
			Effect.Parameters["RoughnessLift"]?.SetValue(0.2f);
		}
	}
}
