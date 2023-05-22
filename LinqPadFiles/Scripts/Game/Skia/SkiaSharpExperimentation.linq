<Query Kind="Program">
  <NuGetReference>SkiaSharp</NuGetReference>
  <NuGetReference>SkiaSharp.Views.Desktop.Common</NuGetReference>
  <Namespace>System.Globalization</Namespace>
</Query>

static void Main(string[] args)
{
    using (var game = new MyGame())
    {
        game.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        game.Run();
    }
}


class MyGame : 
{
    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;

        // Draw a black rectangle with a red border
        SKRect rect = new SKRect(100, 100, 200, 200);
        SKPaint fillPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true
        };
        SKPaint strokePaint = new SKPaint
        {
            Color = SKColors.Red,
            IsAntialias = true,
            StrokeWidth = 5,
            Style = SKPaintStyle.Stroke
        };
        canvas.DrawRect(rect, fillPaint);
        canvas.DrawRect(rect, strokePaint);
    }
}