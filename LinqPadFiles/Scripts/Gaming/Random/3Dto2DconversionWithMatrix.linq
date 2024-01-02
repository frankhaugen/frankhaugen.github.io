<Query Kind="Statements">
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.Gaming.Core\bin\Debug\net7.0\Frank.Libraries.Gaming.Core.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Gaming.Core\bin\Debug\net7.0\Frank.Libraries.Gaming.Core.dll</Reference>
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.Gaming.Core\bin\Debug\net7.0\Frank.Libraries.Gaming.Physics.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Gaming.Core\bin\Debug\net7.0\Frank.Libraries.Gaming.Physics.dll</Reference>
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.Gaming.Rendering\bin\Debug\net7.0\Frank.Libraries.Gaming.Primitives.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Gaming.Rendering\bin\Debug\net7.0\Frank.Libraries.Gaming.Primitives.dll</Reference>
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.Gaming.Rendering\bin\Debug\net7.0\Frank.Libraries.Gaming.Rendering.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Gaming.Rendering\bin\Debug\net7.0\Frank.Libraries.Gaming.Rendering.dll</Reference>
  <Namespace>Frank.Libraries.Gaming.Primitives</Namespace>
  <Namespace>Frank.Libraries.Gaming.Rendering.Svg</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Frank.Libraries.Gaming.Core</Namespace>
  <Namespace>Frank.Libraries.Gaming.Rendering.Console</Namespace>
</Query>

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

// Define a simple Cone shape with 5 base points and 1 apex
List<Vector3> cone3D = new List<Vector3>
        {
            new Vector3(-5, 0, 0), // Base point 1
            new Vector3(5, 5, 0), // Base point 2
            new Vector3(10, 0, 0), // Base point 3
            new Vector3(0, -10, 0), // Base point 4
            new Vector3(10, -10, 0), // Base point 5
            new Vector3(0, 0, 10) // Apex
        };

// Create an orthographic projection matrix
Matrix4x4 projectionMatrix = Matrix4x4.Identity;

// Project each 3D point to 2D using the projection matrix
List<Vector3> cone2D = new List<Vector3>();

foreach (var point3D in cone3D)
{
    Vector3 projected3D = Vector3.Transform(point3D, projectionMatrix);

    // After the transformation, eliminate the z-coordinate to get the 2D coordinates
    cone2D.Add(new Vector3(projected3D.X, projected3D.Y, 0));
}

Console.WriteLine("3D Cone: " + string.Join(", ", cone3D));
Console.WriteLine("2D Projection: " + string.Join(", ", cone2D));

var scene = new Scene("Dispaly");
scene.SceneSize = new Rectangle(-10,-10, 100, 100);
scene.Shapes.Add(new Shape()
{
    Color = Color.CadetBlue,
    Polygon = new Polygon(cone2D.ToArray()).Translate(new Vector3(30,20, 0))
});

var renderer = new ConsoleRenderer(160, 4);
renderer.Render(scene, result => result.Dump("FFF"));


//var rendere = new SvgRenderer(new SvgRendererOptions() {Style = SvgStyle.LinqPad});
//rendere.Render(scene, result => new Svg(result, scene.SceneSize.Width, scene.SceneSize.Height).Dump("FFF"));