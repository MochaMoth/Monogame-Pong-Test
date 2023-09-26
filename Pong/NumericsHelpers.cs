using System.Numerics;

namespace MochaMoth.Pong.Numerics
{
	internal static class NumericsHelpers
	{
		public static Vector3 SafeNormalize(this Vector3 inputVector)
		{
			Vector3 normalizedVector = Vector3.Normalize(inputVector);

			if (float.IsNaN(normalizedVector.X))
				normalizedVector = normalizedVector with { X = 0 };
			if (float.IsNaN(normalizedVector.Y))
				normalizedVector = normalizedVector with { Y = 0 };
			if (float.IsNaN(normalizedVector.Z))
				normalizedVector = normalizedVector with { Z = 0 };

			return normalizedVector;
		}

		public static Matrix4x4 Invert(this Matrix4x4 inputMatrix)
		{
			Matrix4x4 outputMatrix = new Matrix4x4();
			_ = Matrix4x4.Invert(inputMatrix, out outputMatrix);

			return outputMatrix;
		}
	}
}
