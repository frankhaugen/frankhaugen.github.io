using System;
using System.Numerics;
using RayLib_cs;

public interface IBall
{
	float X { get; }
	float Y { get; }
	int Width { get; }
	int Height { get; }
	Vector2 Speed { get; set; }
	void Move();
	void BounceX();
	void BounceY();
	bool CheckCollision(IPaddle paddle, Func<IPaddle, bool> collisionRule);
}

public class RaylibGameWindow : IGameWindow
{
	private readonly int _width;
	private readonly int _height;
	private readonly string _title;

	public int Width => _width;
	public int Height => _height;

	public RaylibGameWindow(int width, int height, string title)
	{
		_width = width;
		_height = height;
		_title = title;
	}

	public void Init() => Raylib.InitWindow(_width, _height, _title);

	public bool ShouldClose() => Raylib.WindowShouldClose();

	public void SetTargetFPS(int fps) => Raylib.SetTargetFPS(fps);

	public void BeginDrawing() => Raylib.BeginDrawing();

	public void ClearBackground(Color color) => Raylib.ClearBackground(color);

	public void EndDrawing() => Raylib.EndDrawing();

	public void Close() => Raylib.CloseWindow();
}

public class Ball : IBall
{
	public float X { get; private set; }
	public float Y { get; private set; }
	public int Width { get; private set; }
	public int Height { get; private set; }
	public Vector2 Speed { get; set; }

	public Ball(float x, float y, int width, int height, Vector2 speed)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
		Speed = speed;
	}

	public void Move() => (X, Y) = (X + Speed.X, Y + Speed.Y);

	public void BounceX() => Speed = new Vector2(-Speed.X, Speed.Y);

	public void BounceY() => Speed = new Vector2(-Speed.Y, Speed.X);

	public bool CheckCollision(IPaddle paddle, Func<IPaddle, bool> collisionRule) => collisionRule.Invoke(paddle);
}


public interface IPaddle
{
	float X { get; }
	float Y { get; }
	int Width { get; }
	int Height { get; }
	void MoveUp(int speed);
	void MoveDown(int speed);
	Rectangle GetRectangle();
}

public class Paddle : IPaddle
{
	public float X { get; private set; }
	public float Y { get; private set; }
	public int Width { get; private set; }
	public int Height { get; private set; }

	public Paddle(float x, float y, int width, int height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public void MoveUp(int speed) => Y -= speed;

	public void MoveDown(int speed) => Y += speed;

	public Rectangle GetRectangle() => new Rectangle(X, Y, Width, Height);
}


public interface IGameWindow
{
	int Width { get; }
	int Height { get; }
	void Init();
	bool ShouldClose();
	void SetTargetFPS(int fps);
	void BeginDrawing();
	void ClearBackground(Color color);
	void EndDrawing();
	void Close();
}
