<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

double targetDistance = 100; // Target distance in meters
float cannonBallMass = 5; // Cannon ball mass in kg
double launchAngleDegrees = 45; // Launch angle in degrees
float gravityStrength = 9.81f; // Gravity strength in m/s²

double gramsOfGunpowder = CalculateRequiredGunpowder(targetDistance, cannonBallMass, launchAngleDegrees, gravityStrength);

Console.WriteLine($"Grams of gunpowder required = {gramsOfGunpowder}");

const float GunpowderEnergyPerGramInJoules = 3000;

static double CalculateRequiredGunpowder(double targetDistance, float cannonBallMass, double launchAngleDegrees, float gravityStrength)
{
    double launchAngleRadians = Math.PI / 180.0 * launchAngleDegrees;

    // Calculating the required initial speed using kinematic equation:
    double requiredInitialSpeed = Math.Sqrt(targetDistance * gravityStrength / Math.Sin(2 * launchAngleRadians));

    // Energy needed to reach required initial speed
    double requiredEnergy = 0.5 * cannonBallMass * requiredInitialSpeed * requiredInitialSpeed;

    requiredEnergy.Dump("Required Energy");

    // Final conversion: energy to grams of gunpowder needed:
    double gramsOfGunpowder = requiredEnergy / GunpowderEnergyPerGramInJoules;

    return gramsOfGunpowder;
}