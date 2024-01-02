<Query Kind="Program">
  <NuGetReference>Raylib-cs</NuGetReference>
  <Namespace>Raylib_cs</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

#load ".\*.cs"


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



