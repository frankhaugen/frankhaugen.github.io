<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
  <NuGetReference>MonoGame.Extended.Collisions</NuGetReference>
  <NuGetReference>MonoGame.Extended.Entities</NuGetReference>
  <NuGetReference>MonoGame.Extended.Graphics</NuGetReference>
  <NuGetReference>MonoGame.Extended.Input</NuGetReference>
  <NuGetReference>MonoGame.Extended.Tiled</NuGetReference>
  <NuGetReference>MonoGame.Extensions.Hosting</NuGetReference>
  <NuGetReference>MonoGame.Framework.WindowsDX</NuGetReference>
  <NuGetReference>MonoGame.Primitives2D</NuGetReference>
  <NuGetReference>MonoGame.Shapes</NuGetReference>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Primitives</Namespace>
  <Namespace>Microsoft.Xna.Framework</Namespace>
  <Namespace>Microsoft.Xna.Framework.Audio</Namespace>
  <Namespace>Microsoft.Xna.Framework.Content</Namespace>
  <Namespace>Microsoft.Xna.Framework.Design</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics.PackedVector</Namespace>
  <Namespace>Microsoft.Xna.Framework.Input</Namespace>
  <Namespace>Microsoft.Xna.Framework.Media</Namespace>
  <Namespace>MonoGame</Namespace>
  <Namespace>MonoGame.Framework.Utilities</Namespace>
  <Namespace>MonoGame.Framework.Utilities.Deflate</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <IncludeWinSDK>true</IncludeWinSDK>
  <CopyLocal>true</CopyLocal>
</Query>

void Main()
{
    var builder = Host.CreateApplicationBuilder();

        
    var app = builder.Build();
    app.Run();
}





