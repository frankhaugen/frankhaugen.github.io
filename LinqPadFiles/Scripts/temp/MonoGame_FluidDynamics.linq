<Query Kind="Statements">
  <NuGetReference>MonoGame.Framework.WindowsDX</NuGetReference>
  <Namespace>Microsoft.Xna.Framework</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics</Namespace>
</Query>

#load "MonoGame_Polygons"
#load "MonoGame_GameObject"


public interface IPhysics
{
	void Update(IGameObject gameObject, TimeSpan elapsed);
}
public class Physics : IPhysics
{
	private readonly EnvironmentalFactors _environment;

	public Physics(EnvironmentalFactors environment)
	{
		_environment = environment;
	}

	public void Update(IGameObject gameObject, TimeSpan elapsed)
	{
		// Calculate the acceleration due to gravity
		var gravityAcceleration = new Vector2(0, _environment.Gravity);

		// Calculate the air resistance force
		var velocity = gameObject.Velocity;
		var density = Fluids.GetDensity(_environment.Medium);
		var surfaceArea = gameObject.Polygon.Area();
		var aerodynamics = Aerodynamics.Calculate(gameObject.Polygon, _environment.Medium, velocity);
		var dragCoefficient = aerodynamics.CoefficientOfDrag;
		var airResistance = 0.5f * density * velocity.LengthSquared() * surfaceArea * dragCoefficient;
		var airResistanceVector = new Vector2(-velocity.X * airResistance, -velocity.Y * airResistance);

		// Calculate the total force acting on the object
		var force = airResistanceVector + gravityAcceleration * gameObject.Mass;

		// Calculate the acceleration of the object
		var acceleration = force / gameObject.Mass;

		// Update the velocity of the object based on the acceleration
		if (!float.IsNaN(acceleration.X) && !float.IsNaN(acceleration.Y))
		{
			gameObject.Velocity += acceleration * (float)elapsed.TotalSeconds;
		}

		// Update the position of the object based on the velocity
		if (!float.IsNaN(gameObject.Velocity.X) && !float.IsNaN(gameObject.Velocity.Y))
		{
			gameObject.Position += gameObject.Velocity * (float)elapsed.TotalSeconds;
		}
		
	 	var optimalDirection = gameObject.Polygon.OptimalFlowDirection();
		var something = Vector2ToHeadingAndSpeed(optimalDirection);
		gameObject.Polygon = gameObject.Polygon.RotateToDirection(something.heading);
	}

	public static (float heading, float speed) Vector2ToHeadingAndSpeed(Vector2 vector)
	{
		var headingRadians = Math.Atan2(vector.Y, vector.X);
		var heading = (float)(headingRadians * 180 / Math.PI);
		var speed = vector.Length();
		return (heading, speed);
	}


}

public struct DragCoefficientCalculator
{
	public float Fd { get; set; } // the drag force
	public float Rho { get; set; } // the density of the fluid
	public float V { get; set; } // the velocity of the object
	public float A { get; set; } // the surface area of the object

	public float CalculateCd()
	{
		return Fd / (0.5f * Rho * V * V * A);
	}
}
public static class EnvironmentalFactorsExtensions
{
	public static float GetAirResistance(this EnvironmentalFactors factors, float coefficientOfDrag, float velocity, float size)
	{
		// Calculate the air resistance experienced by an object using the following formula:
		// airResistance = 0.5 * coefficientOfDrag * airDensity * velocity^2 * size
		return 0.5f * coefficientOfDrag * Fluids.GetDensity(factors.Medium) * velocity * velocity * size;
	}
}
public struct EnvironmentalFactors
{
	public float Gravity { get; set; } = 9.81f;
	public Fluids.FluidName Medium { get; set; }
	public Vector2 Wind { get; set; }

	public EnvironmentalFactors()
	{

	}
}



public struct Aerodynamics
{
	public float SurfaceArea { get; set; }
	public float CoefficientOfDrag { get; set; }
	public float CoefficientOfLift { get; set; }

	public static Aerodynamics Calculate(Polygon polygon, Fluids.FluidName fluid, Vector2 velocity)
	{
		// Calculate the surface area of the polygon
		var surfaceArea = polygon.Area();

		// Get the density of the fluid
		var density = Fluids.GetDensity(fluid);

		// Calculate the velocity magnitude (speed)
		var velocityMagnitude = velocity.Length();

		// Calculate the angle of attack of the velocity relative to the optimal flow direction of the polygon
		var optimalFlowDirection = polygon.OptimalFlowDirection();
		var angleOfAttack = MathHelper.ToDegrees((float)Math.Acos(Vector2.Dot(velocity, optimalFlowDirection) / velocityMagnitude));

		// Calculate the coefficient of drag based on the surface area, fluid density, and velocity magnitude
		var coefficientOfDrag = 0.5f * density * velocityMagnitude * velocityMagnitude * surfaceArea;

		// Calculate the coefficient of lift based on the coefficient of drag and the angle of attack
		var coefficientOfLift = coefficientOfDrag * (float)Math.Sin(angleOfAttack);

		return new Aerodynamics
		{
			SurfaceArea = surfaceArea,
			CoefficientOfDrag = coefficientOfDrag,
			CoefficientOfLift = coefficientOfLift
		};
	}
}

public static class Fluids
{
	public enum FluidName
	{
		Air,
		Water,
		CarbonDioxide,
		Helium,
		Hydrogen,
		Nitrogen,
		Oxygen,
		Propane,
		Steam,
		Vacuum
	}

	private static readonly Dictionary<FluidName, float> Densities = new Dictionary<FluidName, float>
	{
		{ FluidName.Air, 1.225f },
		{ FluidName.Water, 1000f },
		{ FluidName.CarbonDioxide, 1.977f },
		{ FluidName.Helium, 0.178f },
		{ FluidName.Hydrogen, 0.08988f },
		{ FluidName.Nitrogen, 1.251f },
		{ FluidName.Oxygen, 1.429f },
		{ FluidName.Propane, 1.879f },
		{ FluidName.Steam, 0.5961f },
		{ FluidName.Vacuum, 0f }
	};

	public static float GetDensity(FluidName fluid) => Densities[fluid];
}