<Query Kind="Program">
  <NuGetReference>MonoGame.Content.Builder.Task</NuGetReference>
  <NuGetReference>MonoGame.Framework.DesktopGL</NuGetReference>
  <Namespace>Microsoft.Xna.Framework</Namespace>
  <Namespace>Microsoft.Xna.Framework.Audio</Namespace>
  <Namespace>Microsoft.Xna.Framework.Content</Namespace>
  <Namespace>Microsoft.Xna.Framework.Design</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics.PackedVector</Namespace>
  <Namespace>Microsoft.Xna.Framework.Input</Namespace>
  <Namespace>Microsoft.Xna.Framework.Input.Touch</Namespace>
  <Namespace>Microsoft.Xna.Framework.Media</Namespace>
  <Namespace>MonoGame.Framework.Utilities</Namespace>
  <Namespace>MonoGame.Framework.Utilities.Deflate</Namespace>
  <Namespace>MonoGame.OpenGL</Namespace>
  <Namespace>NVorbis</Namespace>
  <Namespace>NVorbis.Ogg</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>


[STAThread]
static void Main()
{
	var game = new MyGame();
	
	game.Run();
}





public class MyGame : Game
{
	protected override void Initialize()
	{
		
		base.Initialize();
	}
}