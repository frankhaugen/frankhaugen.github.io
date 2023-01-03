<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Options.ConfigurationExtensions</NuGetReference>
  <NuGetReference>OpenTK</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>OpenTK.Graphics.OpenGL</Namespace>
  <Namespace>OpenTK.Mathematics</Namespace>
  <Namespace>OpenTK.Windowing.Desktop</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
    // Set up the game window
    var window = new OpenTK.Windowing.Desktop.GameWindow(new(), new() {
     Size = new(800,600),
     Title = "Projectile Physics"
    });

    // Set up the drawer
    var drawer = new OpenTkDrawer(window.Size.X, window.Size.Y);

    // Set up the physics calculator
    var physicsCalculator = new ProjectilePhysicsCalculator(-9.8f);

    // Set up the game
    var game = new Game(physicsCalculator, drawer, 0.1f);
    game.Projectiles.Add(new Projectile(Vector2.Zero, new Vector2(30f, 0f), 1f, 0.2f));

    // Set up the game loop
    window.UpdateFrame += (sender) => game.Update();
    window.RenderFrame += (sender) => game.Draw();
    window.Resize += (sender) => GL.Viewport(0, 0, window.Size.X, window.Size.Y);
    window.Closing += (sender) => window.Close();

    // Run the game loop
    window.Run();
}








public class Game
{
    private readonly IPhysicsCalculator physicsCalculator;
    private readonly IDrawer drawer;
    private readonly float dt;

    public IList<Projectile> Projectiles { get; }

    public Game(IPhysicsCalculator physicsCalculator, IDrawer drawer, float dt)
    {
        this.physicsCalculator = physicsCalculator;
        this.drawer = drawer;
        this.dt = dt;
        Projectiles = new List<Projectile>();
    }

    public void Update()
    {
        foreach (var projectile in Projectiles)
        {
            // Calculate the acceleration of the projectile
            var acceleration = physicsCalculator.CalculateAcceleration(
                projectile.Position,
                projectile.Velocity,
                projectile.Mass,
                projectile.DragCoefficient
            );

            // Update the position and velocity of the projectile
            projectile.Position += projectile.Velocity * dt;
            projectile.Velocity += acceleration * dt;
        }
    }

    public void Draw()
    {
        // Clear the screen
        GL.Clear(ClearBufferMask.ColorBufferBit);

        foreach (var projectile in Projectiles)
        {
            // Draw the projectile
            drawer.Draw(projectile.Position, projectile.Color, projectile.Size);
        }
    }
}

public interface IDrawer
{
    void Draw(Vector2 position, Color4 color, Vector2 size);
}

public class OpenTkDrawer : IDrawer
{
    private readonly int windowWidth;
    private readonly int windowHeight;

    public OpenTkDrawer(int windowWidth, int windowHeight)
    {
        this.windowWidth = windowWidth;
        this.windowHeight = windowHeight;
    }

    public void Draw(Vector2 position, Color4 color, Vector2 size)
    {
        // Set the color
        GL.Color4(color);

        // Draw a rectangle at the given position and with the given size
        GL.Begin(PrimitiveType.Quads);
        GL.Vertex2(position.X, position.Y);
        GL.Vertex2(position.X + size.X, position.Y);
        GL.Vertex2(position.X + size.X, position.Y + size.Y);
        GL.Vertex2(position.X, position.Y + size.Y);
        GL.End();
    }
}

public interface IPhysicsCalculator
{
    Vector2 CalculateAcceleration(Vector2 position, Vector2 velocity, float mass, float dragCoefficient);
}

public class ProjectilePhysicsCalculator : IPhysicsCalculator
{
    private readonly float gravity;

    public ProjectilePhysicsCalculator(float gravity)
    {
        this.gravity = gravity;
    }

    public Vector2 CalculateAcceleration(Vector2 position, Vector2 velocity, float mass, float dragCoefficient)
    {
        // Calculate the acceleration due to gravity
        var acceleration = new Vector2(0f, gravity);

        // Calculate the acceleration due to air resistance
        var speed = velocity.Length;
        var drag = dragCoefficient * speed * speed;
        var dragForce = velocity * drag / mass;
        acceleration -= dragForce;

        return acceleration;
    }
}

public class Projectile
{
    public Color4 Color { get; set; } = Color4.Crimson;
    public Vector2 Size { get; set; } = new(5f, 5f);
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Mass { get; }
    public float DragCoefficient { get; }

    public Projectile(Vector2 position, Vector2 velocity, float mass, float dragCoefficient)
    {
        Position = position;
        Velocity = velocity;
        Mass = mass;
        DragCoefficient = dragCoefficient;
    }
}

