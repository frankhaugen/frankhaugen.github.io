using System.Windows;
using System.Windows.Controls;

namespace WpfWithoutXaml.Controls;

public class MyStackPanel : StackPanel
{
    public MyStackPanel(Orientation orientation = Orientation.Horizontal)
    {
        Orientation = orientation;
    }

    public void Add(UIElement uiElement) => Children.Add(uiElement);
}

public class EasyStackPanel : StackPanel
{
    public EasyStackPanel()
    {
    }

    public void Add(UIElement uiElement) => Children.Add(uiElement);
    public void Add(params UIElement[] uiElements)
    {
        foreach (var uiElement in uiElements)
        {
            Children.Add(uiElement);
        }
    }
}

public class TextInput : GroupBox
{
    private readonly TextBox _textBox;

    public TextInput(string header, Action<object, TextChangedEventArgs> textChanged, string text = "")
    {
        _textBox = new TextBox()
        {
            Text = text
        };
        _textBox.TextChanged += textChanged.Invoke;
        Header = header;

        base.Content = _textBox;
    }

    public new string Content => _textBox.Text;
}

public class PasswordInput : GroupBox
{
    private readonly PasswordBox _textBox;

    public PasswordInput(string header, string text = "")
    {
        _textBox = new PasswordBox()
        {
            Password = text
        };
        Header = header;

        base.Content = _textBox;
    }

    public new string Content { get; set; }
}