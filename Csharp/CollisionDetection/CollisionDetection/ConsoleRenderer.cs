public class ConsoleRenderer : IRenderer
{
    private readonly Viewport _viewport;

    public ConsoleRenderer(int width, float aspectRatio)
    {
        var height = (int)(width / aspectRatio);
        _viewport = new Viewport(new(width, height));
    }

    public string Render(params Polygon[] polygons)
    {
        _viewport.Clear();

        foreach (var polygon in polygons)
        {
            foreach (var vertex in polygon)
            {
                _viewport.SetPixel(vertex, "#");
            }
        }

        return _viewport.ToString();
    }
}