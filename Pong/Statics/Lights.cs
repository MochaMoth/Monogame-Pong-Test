using MonoGame.Extended.Entities;
using System.Collections.Generic;

namespace MochaMothMedia.Pong.Statics
{
	internal static class Lights
	{
		public static List<Entity> PointLights { get; set; } = new List<Entity>();
		public static List<Entity> SpotLights { get; set; } = new List<Entity>();
	}
}
