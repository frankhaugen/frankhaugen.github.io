<Query Kind="Program">
  <NuGetReference>Raylib-cs</NuGetReference>
  <Namespace>Raylib_cs</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

void Main()
{
	IGameWindow gameWindow = new RaylibGameWindow(1024, 768, "Pong Clone");
	gameWindow.Init();
	gameWindow.SetTargetFPS(60);

	// Paddle variables
	const int paddleWidth = 20;
	const int paddleHeight = 100;
	const int paddleSpeed = 5;
	IPaddle playerPaddle = new Paddle(50, gameWindow.Height / 2 - paddleHeight / 2, paddleWidth, paddleHeight);
	IPaddle computerPaddle = new Paddle(gameWindow.Width - 50 - paddleWidth, gameWindow.Height / 2 - paddleHeight / 2, paddleWidth, paddleHeight);

	// Ball variables
	const int ballSize = 20;
	IBall ball = new Ball(gameWindow.Width / 2 - ballSize / 2, gameWindow.Height / 2 - ballSize / 2, ballSize, ballSize, new Vector2(5, 5));

	while (!gameWindow.ShouldClose())
	{
        // Update
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W) && playerPaddle.Y > 0)
            playerPaddle.MoveUp(paddleSpeed);

        if (Raylib.IsKeyDown(KeyboardKey.KEY_S) && playerPaddle.Y + playerPaddle.Height < gameWindow.Height)
            playerPaddle.MoveDown(paddleSpeed);

        // AI for computer paddle
        if (ball.Y < computerPaddle.Y + computerPaddle.Height / 2 && computerPaddle.Y > 0)
            computerPaddle.MoveUp(paddleSpeed);

        if (ball.Y > computerPaddle.Y + computerPaddle.Height / 2 && computerPaddle.Y + computerPaddle.Height < gameWindow.Height)
            computerPaddle.MoveDown(paddleSpeed);

        // Ball movement
        ball.Move();

        // Ball collision with paddles
        if (ball.CheckCollision(playerPaddle))
            ball.BounceX();
        else if (ball.CheckCollision(computerPaddle))
            ball.BounceX();

        // Ball collision with walls
        if (ball.Y <= 0 || ball.Y + ball.Height >= gameWindow.Height)
            ball.BounceY();

        // Draw
        gameWindow.BeginDrawing();
        gameWindow.ClearBackground(Color.BLACK);

        // Draw paddles
        Raylib.DrawRectangleRec(playerPaddle.GetRectangle(), Color.WHITE);
        Raylib.DrawRectangleRec(computerPaddle.GetRectangle(), Color.WHITE);

		// Draw ball
		Raylib.DrawCircleV(new(ball.X, ball.Y), ball.Width / 2, Color.WHITE);

		gameWindow.EndDrawing();
	}

	gameWindow.Close();
}


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
	bool CheckCollision(IPaddle paddle);
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

	public bool CheckCollision(IPaddle paddle) => Raylib.CheckCollisionRecs(new Rectangle(X, Y, Width, Height), paddle.GetRectangle());
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