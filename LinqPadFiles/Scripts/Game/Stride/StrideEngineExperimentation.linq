<Query Kind="Program">
  <NuGetReference>Stride.Audio</NuGetReference>
  <NuGetReference>Stride.Core.Mathematics</NuGetReference>
  <NuGetReference>Stride.Engine</NuGetReference>
  <NuGetReference>Stride.Graphics</NuGetReference>
  <NuGetReference>Stride.Input</NuGetReference>
  <NuGetReference>Stride.Physics</NuGetReference>
  <NuGetReference>Stride.Rendering</NuGetReference>
  <NuGetReference>Stride.UI</NuGetReference>
  <Namespace>Stride.Core.Mathematics</Namespace>
  <Namespace>Stride.Engine</Namespace>
  <Namespace>Stride.Games</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Stride.UI.Controls</Namespace>
  <Namespace>Stride.UI</Namespace>
  <Namespace>Stride.Rendering.Sprites</Namespace>
  <Namespace>Stride.Graphics</Namespace>
  <Namespace>Stride.Rendering</Namespace>
</Query>

void Main()
{
    using (var game = new MyGame())
    {
        game.Run();
    }
}

public class MyGame : Game
{

    //private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private RenderContext renderContext;
    private DrawEffect basicEffect;
    
    protected override void Initialize()
    {
        base.Initialize();
        
        renderContext = RenderContext.GetShared(Services);
        basicEffect = new DrawEffect(;
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        
        spriteBatch.Begin
    }
}