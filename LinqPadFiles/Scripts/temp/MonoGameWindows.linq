<Query Kind="Program">
  <NuGetReference>MonoGame.Framework.WindowsDX</NuGetReference>
  <Namespace>Microsoft.Xna.Framework</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics</Namespace>
</Query>

#load ".\MonoGame_Rendering"
#load ".\MonoGame_Polygons"
#load ".\MonoGame_Textures"
#load ".\MonoGame_GameObject"
#load ".\MonoGame_FluidDynamics"

static void Main(string[] args)
{
	using (var game = new MyGame())
	{
		game.Run();
	}
}

public class MyGame : Game
{
	private readonly GraphicsDeviceManager _graphics;
	private Renderer _renderer;
	private IGameObject _gameObject;
	private IPhysics _physics;

	public MyGame()
	{
		_physics = new Physics(new EnvironmentalFactors
		{
			Gravity = -9.81f,
			Medium = Fluids.FluidName.Hydrogen,
			Wind = Vector2.Zero
		});
		_graphics = new GraphicsDeviceManager(this)
		{
			PreferredBackBufferWidth = 800,
			PreferredBackBufferHeight = 600
		};
	}

	protected override void Initialize()
	{
		_renderer = new(_graphics);
		Window.Title = "My Game";
		IsMouseVisible = true;
		GraphicsDevice.Clear(Color.CornflowerBlue);
		base.Initialize();
	}
	
	public static Vector2 HeadingAndSpeedToVector2(float heading, float speed)
	{
		var headingRadians = heading * Math.PI / 180;
		var x = speed * Math.Cos(headingRadians);
		var y = speed * Math.Sin(headingRadians);
		return new Vector2((float)x, (float)y);
	}

	protected override void LoadContent()
	{
		var position = Vector2.Zero;
		var position2 = new Vector2(0f, _graphics.PreferredBackBufferHeight / 2f);

		_gameObject = new GameObject()
		{
			Name = "ArtilleryProjectile",
			Mass = 1f,
			Velocity = HeadingAndSpeedToVector2(45f, 100f),
			Color = Color.White,
			Position = position2,
			Polygon = PolygonFactory.GetArtilleryShellPolygon(position, 50),
		};
	}

	protected override void Update(GameTime gameTime)
	{
		_physics.Update(_gameObject, gameTime.ElapsedGameTime);
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);
		_renderer.Render(_gameObject);
	}

	protected override void Dispose(bool disposing)
	{
		_renderer.Dispose();
		base.Dispose(disposing);
	}
}