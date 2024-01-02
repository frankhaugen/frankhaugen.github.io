using System;
using System.Numerics;
using System.Threading;

class Program
{
    static Vector3[] vertices = new Vector3[]
    {
        new Vector3(-1, -1, -1), // 0
        new Vector3(1, -1, -1),  // 1
        new Vector3(1, 1, -1),   // 2
        new Vector3(-1, 1, -1),  // 3
        new Vector3(-1, -1, 1),  // 4
        new Vector3(1, -1, 1),   // 5
        new Vector3(1, 1, 1),    // 6
        new Vector3(-1, 1, 1)    // 7
    };

    static (int, int)[] edges = new (int, int)[]
    {
        (0, 1), (1, 2), (2, 3), (3, 0),
        (4, 5), (5, 6), (6, 7), (7, 4),
        (0, 4), (1, 5), (2, 6), (3, 7)
    };

    static Vector3 cameraPosition = new Vector3(30, 5, 30);
    static Vector3 cameraLookAt = Vector3.Zero;

    static void Main(string[] args)
    {
        double angle = 0;
        while (true)
        {
            Console.Clear();
            DrawCube(angle);
            Thread.Sleep(100);
            angle += 0.04;
        }
    }

    static void DrawCube(double angle)
    {
        int originX = Console.WindowWidth / 2;
        int originY = Console.WindowHeight / 2;
        char ch = '█';

        foreach (var edge in edges)
        {
            Vector3 v1 = Rotate(vertices[edge.Item1] * 10, angle);
            Vector3 v2 = Rotate(vertices[edge.Item2] * 10, angle);

            // Translate and Project
            Vector3 v1t = Transform(v1, originX, originY);
            Vector3 v2t = Transform(v2, originX, originY);

            DrawLine(v1t, v2t, ch);
        }
    }

    static Vector3 Rotate(Vector3 v, double angle)
    {
        double cos = Math.Cos(angle);
        double sin = Math.Sin(angle);

        // Rotate around y-axis
        v = new Vector3((float)(v.X * cos - v.Z * sin), v.Y, (float)(v.X * sin + v.Z * cos));
        return v;
    }

    static Vector3 Transform(Vector3 v, int originX, int originY)
    {
        // Translate camera to origin
        v -= cameraPosition;

        // Camera rotation
        Vector3 forward = Vector3.Normalize(cameraLookAt - cameraPosition);
        Vector3 right = Vector3.Normalize(Vector3.Cross(forward, new Vector3(0, 1, 0)));
        Vector3 up = Vector3.Normalize(Vector3.Cross(forward, right));
        v = new Vector3(Vector3.Dot(v, right), Vector3.Dot(v, up), Vector3.Dot(v, forward));

        // Perspective projection
        double fov = Math.PI / 4.0;
        double scale = Console.WindowHeight / Math.Tan(fov / 2.0) / 2.0;
        v.X = originX + (int)(v.X / v.Z * scale);
        v.Y = originY - (int)(v.Y / v.Z * scale);

        return v;
    }

    static void DrawLine(Vector3 v1, Vector3 v2, char ch)
    {
        double dx = v2.X - v1.X;
        double dy = v2.Y - v1.Y;
        double steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

        for (int i = 0; i <= steps; i++)
        {
            double t = steps == 0 ? 0.0 : i / steps;
            int x = (int)(v1.X + t * dx);
            int y = (int)(v1.Y + t * dy);
            if (x >= 0 && x < Console.WindowWidth && y >= 0 && y < Console.WindowHeight)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(ch);
            }
        }
    }
}
