<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Options.ConfigurationExtensions</NuGetReference>
  <NuGetReference>OpenTK</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>OpenTK.Graphics.OpenGL</Namespace>
  <Namespace>OpenTK.Mathematics</Namespace>
  <Namespace>OpenTK.Windowing.Desktop</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>System.Windows.Media</Namespace>
  <Namespace>System.Windows.Shapes</Namespace>
  <Namespace>OpenTK.Windowing.Common</Namespace>
</Query>

void Main()
{
    // Set up service collection
    var services = new ServiceCollection();
    services.AddTransient<IPhysicsCalculator, ProjectilePhysicsCalculator>();
    services.AddTransient<IDrawer, OpenTkDrawer>();
    services.AddTransient<Projectile>();
    services.AddSingleton<ProjectileOptions>(new ProjectileOptions()
    {
        DragCoefficient = 0.2f,
        Gravity = -9.82f,
        InitialPosition = System.Numerics.Vector2.Zero,
        Mass = 1f,
        TimeStep= 0.1f,
        InitialVelocity = System.Numerics.Vector2.Zero,
        Steps = 100,
        Width = 800,
        Height = 600,
        
        Color = Color4.Azure
    });

    // Set up service provider
    var provider = services.BuildServiceProvider();

    // Get the projectile options from the service provider
    var options = provider.GetRequiredService<ProjectileOptions>();

    // Get the projectile instance from the service provider
    var projectile = provider.GetRequiredService<Projectile>();

    // Get the drawer instance from the service provider
    var drawer = provider.GetRequiredService<IDrawer>();


    // Calculate the positions of the projectile
    var positions = projectile.GetPositions(options.TimeStep, options.Steps);

    // Draw the projectile at each position
    drawer.Draw(positions);
}

public class ProjectileOptions
{
    public System.Numerics.Vector2 InitialPosition { get; set; }
    public System.Numerics.Vector2 InitialVelocity { get; set; }
    public float TimeStep { get; set; }
    public int Steps { get; set; }
    public float Gravity { get; set; }
    public float DragCoefficient { get; set; }
    public float Mass { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Color4 Color { get; set; }
}
public interface IPhysicsCalculator
{
    System.Numerics.Vector2 CalculateVelocity(System.Numerics.Vector2 velocity, float dt);
    System.Numerics.Vector2 CalculatePosition(System.Numerics.Vector2 position, System.Numerics.Vector2 velocity, float dt);
}

public class ProjectilePhysicsCalculator : IPhysicsCalculator
{
    private readonly float g;
    private readonly float b;
    private readonly float m;

    public ProjectilePhysicsCalculator(ProjectileOptions options)
    {
        this.g = options.Gravity;
        this.b = options.DragCoefficient;
        this.m = options.Mass;
    }

    public System.Numerics.Vector2 CalculateVelocity(System.Numerics.Vector2 velocity, float dt)
    {
        velocity.X -= b / m * velocity.X * dt;
        velocity.Y -= g * dt - b / m * velocity.Y * dt;
        return velocity;
    }

    public System.Numerics.Vector2 CalculatePosition(System.Numerics.Vector2 position, System.Numerics.Vector2 velocity, float dt)
    {
        return position + velocity * dt + 0.5f * System.Numerics.Vector2.UnitY * g * dt * dt;
    }
}

public class Projectile
{
    private readonly IPhysicsCalculator physicsCalculator;
    public System.Numerics.Vector2 Position { get; private set; }
    public System.Numerics.Vector2 Velocity { get; private set; }

    public Projectile(IPhysicsCalculator physicsCalculator, ProjectileOptions options)
    {
        this.physicsCalculator = physicsCalculator;
        this.Position = options.InitialPosition;
        this.Velocity = options.InitialVelocity;
    }

    public void Update(float dt)
    {
        Velocity = physicsCalculator.CalculateVelocity(Velocity, dt);
        Position = physicsCalculator.CalculatePosition(Position, Velocity, dt);
    }

    public IEnumerable<System.Numerics.Vector2> GetPositions(float dt, int steps)
    {
        var positions = new List<System.Numerics.Vector2>();
        for (int i = 0; i < steps; i++)
        {
            Update(dt);
            positions.Add(Position);

            // Check if the projectile has hit the ground
            if (Position.Y < 0)
            {
                break;
            }
        }
        return positions;
    }
}

public class Game : GameWindow
{
    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {

    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        
        
        
        base.OnRenderFrame(args);
    }
}

public interface IDrawer
{
    void Draw(IEnumerable<System.Numerics.Vector2> positions);
}


public class OpenTkDrawer : IDrawer
{
    private readonly int width;
    private readonly int height;
    private readonly Color4 color;

    public OpenTkDrawer(ProjectileOptions options)
    {
        this.width = options.Width;
        this.height = options.Height;
        this.color = options.Color;
    }

    public void Draw(IEnumerable<System.Numerics.Vector2> positions)
    {
        using (var window = new OpenTK.Windowing.Desktop.GameWindow(new GameWindowSettings() { }, new NativeWindowSettings()))
        {
            window.Load += () =>
            {
                GL.ClearColor(Color4.White);
            };

            window.RenderFrame += (TimeSpan) =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.Begin(PrimitiveType.Points);
                GL.Color4(color);

                foreach (var pos in positions.Select(x => new OpenTK.Mathematics.Vector2(x.X, x.Y)))
                {
                    GL.Vertex2(pos);
                }

                GL.End();

                window.SwapBuffers();
            };

            window.Run();
        }
    }
}

public class ConsoleDrawer : IDrawer
{
    public void Draw(IEnumerable<System.Numerics.Vector2> positions)
    {
        // Clear the console
        Console.Clear();

        // Draw the projectile at each position
        foreach (var pos in positions)
        {
            Console.SetCursorPosition((int)pos.X, (int)pos.Y);
            Console.Write("*");
        }
    }
}
public class WpfDrawer : IDrawer
{
    private readonly Canvas canvas;

    public WpfDrawer(Canvas canvas)
    {
        this.canvas = canvas;
    }

    public void Draw(IEnumerable<System.Numerics.Vector2> positions)
    {
        // Clear the canvas
        canvas.Children.Clear();

        // Draw the projectile at each position
        foreach (var pos in positions)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(ellipse, pos.X - 5);
            Canvas.SetTop(ellipse, pos.Y - 5);
            canvas.Children.Add(ellipse);
        }
    }
}
public class SvgDrawer : IDrawer
{
    private readonly int width;
    private readonly int height;
    private readonly FileInfo _file;

    public SvgDrawer(int width, int height, FileInfo file)
    {
        this.width = width;
        this.height = height;
        this._file = file;
    }

    public void Draw(IEnumerable<System.Numerics.Vector2> positions)
    {
        using (StreamWriter file = new StreamWriter(_file.FullName))
        {
            file.WriteLine("<svg width='" + width + "' height='" + height + "'>");
            foreach (var pos in positions)
            {
                file.WriteLine("  <circle cx='" + pos.X + "' cy='" + pos.Y + "' r='5' fill='red' />");
            }
            file.WriteLine("</svg>");
        }
    }
}