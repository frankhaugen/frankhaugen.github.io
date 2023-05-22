<Query Kind="Program">
  <NuGetReference>MonoGame.Framework.WindowsDX</NuGetReference>
  <Namespace>Microsoft.Xna.Framework</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics</Namespace>
  <Namespace>Microsoft.Xna.Framework.Input</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
    using (var game = new MyGame())
    {
        game.Run();
    }
}

//public class MyGame : Game
//{
//    GraphicsDeviceManager graphics;
//    SpriteBatch spriteBatch;
//
//    public MyGame()
//    {
//        graphics = new GraphicsDeviceManager(this);
//        Content.RootDirectory = "Content";
//    }
//
//    protected override void LoadContent()
//    {
//        spriteBatch = new SpriteBatch(GraphicsDevice);
//    }
//
//    protected override void Update(GameTime gameTime)
//    {
//        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
//            Exit();
//
//        base.Update(gameTime);
//    }
//
//    protected override void Draw(GameTime gameTime)
//    {
//        GraphicsDevice.Clear(Color.Black);
//
//        using var effect = new BasicEffect(GraphicsDevice);
//        effect.VertexColorEnabled = true;
//        effect.World = Matrix.Identity;
//        effect.View = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
//        effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 100);
//
//        foreach (var pass in effect.CurrentTechnique.Passes)
//        {
//            pass.Apply();
//            GraphicsDevice.DrawPolygon(polygon, Color.LawnGreen, PrimitiveType.LineList);
//        }
//
//        base.Draw(gameTime);
//    }
//}

public class MyGame : Game
{
    private const int ScoreToWin = 10;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Ball _ball;
    private Paddle _paddle1;
    private Paddle _paddle2;
    private DebugSquare _debugSquare = new DebugSquare();
    private int _player1Score;
    private int _player2Score;
    private bool _isGameOver;

    public MyGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        
        base.IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create ball
        _ball = new Ball(new Vector2(400, 240), new Vector2(-Ball.Speed, 0), 2, 32, Color.White);

        // Create paddles
        var paddleVertices = new[]
        {
            new Vector2(0, 0),
            new Vector2(10, 0),
            new Vector2(10, 80),
            new Vector2(0, 80)
        };
        var paddlePolygon = new Polygon(paddleVertices);
        _paddle1 = new Paddle(new Vector2(20, 200), Vector2.Zero, paddlePolygon, Color.White);
        _paddle2 = new Paddle(new Vector2(770, 200), Vector2.Zero, paddlePolygon, Color.White);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        if (_isGameOver)
        {
            return;
        }

        _ball.Update(gameTime);
        _paddle1.Update(gameTime);
        _paddle2.Update(gameTime);

        // Check for collision with paddles
        if (PolygonHelper.Intersects(_ball.Polygon, _paddle1.Polygon))
        {
            _ball.Velocity.X = Ball.Speed;
        }
        else if (PolygonHelper.Intersects(_ball.Polygon, _paddle2.Polygon))
        {
            _ball.Velocity.X = -Ball.Speed;
        }

        // Check for scoring
        if (_ball.Position.X < 0)
        {
            _player2Score++;
            if (_player2Score >= ScoreToWin)
            {
                _isGameOver = true;
            }
            else
            {
                _ball.Reset();
            }
        }
        else if (_ball.Position.X > 800)
        {
            _player1Score++;
            if (_player1Score >= ScoreToWin)
            {
                _isGameOver = true;
            }
            else
            {
                _ball.Reset();
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        using var effect = new BasicEffect(GraphicsDevice);
        effect.VertexColorEnabled = true;
        effect.World = Matrix.Identity;
        effect.View = Matrix.CreateLookAt(new Vector3(0, 0, -1000), Vector3.Zero, Vector3.Up);
        effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 1000);

        foreach (var pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();

            // Draw ball and paddles
            GraphicsDevice.DrawPolygon(_ball.Polygon, _ball.Color, PrimitiveType.LineList);
            GraphicsDevice.DrawPolygon(_paddle1.Polygon, _paddle1.Color, PrimitiveType.LineList);
            GraphicsDevice.DrawPolygon(_paddle2.Polygon, _paddle2.Color, PrimitiveType.LineList);
            GraphicsDevice.DrawPolygon(_debugSquare.Polygon, _debugSquare.Color, PrimitiveType.LineList);
            
        }
        // Draw scores
        //var font = new SpriteFont(
        //_spriteBatch.DrawString(font, $"{_player1Score}", new Vector2(350, 10), Color.White);
        //_spriteBatch.DrawString(font, $"{_player2Score}", new Vector2(450, 10), Color.White);
    
        // Draw game over message
        if (_isGameOver)
        {
            var message = _player1Score >= ScoreToWin ? "Player 1 wins!" : "Player 2 wins!";
            //var messageSize = font.MeasureString(message);
            //_spriteBatch.DrawString(font, message, new Vector2(400 - messageSize.X / 2, 240 - messageSize.Y / 2), Color.White);
        }

        //_spriteBatch.End();

        base.Draw(gameTime);
    }
}

