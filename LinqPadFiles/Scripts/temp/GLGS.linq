<Query Kind="Statements" />


public abstract class GLSLNode
{
	public abstract string Emit();
}

public class GLSLProgram : GLSLNode
{
	public GLSLFunction Main { get; set; }

	public override string Emit()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("#version 330");
		sb.AppendLine(Main.Emit());
		return sb.ToString();
	}
}

public class GLSLFunction : GLSLNode
{
	public string ReturnType { get; set; }
	public string Name { get; set; }
	public GLSL Statement { get; set; }

	public override string Emit()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append($"{ReturnType} {Name}()");
		sb.AppendLine("{");
		sb.AppendLine(Statement.Emit());
		sb.AppendLine("}");
		return sb.ToString();
	}
}

public abstract class GLSL : GLSLNode
{
	public string Indent { get; set; } = "";

	public abstract override string Emit();
}

public class GLSLBlock : GLSL
{
	public GLSL[] Statements { get; set; }

	public override string Emit()
	{
		StringBuilder sb = new StringBuilder();
		foreach (var statement in Statements)
		{
			sb.Append(statement.Indent);
			sb.AppendLine(statement.Emit());
		}
		return sb.ToString();
	}
}

public class GLSLVariableDeclaration : GLSL
{
	public string Type { get; set; }
	public string Name { get; set; }
	public GLSLExpression InitialValue { get; set; }

	public override string Emit()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append($"{Type} {Name}");
		if (InitialValue != null)
		{
			sb.Append(" = ");
			sb.Append(InitialValue.Emit());
		}
		sb.Append(";");
		return sb.ToString();
	}
}

public class GLSLReturn : GLSL
{
	public GLSLExpression Value { get; set; }

	public override string Emit()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("return ");
		sb.Append(Value.Emit());
		sb.Append(";");
		return sb.ToString();
	}
}

public abstract class GLSLExpression : GLSLNode
{
}

public class GLSLValue : GLSLExpression
{
	public object Value { get; set; }

	public override string Emit()
	{
		if (Value is string)
		{
			return $"\"{Value}\"";
		}
		else
		{
			return Value.ToString();
		}
	}
}

public class GLSLBinaryOperator : GLSLExpression
{
	public GLSLExpression Left { get; set; }
	public string Operator { get; set; }
	public GLSLExpression Right { get; set; }

	public override string Emit()
	{
		return $"{Left.Emit()} {Operator} {Right.Emit()}";
	}
}

public class GLSLFunctionCall : GLSLExpression
{
	public string FunctionName { get; set; }
	public GLSLExpression[] Arguments { get; set; }

	public override string Emit()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(FunctionName);
		sb.Append("(");
		if (Arguments.Length > 0)
		{
			sb.Append(Arguments[0].Emit());
			for (int i = 1; i < Arguments.Length; i++)
			{
				sb.Append(", ");
				sb.Append(Arguments[i].Emit());
			}
		}
		sb.Append(")");
		return sb.ToString();
	}
}

public class GLSLArrayAccess : GLSLExpression
{
	public GLSLExpression Array { get; set; }
	public GLSLExpression Index { get; set; }

	public override string Emit()
	{
		return $"{Array.Emit()}[{Index.Emit()}]";
	}
}

namespace MyApplication
{
	public class GLSLVariable
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }

		public GLSLVariable(string name, string type, string value)
		{
			Name = name;
			Type = type;
			Value = value;
		}

		public override string ToString()
		{
			return $"{Type} {Name} = {Value};";
		}
	}
}