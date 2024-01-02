<Query Kind="Statements">
  <NuGetReference>MonoGame.Framework.WindowsDX</NuGetReference>
  <Namespace>Microsoft.Xna.Framework</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics</Namespace>
</Query>

#load "MonoGame_Polygons"
#load "MonoGame_Rendering"
#load "MonoGame_Input"
#load "MonoGame_FluidDynamics"



public interface IControllable
{
	void MoveUp();
	void MoveDown();
	void MoveLeft();
	void MoveRight();
}

// Contains all properties
public interface IGameObjectBase
{
	public string Name { get; set; }
	public bool PhysicsEnebled { get; set; }
	public bool CollissionEnabled { get; set; }

	public float Mass { get; set; }
	public Vector2 Position { get; set; }
	public Vector2 Velocity { get; set; }
	public Polygon Polygon { get; set; }
	public Color Color { get; set; }
}

// Contains all methods
public interface IGameObject : IGameObjectBase, IControllable
{
}
public class GameObjects
{
    private Dictionary<string, IGameObject> _gameObjects;
    private IPhysics _physics;

    public GameObjects(IPhysics physics)
    {
        _gameObjects = new Dictionary<string, IGameObject>();
        _physics = physics;
    }

    public void Add(IGameObject gameObject)
    {
        _gameObjects.Add(gameObject.Name, gameObject);
    }

    public void Remove(string name)
	{
		_gameObjects.Remove(name);
	}

	public void Update(GameTime gameTime)
	{
		var collissions = new List<Collission>();

		// Update all game objects
		foreach (var gameObject in _gameObjects.Values)
		{
			_physics.Update(gameObject, gameTime.ElapsedGameTime);

			// Check for collissions with other game objects
			foreach (var other in _gameObjects.Values)
			{
				if (gameObject != other && gameObject.Polygon.Intersects(other.Polygon))
				{
					collissions.Add(new Collission
					{
						GameObject1 = gameObject.Name,
						GameObject2 = other.Name,
						// Calculate the force of the collission here
					});
				}
			}
		}

		// Handle collissions
		foreach (var collission in collissions)
		{
			// Get the colliding game objects
			var gameObject1 = _gameObjects[collission.GameObject1];
			var gameObject2 = _gameObjects[collission.GameObject2];

			// Update the velocities of the colliding game objects based on the collission force
			// ...
		}
	}
}

public struct Collission
{
	public string GameObject1 { get; set; }
	public string GameObject2 { get; set; }
	public Vector2 Force { get; set; }
}


public class GameObject : IGameObject
{
	public string Name { get; set; }
	public bool PhysicsEnebled { get; set; }
	public bool CollissionEnabled { get; set; }
	public float Mass { get; set; }
	public Vector2 Position { get; set; }
	public Vector2 Velocity { get; set; }
	public Polygon Polygon { get; set; }
	public Color Color { get; set; }

	public void MoveDown()
	{
	}

	public void MoveLeft()
	{
	}

	public void MoveRight()
	{
	}

	public void MoveUp()
	{
	}
}