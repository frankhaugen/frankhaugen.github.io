<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

var s = 8f;

var taskA = TaskA_InitialVelocity(new Vector2(12,12), 45);
taskA.Dump();

var taskB = TaskB_TimeToMaxHeight(taskA);
taskB.Dump();

var taskC = TaskC_MaxHeight(taskA, taskB);
taskC.Dump();

var taskDVelocity = TaskD_EndVelocity(taskA);
var taskDDirection = TaskD_EndDirection(taskA);
taskDVelocity.Dump();
taskDDirection.Dump();

var taskE = TaskE_TotalTime(taskA);
taskE.Dump();

var taskF = TaskF_TotalDistance(taskA, taskE);
taskF.Dump();


float TaskF_TotalDistance(Vector2 initialVelocity, TimeSpan time) =>MathF.Sqrt(MathF.Pow(Convert.ToSingle(initialVelocity.X * time.TotalSeconds), 2) + MathF.Pow(s, 2));

TimeSpan TaskE_TotalTime(Vector2 initialVelocity, float gravity = -9.81f) =>  TimeSpan.FromSeconds((CalculateVelocityOfY(initialVelocity)-initialVelocity.X) / gravity);

float TaskD_EndDirection(Vector2 initialVelocity, float gravity = -9.81f) => MathF.Tan((CalculateVelocityOfY(initialVelocity)/initialVelocity.X)).ToRadian();

float TaskD_EndVelocity(Vector2 initialVelocity, float gravity = -9.81f) => MathF.Sqrt(MathF.Pow(initialVelocity.X ,2) + MathF.Pow(CalculateVelocityOfY(initialVelocity) ,2));

float TaskC_MaxHeight(Vector2 initialVelocity, TimeSpan time, float gravity = -9.81f) => Convert.ToSingle(initialVelocity.Y * time.TotalSeconds + 0.5f * gravity * MathF.Pow(Convert.ToSingle(time.TotalSeconds), 2));

TimeSpan TaskB_TimeToMaxHeight(Vector2 initialVelocity, float gravity = -9.81f) => TimeSpan.FromSeconds((initialVelocity.Y * -1f) / gravity);

Vector2 TaskA_InitialVelocity(Vector2 velocity, float angle) => new Vector2(velocity.X * MathF.Cos(angle.ToRadian()), velocity.Y * MathF.Sin(angle.ToRadian()));

float CalculateVelocityOfY(Vector2 initialVelocity, float gravity = -9.81f) => MathF.Sqrt(MathF.Pow(initialVelocity.Y, 2) + (2 * gravity * (s * -1))) * -1;