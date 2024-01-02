<Query Kind="Statements">
  <NuGetReference>MonoGame.Framework.WindowsDX</NuGetReference>
  <Namespace>Microsoft.Xna.Framework</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics</Namespace>
</Query>

public struct Polygon
{
	public Vector2[] Vertices { get; set; }

	public Polygon(Vector2[] vertices) => Vertices = vertices;


}

public static class PolygonExtensions
	{
		public static Polygon Translate(this Polygon polygon, Vector2 position)
	{
		var translatedVertices = polygon.Vertices.Select(vertex => vertex + position).ToArray();
		return new Polygon(translatedVertices);
	}
	
	public static Vector2 OptimalFlowDirection(this Polygon polygon)
	{
		// Calculate the average normal vector of the polygon's edges
		var normalSum = Vector2.Zero;
		for (int i = 0; i < polygon.Vertices.Length - 1; i++)
		{
			var edge = polygon.Vertices[i + 1] - polygon.Vertices[i];
			var normal = new Vector2(edge.Y, -edge.X);
			normalSum += Vector2.Normalize(normal);
		}
		var averageNormal = Vector2.Normalize(normalSum);

		// The optimal flow direction is the opposite of the average normal vector
		return -averageNormal;
	}
	
	public static float GetAngleInDegrees(this Vector2 vector)
	{
		return (float)(Math.Atan2(vector.Y, vector.X) * 180 / Math.PI);
	}

	public static Polygon RotateToDirection(this Polygon polygon, float angle)
	{
		var rotatedVertices = new List<Vector2>();

		double sin = Math.Sin(angle);
		double cos = Math.Cos(angle);

		foreach (Vector2 point in polygon.Vertices)
		{
			float rotatedX = (float)(point.X * cos - point.Y * sin);
			float rotatedY = (float)(point.X * sin + point.Y * cos);
			rotatedVertices.Add(new Vector2(rotatedX, rotatedY));
		}

		return new Polygon(rotatedVertices.ToArray());
	}

	public static float CalculateDragCoefficient(this Polygon polygon, float density, float velocity)
	{
		float referenceArea = polygon.Area();
		float dragForce = 0.5f * density * velocity * velocity * referenceArea;
		float dynamicPressure = 0.5f * density * velocity * velocity;
		float dragCoefficient = dragForce / dynamicPressure;
		return dragCoefficient;
	}

	public static float Area(this Polygon polygon)
	{
		// Calculate the area of the polygon using the Shoelace Theorem
		// This involves summing the products of the x and y coordinates of the vertices,
		// alternating between adding and subtracting them
		float area = 0.0f;
		for (int i = 0; i < polygon.Vertices.Count(); i++)
		{
			Vector2 v1 = polygon.Vertices[i];
			Vector2 v2 = polygon.Vertices[(i + 1) % polygon.Vertices.Count()];
			area += v1.X * v2.Y - v1.Y * v2.X;
		}
		area = Math.Abs(area) / 2.0f;
		return area;
	}


	public static bool Intersects(this Polygon polygon1, Polygon polygon2)
	{
		// Check if any of the vertices of polygon1 are contained within polygon2
		for (int i = 0; i < polygon1.Vertices.Length; i++)
		{
			if (polygon2.Vertices.Contains(polygon1.Vertices[i]))
			{
				return true;
			}
		}

		// Check if any of the vertices of polygon2 are contained within polygon1
		for (int i = 0; i < polygon2.Vertices.Length; i++)
		{
			if (polygon1.Vertices.Contains(polygon2.Vertices[i]))
			{
				return true;
			}
		}

		// Check if any of the edges of polygon1 intersect with any of the edges of polygon2
		for (int i = 0; i < polygon1.Vertices.Length - 1; i++)
		{
			for (int j = 0; j < polygon2.Vertices.Length - 1; j++)
			{
				if (Intersects(polygon1.Vertices[i], polygon1.Vertices[i + 1], polygon2.Vertices[j], polygon2.Vertices[j + 1]))
				{
					return true;
				}
			}
		}

		return false;
	}


	private static bool Intersects(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
	{
		// Check if the lines are parallel
		if ((b.Y - a.Y) / (b.X - a.X) == (d.Y - c.Y) / (d.X - c.X))
		{
			return false;
		}
		// Check if the lines intersect
		if (a.X < b.X)
		{
			if (c.X < d.X)
			{
				return Math.Max(a.X, c.X) <= Math.Min(b.X, d.X);
			}
			else
			{
				return Math.Max(a.X, d.X) <= Math.Min(b.X, c.X);
			}
		}
		else
		{
			if (c.X < d.X)
			{
				return Math.Max(b.X, c.X) <= Math.Min(a.X, d.X);
			}
			else
			{
				return Math.Max(b.X, d.X) <= Math.Min(a.X, c.X);
			}
		}

	}
}

public static class PolygonFactory
{
	public static Polygon GetLine(Vector2 start, Vector2 end) => new Polygon(new[] { start, end });

	public static Polygon GetCircle(Vector2 center, int numSegments, float radius)
	{
		Vector2[] vertices = new Vector2[numSegments];
		float angleStep = MathHelper.TwoPi / (float)numSegments;

		for (int i = 0; i < numSegments; i++)
		{
			float angle = i * angleStep;
			vertices[i] = center + radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		return new Polygon(vertices);
	}

	public static Polygon GetTriangle(Vector2 position, float scale = 1f) =>
		new Polygon(new[]
		{
			position,
			position + new Vector2(0.5f * scale, -0.5f * scale),
			position + new Vector2(-0.5f * scale, -0.5f * scale),
		});

	public static Polygon GetSquare(Vector2 position, float scale = 1f) =>
		new Polygon(new[]
		{
			position + new Vector2(-0.5f * scale, -0.5f * scale),
			position + new Vector2(0.5f * scale, -0.5f * scale),
			position + new Vector2(0.5f * scale, 0.5f * scale),
			position + new Vector2(-0.5f * scale, 0.5f * scale),
		});

	public static Polygon GetPentagon(Vector2 position, float scale = 1f) =>
		new Polygon(new[]
		{
		position + new Vector2(-0.5f * scale, -0.5f * scale),
		position + new Vector2(-0.1f * scale, -0.5f * scale),
		position + new Vector2(0.5f * scale, 0f * scale),
		position + new Vector2(-0.1f * scale, 0.5f * scale),
		position + new Vector2(-0.5f * scale, 0.5f * scale),
		});

	public static Polygon GetArtilleryShellPolygon(Vector2 position, float scale = 1f)
	{
		var vertices = new Vector2[]
		{
		new Vector2(-0.155f * scale / 2, 0),
		new Vector2(-0.155f * scale / 2, -0.640f * scale / 3),
		new Vector2(0.155f * scale / 2, -0.640f * scale / 3),
		new Vector2(0.155f * scale / 2, 0),
		new Vector2(0, 0.640f * scale / 3)
		};

		var polygon = new Polygon(vertices);
		return polygon.Translate(position);
	}


}

