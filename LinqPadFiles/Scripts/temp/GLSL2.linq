<Query Kind="Statements">
  <NuGetReference>OpenTK</NuGetReference>
  <Namespace>OpenTK.Graphics.OpenGL</Namespace>
</Query>


	public static class GLSLFactory
	{
		public static GLSLProgram CreateProgram(int vertexShaderHandle, int fragmentShaderHandle)
		{
			// Create shader program
			int program = GL.CreateProgram();
			GL.AttachShader(program, vertexShaderHandle);
			GL.AttachShader(program, fragmentShaderHandle);
			GL.LinkProgram(program);
			GL.ValidateProgram(program);

			// Check for errors
			int status;
			GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
			if (status == 0)
			{
				string infoLog = GL.GetProgramInfoLog(program);
				throw new Exception($"Error linking program: {infoLog}");
			}

			return new GLSLProgram(program);
		}

		public static GLSLVariable CreateVariable(string name, string type, string value)
		{
			return new GLSLVariable(name, type, value);
		}

		public static int CreateShader(ShaderType type, string source)
		{
			// Create shader
			int shader = GL.CreateShader(type);

			// Set shader source code
			GL.ShaderSource(shader, source);

			// Compile shader
			GL.CompileShader(shader);

			// Check for errors
			int status;
			GL.GetShader(shader, ShaderParameter.CompileStatus, out status);
			if (status == 0)
			{
				string infoLog = GL.GetShaderInfoLog(shader);
				throw new Exception($"Error compiling shader: {infoLog}");
			}

			return shader;
		}

		public static int CreateShaderFromFile(ShaderType type, string path)
		{
			// Read shader source code from file
			string source = File.ReadAllText(path);

			return CreateShader(type, source);
		}

		public static void UseProgram(GLSLProgram program)
		{
			GL.UseProgram(program.Handle);
		}
	}

	public class GLSLProgram
	{
		public int Handle { get; }

		public GLSLProgram(int handle)
		{
			Handle = handle;
		}
	}

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
