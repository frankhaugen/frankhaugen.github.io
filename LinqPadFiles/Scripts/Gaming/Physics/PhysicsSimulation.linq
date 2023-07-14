<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>


void Main()
{
    RunSimulation();

    particles.Chart(x => x.position.X, x => x.position.Y, LINQPad.Util.SeriesType.Line).Dump();
}

// Just applies Earth's gravity force (mass times gravity acceleration 9.81 m/s^2) to each particle.
Vector2 ComputeForce(float mass)
{
    return new Vector2(0, mass * -9.81f);
}

void RunSimulation()
{
    float totalSimulationTime = 5.5f; // The simulation will run for 10 seconds.
    float currentTime = 0; // This accumulates the time that has passed.
    float dt = 0.001f; // Each step will take one second.

    while (currentTime < totalSimulationTime)
    {
        // We're sleeping here to keep things simple. In real applications you'd use some
        // timing API to get the current time in milliseconds and compute dt in the beginning 
        // of every iteration like this:
        // currentTime = GetTime()
        // dt = currentTime - previousTime
        // previousTime = currentTime
        //Thread.Sleep((int)dt);

        _particle.velocity = PhysicsHelper.AddSimulationForces(_particle.velocity, _particle.mass, dt);
            var newPosition = PhysicsHelper.GetNextPosition(_particle.position, _particle.velocity, dt);
            
            var direction = Vector2.Normalize(_particle.velocity);
            _particle.direction = direction;
            _particle.position = newPosition;
            
            //Vector2 force = ComputeForce(particle.mass);
            //Vector2 acceleration = new Vector2(force.X / particle.mass, force.Y / particle.mass);
            //particle.velocity.X += acceleration.X * dt;
            //particle.velocity.Y += acceleration.Y * dt;
            //particle.position.X += particle.velocity.X * dt;
            //particle.position.Y += particle.velocity.Y * dt;
            _particle.age += dt;
            _particle.angle = MathHelper.CalcAngleFromDirection(_particle.direction);
            _particle.ToString().Dump();


        
        if (_particle.position.Y > 0)
        {
            particles.Add(_particle);
        }

        //PrintParticles();
        currentTime += dt;
    }
}

// Two dimensional particle.
public record Particle
{
    public Vector2 velocity;
    public Vector2 position;
    public Vector2 direction;
    public float mass;
    public float age;
    public float angle;
}

// Global array of particles.
public List<Particle> particles = new List<UserQuery.Particle>();
public Particle _particle = new() {
     position = new Vector2(0, 8f),
     velocity = Vector2.Zero,
     direction = CalcDirection(45),
};

public static Vector2 CalcDirection(float angle) => new Vector2(MathF.Cos(angle* Deg2Rad), MathF.Sin(angle* Deg2Rad));

const float Deg2Rad = 0.01745329f;

internal static class MathConstants
{
    public const float Rad2Deg = 180f / MathF.PI;
    public const float Deg2Rad = MathF.PI / 180f;
}

internal static class PhysicsConstants
{
    public const float G = -9.81f;
}

internal static class PhysicsHelper
{
    public static Vector2 GetGravityVector(float mass) => new(0f, mass * PhysicsConstants.G);

    public static Vector2 GetNextPosition(Vector2 position, Vector2 velocity, float ΔTime)
    {
        var nextPosition = position.Copy();
        nextPosition.X += velocity.X * ΔTime;
        nextPosition.Y += velocity.Y * ΔTime;
        return nextPosition;
    }

    public static Vector2 GetNextPosition(Vector2 source, float mass, float ΔTime) => GetNextPosition(source, AddSimulationForces(source, mass, ΔTime), ΔTime);

    public static Vector2 AddSimulationForces(Vector2 source, float mass, float ΔTime)
    {
        var velocity = source.Copy();
        var gravity = GetGravityVector(mass);
        var acceleration = new Vector2(gravity.X / mass, gravity.Y / mass);
        velocity.X += acceleration.X * ΔTime;
        velocity.Y += acceleration.Y * ΔTime;

        return velocity;
    }
}

internal static class MathHelper
{
    public static float CalcAngleFromDirection(Vector2 vector2)
    {
        if (vector2.X < 0)
        {
            return 360 - (MathF.Atan2(vector2.X, vector2.Y) * MathConstants.Rad2Deg * -1);
        }
        else
        {
            return MathF.Atan2(vector2.X, vector2.Y) * MathConstants.Rad2Deg;
        }

        var rads = 360 - (MathF.Atan2(vector2.X, vector2.Y) * MathConstants.Rad2Deg * MathF.Sign(vector2.Y));
        return rads;
        return ConvertRadiansToDegrees(rads);
    }
    
    public static Vector2 CalcDirectionFromAngle(float angle) => new(MathF.Cos(ConvertDegreesToRadians(angle)), MathF.Sin(ConvertDegreesToRadians(angle)));

    public static float ConvertRadiansToDegrees(float radians) => radians * MathConstants.Rad2Deg;

    public static float ConvertDegreesToRadians(float degrees) => degrees * MathConstants.Deg2Rad;
}