public abstract class GameObject
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Polygon Polygon { get; set; }
    public Color Color { get; set; }

    public GameObject(Vector2 position, Vector2 velocity, Polygon polygon, Color color)
    {
        Position = position;
        Velocity = velocity;
        Polygon = polygon;
        Color = color;
    }

    public virtual void Update(GameTime gameTime)
    {
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}

public struct PongGameState
{
    public Vector2 BallPosition { get; set; }
    public Vector2 BallVelocity { get; set; }
    public Vector2 Paddle1Position { get; set; }
    public Vector2 Paddle2Position { get; set; }
    public int Player1Score { get; set; }
    public int Player2Score { get; set; }
    public bool IsGameOver { get; set; }
}


public struct Polygon
{
    private Vector2[] _vertices;

    public Polygon(Vector2[] vertices)
    {
        _vertices = vertices;
    }

    public Vector2[] Vertices
    {
        get { return _vertices; }
        set { _vertices = value; }
    }

    public float Area()
    {
        float area = 0f;
        int j = _vertices.Length - 1;

        for (int i = 0; i < _vertices.Length; i++)
        {
            area += (_vertices[j].X + _vertices[i].X) * (_vertices[j].Y - _vertices[i].Y);
            j = i;
        }

        return Math.Abs(area / 2f);
    }

    public float Perimeter()
    {
        float perimeter = 0f;
        int j = _vertices.Length - 1;

        for (int i = 0; i < _vertices.Length; i++)
        {
            perimeter += Vector2.Distance(_vertices[j], _vertices[i]);
            j = i;
        }

        return perimeter;
    }
}


public class Ball : GameObject
{
    public const float Speed = 300f;

    public Ball(Vector2 position, Vector2 velocity, int radius, int segments, Color color)
        : base(position, velocity, CreateCircle(position, radius, segments), color)
    {

    }
    
    private static Polygon CreateCircle(Vector2 postition, int radius, int segments)
    {
        var vertices = new List<Vector2>();
        var angle = 0f;
        var angleIncrement = MathHelper.TwoPi / segments;
        for (var i = 0; i < segments; i++)
        {
            var x = postition.X + radius * (float)Math.Cos(angle);
            var y = postition.Y + radius * (float)Math.Sin(angle);
            vertices.Add(new Vector2(x, y));
            angle += angleIncrement;
        }
        
        return new Polygon(vertices.ToArray());
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Bounce off top and bottom walls
        if (Position.Y < 0 || Position.Y > 480 - Polygon.Vertices[0].Y)
        {
            Velocity.Y = -Velocity.Y;
        }
    }

    public void Reset()
    {
        Position = new Vector2(400, 240);
        Velocity = new Vector2(-Speed, 0);
    }
}

public class Paddle : GameObject
{
    private const float Speed = 400f;

    public Paddle(Vector2 position, Vector2 velocity, Polygon polygon, Color color)
        : base(position, velocity, polygon, color)
    {
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Move up and down in response to player input
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Up))
        {
            Velocity.Y = -Speed;
        }
        else if (keyboardState.IsKeyDown(Keys.Down))
        {
            Velocity.Y = Speed;
        }
        else
        {
            Velocity.Y = 0;
        }

        // Keep paddle within screen bounds
        if (Position.Y < 0)
        {
            Position.Y = 0;
        }
        else if (Position.Y > 480 - Polygon.Vertices[0].Y)
        {
            Position.Y = 480 - Polygon.Vertices[0].Y;
        }
    }
}

