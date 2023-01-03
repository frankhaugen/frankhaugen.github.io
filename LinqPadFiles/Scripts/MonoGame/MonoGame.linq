<Query Kind="Statements">
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
  <IncludeWinSDK>true</IncludeWinSDK>
  <CopyLocal>true</CopyLocal>
</Query>


using(var game = new MyGame())
{
    game.Run();
}





class MyGame : Game
{
    
    private readonly GraphicsDeviceManager _graphicsManager;
    private SpriteBatch _spriteBatch;
    
    public MyGame()
    {
        _graphicsManager = new GraphicsDeviceManager(this);   
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {

        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        
        
        
        

        _spriteBatch.End();



        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        EndRun();
        base.Dispose(disposing);
    }

}






