<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Drawing</Namespace>
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>






























public class GameObject
{
    public Rigidbody Rigidbody { get; set; }
    public Transform Transform { get; set; }






}

public class Rigidbody
{
    public float Mass { get; set; }
    public float LinearVelocity { get; set; }
}

public class Transform
{
    public Vector2 Position { get; set; }
    public float Direction { get; set; }
}

public static class MathConstants
{
    public static Single Deg2Rad = 0.01745329f;
}

public static class Maths
{
    public static Vector2 AngleToVector2(float angle) => new Vector2(MathF.Cos(angle * MathConstants.Deg2Rad), MathF.Sin(angle * MathConstants.Deg2Rad));
}

public static class EulerFunctions
{
    public static T ExplicitEulerPosition<T>(T velocity, T ΔTime) where T : INumber<T>
    {
        return velocity * ΔTime;
    }


    public static T SymplecticEulerPosition<T>(T mass, T force, T ΔTime) where T : INumber<T>
    {
        var velocity = ExplicitEulerVelocity<T>(mass, force, ΔTime);
        return velocity * ΔTime;
    }
    
    public static T ExplicitEulerVelocity<T>(T mass, T force, T ΔTime) where T : INumber<T>
    {
        return NewtonianFunctions.SecondLawAccelleration(mass, force) * ΔTime;
    }
}

public static class NewtonianFunctions
{
    public static T SecondLawForce<T>(T mass, T accelleration) where T : INumber<T>
    {
        return mass * accelleration;
    }
    
    public static T SecondLawAccelleration<T>(T mass, T force) where T : INumber<T>
    {
        return (T.One / mass) * force;
    }
}
