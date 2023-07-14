using System.Text;

public class Viewport
{
    private readonly string[,] _buffer;
    
    public Viewport(Vertex size)
    {
        _buffer = new string[(int)size.X + 2, (int)size.Y + 2];

        for (var y = 0; y < size.Y; y++)
        {
            for (var x = 0; x < size.X; x++)
            {
                _buffer[x, y] = " ";
            }
        }

        CreateBorder();
    }

private void CreateBorder()
{
    for (var x = 0; x < _buffer.GetLength(0); x++)
    {
        _buffer[x, 0] = "─";
        _buffer[x, _buffer.GetLength(1) - 1] = "─";
    }

    for (var y = 0; y < _buffer.GetLength(1); y++)
    {
        _buffer[0, y] = "│";
        _buffer[_buffer.GetLength(0) - 1, y] = "│";
    }

    _buffer[0, 0] = "┌";
    _buffer[_buffer.GetLength(0) - 1, 0] = "┐";
    _buffer[0, _buffer.GetLength(1) - 1] = "└";
    _buffer[_buffer.GetLength(0) - 1, _buffer.GetLength(1) - 1] = "┘";

    var horizontalSpacing = (_buffer.GetLength(0) - 2) / 10;
    var verticalSpacing = (_buffer.GetLength(1) - 2) / 10;

    for (var i = 1; i <= 10; i++)
    {
        var x = i * horizontalSpacing;
        _buffer[x, 0] = "┬";
        _buffer[x, _buffer.GetLength(1) - 1] = "┴";

        var y = i * verticalSpacing;
        _buffer[0, y] = "├";
        _buffer[_buffer.GetLength(0) - 1, y] = "┤";
    }

    _buffer[1, _buffer.GetLength(1) - 2] = "+";
}

    public void Clear()
    {
        for (var y = 0; y < _buffer.GetLength(1); y++)
        {
            for (var x = 0; x < _buffer.GetLength(0); x++)
            {
                _buffer[x, y] = " ";
            }
        }

        CreateBorder();
    }

    public void SetPixel(Vertex vertex, string pixel)
    {
        var x = (int)vertex.X;
        var y = (int)vertex.Y;

        if (x >= 0 && x < _buffer.GetLength(0) && y >= 0 && y < _buffer.GetLength(1))
        {
            _buffer[x, _buffer.GetLength(1) - y - 1] = pixel;
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        for (var y = 0; y < _buffer.GetLength(1); y++)
        {
            for (var x = 0; x < _buffer.GetLength(0); x++)
            {
                builder.Append(_buffer[x, y]);
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}
internal class ViewportV1
{
    private readonly char[,] _buffer;
    
    public ViewportV1(Vertex size)
    {

        _buffer = new char[(int)size.X + 2, (int)size.Y + 2];

        for (var y = 0; y < size.Y; y++)
        {
            for (var x = 0; x < size.X; x++)
            {
                _buffer[x, y] = ' ';
            }
        }

        CreateBorder();
    }

    private void CreateBorder()
    {
        for (var x = 0; x < _buffer.GetLength(0); x++)
        {
            _buffer[x, 0] = '-';
            _buffer[x, _buffer.GetLength(1) - 1] = '-';
        }

        for (var y = 0; y < _buffer.GetLength(1); y++)
        {
            _buffer[0, y] = '|';
            _buffer[_buffer.GetLength(0) - 1, y] = '|';
        }

        _buffer[0, 0] = '+';
        _buffer[_buffer.GetLength(0) - 1, 0] = '+';
        _buffer[0, _buffer.GetLength(1) - 1] = '+';
        _buffer[_buffer.GetLength(0) - 1, _buffer.GetLength(1) - 1] = '+';
    }

    public void Clear()
    {
        for (var y = 0; y < _buffer.GetLength(1); y++)
        {
            for (var x = 0; x < _buffer.GetLength(0); x++)
            {
                _buffer[x, y] = ' ';
            }
        }

        CreateBorder();
    }

    public void SetPixel(Vertex vertex, char pixel)
    {
        var x = (int)vertex.X;
        var y = (int)vertex.Y;

        if (x >= 0 && x < _buffer.GetLength(0) && y >= 0 && y < _buffer.GetLength(1))
        {
            _buffer[x, _buffer.GetLength(1) - y - 1] = pixel;
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        for (var y = 0; y < _buffer.GetLength(1); y++)
        {
            for (var x = 0; x < _buffer.GetLength(0); x++)
            {
                builder.Append(_buffer[x, y]);
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}
