<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main()
{
	var builder = new IndentedStringBuilder();
	var subBuilder = new IndentedStringBuilder();
	subBuilder.IncreaseIndent()
			   .WriteLine("Sub-item 1")
			   .WriteLine("Sub-item 2");

	builder.WriteLine("List of items:")
		  .IncreaseIndent()
		  .WriteLine(subBuilder)
		  .DecreaseIndent()
		  .WriteLine("End of list {0}", DateTime.Now)
		  ;

	Console.WriteLine(builder.ToString());
}

/// <summary>
/// Defines the functionality for a builder that supports indentation-sensitive string building.
/// </summary>
public interface IIndentedStringBuilder
{
	/// <summary>
	/// Increases the current indentation level by one step.
	/// </summary>
	/// <returns>The current instance of the builder for method chaining.</returns>
	IIndentedStringBuilder IncreaseIndent();

	/// <summary>
	/// Decreases the current indentation level by one step, if it is greater than zero.
	/// </summary>
	/// <returns>The current instance of the builder for method chaining.</returns>
	IIndentedStringBuilder DecreaseIndent();

	/// <summary>
	/// Appends the specified text to the current string without a newline.
	/// </summary>
	/// <param name="text">The text to append.</param>
	/// <returns>The current instance of the builder for method chaining.</returns>
	IIndentedStringBuilder Write(string text);

	/// <summary>
	/// Appends the specified line of text to the current string with a newline.
	/// </summary>
	/// <param name="line">The line to append. If null, just appends a newline.</param>
	/// <returns>The current instance of the builder for method chaining.</returns>
	IIndentedStringBuilder WriteLine(string line = "");

	/// <summary>
	/// Appends formatted text to the current string without a newline.
	/// </summary>
	/// <param name="format">A composite format string.</param>
	/// <param name="args">An array of objects to format.</param>
	/// <returns>The current instance of the builder for method chaining.</returns>
	IIndentedStringBuilder Write(string format, params object[] args);

	/// <summary>
	/// Appends a formatted line of text to the current string with a newline.
	/// </summary>
	/// <param name="format">A composite format string.</param>
	/// <param name="args">An array of objects to format.</param>
	/// <returns>The current instance of the builder for method chaining.</returns>
	IIndentedStringBuilder WriteLine(string format, params object[] args);

	/// <summary>
	/// Appends the contents of another <see cref="IIndentedStringBuilder"/> instance to this builder, 
	/// preserving the indentation and adding new lines as needed.
	/// </summary>
	/// <param name="other">The other instance of IIndentedStringBuilder whose contents are to be appended.</param>
	/// <returns>The current instance of the builder for method chaining.</returns>
	IIndentedStringBuilder WriteLine(IIndentedStringBuilder other);
}


public class IndentedStringBuilder : IIndentedStringBuilder
{
	private StringBuilder _builder = new StringBuilder();
	private int _indentLevel = 0;
	private readonly string _indentString;

	public IndentedStringBuilder(string indentString = "    ")  // Default to 4 spaces
	{
		_indentString = indentString;
	}

	public IIndentedStringBuilder IncreaseIndent()
	{
		_indentLevel++;
		return this;
	}

	public IIndentedStringBuilder DecreaseIndent()
	{
		if (_indentLevel > 0)
			_indentLevel--;
		return this;
	}

	public IIndentedStringBuilder Write(string text)
	{
		_builder.Append(new String(_indentString[0], _indentLevel * _indentString.Length));
		_builder.Append(text);
		return this;
	}

	public IIndentedStringBuilder WriteLine(string line = "")
	{
		_builder.Append(new String(_indentString[0], _indentLevel * _indentString.Length));
		_builder.AppendLine(line);
		return this;
	}

	public IIndentedStringBuilder Write(string format, params object[] args)
	{
		string formattedText = string.Format(format, args);
		_builder.Append(new String(_indentString[0], _indentLevel * _indentString.Length));
		_builder.Append(formattedText);
		return this;
	}

	public IIndentedStringBuilder WriteLine(string format, params object[] args)
	{
		string formattedText = string.Format(format, args);
		_builder.Append(new String(_indentString[0], _indentLevel * _indentString.Length));
		_builder.AppendLine(formattedText);
		return this;
	}

	public IIndentedStringBuilder WriteLine(IIndentedStringBuilder other)
	{
		if (other is IndentedStringBuilder otherBuilder)
		{
			string[] lines = otherBuilder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			foreach (var line in lines.Where(l => l.Any()))
			{
				this.WriteLine(line);
			}
		}
		return this;
	}

	public override string ToString()
	{
		return _builder.ToString();
	}
}