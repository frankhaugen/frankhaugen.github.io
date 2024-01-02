<Query Kind="Statements">
  <NuGetReference>OpenTK</NuGetReference>
  <Namespace>OpenTK.Graphics.OpenGL</Namespace>
  <Namespace>OpenTK.Windowing.Common</Namespace>
  <Namespace>OpenTK.Windowing.Desktop</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>


var game = new MyGame(new(), new());

game.Run();

class MyGame : GameWindow
{
	private int _vertexShaderHandle;
	private int _fragmentShaderHandle;
	private int _shaderProgramHandle;

	public MyGame(GameWindowSettings gameWindowsSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowsSettings, nativeWindowSettings)
	{

	}

	public override void Close()
	{
		Environment.Exit(0);
	}

	protected override void OnLoad()
	{
		base.OnLoad();

		// Create and compile vertex shader
		_vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
		GL.ShaderSource(_vertexShaderHandle, vertexShaderSource);
		GL.CompileShader(_vertexShaderHandle);

		// Create and compile fragment shader
		_fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
		GL.ShaderSource(_fragmentShaderHandle, fragmentShaderSource);
		GL.CompileShader(_fragmentShaderHandle);

		// Create shader program and attach shaders
		_shaderProgramHandle = GL.CreateProgram();
		GL.AttachShader(_shaderProgramHandle, _vertexShaderHandle);
		GL.AttachShader(_shaderProgramHandle, _fragmentShaderHandle);

		// Link and use the shader program
		GL.LinkProgram(_shaderProgramHandle);
		GL.UseProgram(_shaderProgramHandle);
	}

	protected override void OnRenderFrame(FrameEventArgs e)
	{
		base.OnRenderFrame(e);

		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		// Set up vertex data and attributes
		GL.Begin(PrimitiveType.Triangles);
		GL.Color3(Color.Red);
		GL.Vertex2(0.5f, -0.5f);
		GL.Vertex2(0.5f, 0.5f);
		GL.End();

		SwapBuffers();
	}

	protected override void OnResize(ResizeEventArgs e)
	{
		base.OnResize(e);

		GL.Viewport(0, 0, e.Width, e.Height);
		GL.MatrixMode(MatrixMode.Projection);
		GL.LoadIdentity();
		GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
	}
	private string vertexShaderSource = @"
    #version 330

    in vec3 vertexPosition;
    in vec3 vertexColor;

    out vec3 fragmentColor;

    void main()
    {
        fragmentColor = vertexColor;
        gl_Position = vec4(vertexPosition, 1.0);
    }
";
	private string fragmentShaderSource = @"
    #version 330

    in vec3 fragmentColor;

    out vec4 outputColor;

    void main()
    {
        outputColor = vec4(fragmentColor, 1.0);
    }
";

}

