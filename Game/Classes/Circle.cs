using Assimp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
	public class Circle
	{
		public List<Vector3> vertices = new List<Vector3>();
		public List<int> indices = new List<int>();

		public int segments = 32;
		public float radius = 3;
		public Circle(int newSegments, float newRadius)
		{
			this.segments = newSegments;
			this.radius = newRadius;

			for (double y = 0; y < 2; y++)
			{
				for (double x = 0; x < segments; x++)
				{
					double theta = (x / (segments - 1)) * 2 * Math.PI;
					vertices.Add(new Vector3()
					{
						X = (float)(radius * Math.Cos(theta)),
						Y = 0,
						Z = (float)(radius * Math.Sin(theta)),
					});
				}
			}

			for (int x = 0; x < segments - 1; x++)
			{
				indices.Add(0);
				indices.Add(x + segments);
				indices.Add(x + segments + 1);
			}

		}
	}
}
