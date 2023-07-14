using System.Drawing;
using System.Text;

public class SvgRenderer : IRenderer
{
    private readonly ViewBox _defaultViewBox;

    public SvgRenderer(ViewBox defaultViewBox) => _defaultViewBox = defaultViewBox;

    public string Render(params Polygon[] polygons) => BuildSvgImage(polygons);

    private string BuildSvgImage(Polygon[] polygons, bool includeSvgTag = false)
    {
        var svgBuilder = new StringBuilder();

        if (includeSvgTag)
        {
            svgBuilder.AppendLine("<svg xmlns='http://www.w3.org/2000/svg' version='1.1'>");
        }

        var backgroundRect = BuildBackgroundRect(Color.Transparent);
        svgBuilder.AppendLine(backgroundRect);

        foreach (var polygon in polygons)
        {
            var polygonElement = BuildPolygonElement(polygon);
            svgBuilder.AppendLine(polygonElement);
        }

        if (includeSvgTag)
        {
            svgBuilder.AppendLine("</svg>");
        }

        var svgImage = svgBuilder.ToString();
        return svgImage;
    }

    private static string BuildPolygonElement(Polygon polygon)
    {
        var scale = 10;

        var fill = $"#{128:X2}{128:X2}{128:X2}";
        var stroke = $"#{0:X2}{0:X2}{0:X2}";
        var strokeWidth = 1;

        var points = string.Join(" ", polygon.Select(vertex => $"{vertex.X * scale},{vertex.Y * scale}"));

        var polygonElement = $"<polygon points='{points}' style='fill:{fill};stroke:{stroke};stroke-width:{strokeWidth}' />";
        return polygonElement;
    }

    private string BuildBackgroundRect(Color color) => $"<rect x='{_defaultViewBox.X}' y='{_defaultViewBox.Y}' width='{_defaultViewBox.Width}' height='{_defaultViewBox.Height}' fill='{ColorTranslator.ToHtml(color)}' />";
}