public class DebugSquare : GameObject
{
    private static int Size = 100;

    public DebugSquare()
        : base(Vector2.Zero, Vector2.Zero, CreateSquarePolygon(Size++), Color.Crimson)
    {
    }

    private static Polygon CreateSquarePolygon(int size)
    {
        var halfSize = size / 2;
        var vertices = new List<Vector2>
        {
            new Vector2(-halfSize, -halfSize),
            new Vector2(halfSize, -halfSize),
            new Vector2(halfSize, halfSize),
            new Vector2(-halfSize, halfSize)
        };
        return new Polygon(vertices.ToArray());
    }
}

    public static class PolygonHelper
    {
        public static bool Intersects(Polygon polygon1, Polygon polygon2)
        {
            Vector2[] axes = GetAxes(polygon1).Concat(GetAxes(polygon2)).ToArray();

            foreach (Vector2 axis in axes)
            {
                float min1 = float.MaxValue, max1 = float.MinValue;
                float min2 = float.MaxValue, max2 = float.MinValue;

                foreach (Vector2 vertex in polygon1.Vertices)
                {
                    float projection = Vector2.Dot(vertex, axis);
                    min1 = Math.Min(min1, projection);
                    max1 = Math.Max(max1, projection);
                }

                foreach (Vector2 vertex in polygon2.Vertices)
                {
                    float projection = Vector2.Dot(vertex, axis);
                    min2 = Math.Min(min2, projection);
                    max2 = Math.Max(max2, projection);
                }

                if (max1 < min2 || max2 < min1)
                {
                    return false;
                }
            }

            return true;
        }

        private static Vector2[] GetAxes(Polygon polygon)
        {
            Vector2[] axes = new Vector2[polygon.Vertices.Length];

            for (int i = 0; i < polygon.Vertices.Length; i++)
            {
                Vector2 vertex1 = polygon.Vertices[i];
                Vector2 vertex2 = polygon.Vertices[(i + 1) % polygon.Vertices.Length];
                Vector2 edge = vertex2 - vertex1;
                axes[i] = new Vector2(-edge.Y, edge.X);
                axes[i] = Vector2.Normalize(axes[i]);
            }

            return axes;
        }
    }

    public static class GraphicsDeviceExtensions
    {
        public static void DrawPolygon(this GraphicsDevice graphicsDevice, Polygon polygon, Color color, PrimitiveType primitiveType)
        {
            if (primitiveType == PrimitiveType.LineList)
            {
                var lineList = polygon.ToLineList(color);
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, lineList, 0, lineList.Length / primitiveType.GetVertexCount());
            }
            else
            {
                var vertexArray = polygon.ToVertexPositionColors(color);
                graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexArray, 0, vertexArray.Length / primitiveType.GetVertexCount());
            }
        }

        public static Texture2D CreateWhitePixel(this GraphicsDevice graphicsDevice)
        {
            var texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });
            return texture;
        }
    }

    public static class PolygonExtensions
    {
        public static VertexPositionColor[] ToLineList(this Polygon polygon, Color color)
        {
            var vertexArray = polygon.Vertices.ToArray();
            var lineList = new VertexPositionColor[vertexArray.Length * 2];
            for (var i = 0; i < vertexArray.Length; i++)
            {
                lineList[i * 2] = new VertexPositionColor(new Vector3(vertexArray[i], 0), color);
                lineList[i * 2 + 1] = new VertexPositionColor(new Vector3(vertexArray[(i + 1) % vertexArray.Length], 0), color);
            }
            return lineList;
        }

        public static VertexPositionColor[] ToVertexPositionColors(this Polygon polygon, Color color) => polygon.Vertices.Select(vertex => new VertexPositionColor() { Position = new Vector3(vertex, 0), Color = color }).ToArray();
    }

    public static class PrimitiveTypeExtensions
    {
        public static int GetVertexCount(this PrimitiveType primitiveType)
        {
            return primitiveType switch
            {
                PrimitiveType.LineList => 2,
                PrimitiveType.LineStrip => 2,
                PrimitiveType.TriangleList => 3,
                PrimitiveType.TriangleStrip => 3,
                PrimitiveType.PointList => 3,
                _ => 0
            };
        }
    }
