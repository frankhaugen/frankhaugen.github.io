<Query Kind="Statements">
  <NuGetReference>MonoGame.Framework.WindowsDX</NuGetReference>
  <Namespace>Microsoft.Xna.Framework</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics</Namespace>
</Query>

public class Textures
{
	private readonly GraphicsDevice _graphicsDevice;

	public Textures(GraphicsDeviceManager graphicsDeviceManager)
	{
		_graphicsDevice = graphicsDeviceManager.GraphicsDevice;
	}

	/// <summary>
	/// Gets a texture with the specified width, height, and color.
	/// </summary>
	/// <param name="width">The width of the texture in pixels. Default is 1.</param>
	/// <param name="height">The height of the texture in pixels. Default is 1.</param>
	/// <param name="color">The color of the texture. Default is White.</param>
	/// <returns>A texture with the specified width, height, and color.</returns>
	public Texture2D GetColor(int width = 1, int height = 1, Color color = default)
	{
		if (color == default)
		{
			color = Color.White;
		}

		Color[] data = new Color[width * height];
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = color;
		}

		Texture2D texture = new Texture2D(_graphicsDevice, width, height);
		texture.SetData(data);
		return texture;
	}
}
