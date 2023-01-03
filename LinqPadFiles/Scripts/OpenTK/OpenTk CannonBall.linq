<Query Kind="Statements">
  <NuGetReference>OpenTK</NuGetReference>
  <Namespace>OpenTK</Namespace>
  <Namespace>OpenTK.Audio.OpenAL</Namespace>
  <Namespace>OpenTK.Core</Namespace>
  <Namespace>OpenTK.Graphics</Namespace>
  <Namespace>OpenTK.Graphics.GL</Namespace>
  <Namespace>OpenTK.Input.Hid</Namespace>
  <Namespace>OpenTK.Mathematics</Namespace>
  <Namespace>OpenTK.Platform.Windows</Namespace>
  <Namespace>OpenTK.Windowing.Common</Namespace>
  <Namespace>OpenTK.Windowing.Common.Input</Namespace>
  <Namespace>OpenTK.Windowing.Desktop</Namespace>
  <Namespace>OpenTK.Windowing.GraphicsLibraryFramework</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>


using (var game = new Game(800, 600, "MyGame"))
{
    game.Run();
}










public class Game : GameWindow
{
    public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }

    


    protected override void Dispose(bool disposing)
    {
        Close(); 
        base.Dispose(disposing);
    }
}