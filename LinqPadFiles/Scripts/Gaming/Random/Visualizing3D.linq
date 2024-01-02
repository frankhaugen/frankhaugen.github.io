<Query Kind="Statements">
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.Gaming.Core\bin\Debug\net7.0\Frank.Libraries.Gaming.Core.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Gaming.Core\bin\Debug\net7.0\Frank.Libraries.Gaming.Core.dll</Reference>
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.Gaming.Core\bin\Debug\net7.0\Frank.Libraries.Gaming.Physics.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Gaming.Core\bin\Debug\net7.0\Frank.Libraries.Gaming.Physics.dll</Reference>
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.Gaming.Rendering\bin\Debug\net7.0\Frank.Libraries.Gaming.Primitives.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Gaming.Rendering\bin\Debug\net7.0\Frank.Libraries.Gaming.Primitives.dll</Reference>
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.Gaming.Rendering\bin\Debug\net7.0\Frank.Libraries.Gaming.Rendering.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Gaming.Rendering\bin\Debug\net7.0\Frank.Libraries.Gaming.Rendering.dll</Reference>
  <Reference Relative="..\..\..\..\..\Frank.Libraries\src\Frank.Libraries.Gaming.Testing\bin\Debug\net7.0\Frank.Libraries.Gaming.Testing.dll">C:\repos\frankhaugen\Frank.Libraries\src\Frank.Libraries.Gaming.Testing\bin\Debug\net7.0\Frank.Libraries.Gaming.Testing.dll</Reference>
  <Namespace>Frank.Libraries.Gaming.Core</Namespace>
  <Namespace>Frank.Libraries.Gaming.Primitives</Namespace>
  <Namespace>Frank.Libraries.Gaming.Rendering.Console</Namespace>
  <Namespace>Frank.Libraries.Gaming.Rendering.Svg</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Frank.Libraries.Gaming.Testing</Namespace>
</Query>

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var box = PolygonFactory.CreateCube(20);
var sphere = PolygonFactory.CreateSphere(10, 32);
var pyramid = PolygonFactory.CreatePyramid(20, 30);

var scene = new Scene("Dispaly");
scene.SceneSize = new Rectangle(0,0, 300, 180);
scene.Shapes.Add(new Shape() {Color = Color.Chartreuse, Polygon = box});
scene.Shapes.Add(new Shape() {Color = Color.Crimson, Polygon = sphere});
scene.Shapes.Add(new Shape() {Color = Color.CornflowerBlue, Polygon = pyramid});



//var renderer = new ConsoleRenderer(160, 4);
var svgOptions = new SvgRendererOptions()
{
    BackgroundColor = Color.White,
    IncludeGridLines = true,
    Style = SvgStyle.LinqPad,
    ViewBox = scene.SceneSize
};
var renderer = new SvgRenderer(svgOptions);

var simulator = new Simulator()
{
    SimulationSpeed = 1,
    TimeIncrement = System.TimeSpan.FromMilliseconds(100)
};

simulator.Run(100, deltaTime =>
{
    foreach (var shape in scene.Shapes)
    {
        shape.Polygon.Translate((Vector3.UnitX * (float)deltaTime.TotalSeconds));
    }
    
    Util.ClearResults();
    renderer.Render(scene, result => new Svg(result, scene.SceneSize.Width, scene.SceneSize.Height).Dump(deltaTime.ToString()));
    //renderer.Render(scene, result => result.Dump("FFF"));
});


