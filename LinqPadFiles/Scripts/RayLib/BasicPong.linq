<Query Kind="Program">
  <NuGetReference>Raylib-cs</NuGetReference>
  <Namespace>Raylib_cs</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

void Main()
{
    Vector2 loadedWindowSize = new(1024, 768);
    int screenWidth = (int)loadedWindowSize.X;
    int screenHeight = (int)loadedWindowSize.Y;

    Raylib.InitWindow(screenWidth, screenHeight, "Pong Clone");
    Raylib.SetTargetFPS(60);

    // Paddle variables
    const int paddleWidth = 20;
    const int paddleHeight = 100;
    const int paddleSpeed = 5;
    Paddle playerPaddle = new Paddle(50, screenHeight / 2 - paddleHeight / 2, paddleWidth, paddleHeight);
    Paddle computerPaddle = new Paddle(screenWidth - 50 - paddleWidth, screenHeight / 2 - paddleHeight / 2, paddleWidth, paddleHeight);

    // Ball variables
    const int ballSize = 20;
    Ball ball = new Ball(screenWidth / 2 - ballSize / 2, screenHeight / 2 - ballSize / 2, ballSize, ballSize, new Vector2(5, 5));

    while (!Raylib.WindowShouldClose())
    {
        // Update
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W) && playerPaddle.Y > 0)
        {
            playerPaddle.MoveUp(paddleSpeed);
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_S) && playerPaddle.Y + playerPaddle.Height < screenHeight)
        {
            playerPaddle.MoveDown(paddleSpeed);
        }

        // AI for computer paddle
        if (ball.Y < computerPaddle.Y + computerPaddle.Height / 2 && computerPaddle.Y > 0)
        {
            computerPaddle.MoveUp(paddleSpeed);
        }
        if (ball.Y > computerPaddle.Y + computerPaddle.Height / 2 && computerPaddle.Y + computerPaddle.Height < screenHeight)
        {
            computerPaddle.MoveDown(paddleSpeed);
        }

        // Ball movement
        ball.Move();

        // Ball collision with paddles
        if (ball.CheckCollision(playerPaddle))
        {
            ball.BounceX();
        }
        else if (ball.CheckCollision(computerPaddle))
        {
            ball.BounceX();
        }

        // Ball collision with walls
        if (ball.Y <= 0 || ball.Y + ball.Height >= screenHeight)
        {
            ball.BounceY();
        }

        // Draw
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.BLACK);

        // Draw paddles
        Raylib.DrawRectangleRec(playerPaddle.GetRectangle(), Color.WHITE);
        Raylib.DrawRectangleRec(computerPaddle.GetRectangle(), Color.WHITE);

        // Draw ball
        Raylib.DrawRectangle((int)ball.X, (int)ball.Y, ball.Width, ball.Height, Color.WHITE);

        Raylib.EndDrawing();
    }

    Raylib.CloseWindow();
}

public class Paddle
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

    public void MoveUp(int speed)
    {
        Y -= speed;
    }

    public void MoveDown(int speed)
    {
        Y += speed;
    }

    public Rectangle GetRectangle()
    {
        return new Rectangle(X, Y, Width, Height);
    }
}

public class Ball
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

	public void Move()
	{
		X += Speed.X;
		Y += Speed.Y;
	}

	public void BounceX()
	{
		Speed = new Vector2(-Speed.X, Speed.Y);
	}
	
	public void BounceY()
	{
		Speed = new Vector2(-Speed.Y, Speed.X);
	}

	public bool CheckCollision(Paddle paddle)
	{
		return Raylib.CheckCollisionRecs(new Rectangle(X, Y, Width, Height), paddle.GetRectangle());
	}
}