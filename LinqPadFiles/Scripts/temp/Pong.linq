<Query Kind="Program">
  <NuGetReference>OpenTK</NuGetReference>
  <Namespace>OpenTK</Namespace>
  <Namespace>OpenTK.Graphics</Namespace>
  <Namespace>OpenTK.Graphics.OpenGL</Namespace>
  <Namespace>OpenTK.Mathematics</Namespace>
  <Namespace>OpenTK.Windowing.Common</Namespace>
  <Namespace>OpenTK.Windowing.Desktop</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>OpenTK.Windowing.GraphicsLibraryFramework</Namespace>
</Query>


void Main()
{
	using (PongGame game = new PongGame(new(), new()))
	{
		game.Run();
	}
}



public class PongGame : GameWindow
{
	private readonly Ball _ball;
	private readonly Paddle _leftPaddle;
	private readonly Paddle _rightPaddle;
	private readonly Shaders _shaders;
	private readonly GLWrapper _glWrapper;
	
	public float Width => Size.X;
	public float Height => Size.Y;

	public PongGame(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
	{
		_ball = new Ball(new Vector2(nativeWindowSettings.Size.X / 2, nativeWindowSettings.Size.Y / 2), new Vector2(5, 5), 50);
		_leftPaddle = new Paddle(new Vector2(50, nativeWindowSettings.Size.Y / 2), new Vector2(10, 60), 0.0f);
		_rightPaddle = new Paddle(new Vector2(nativeWindowSettings.Size.Y - 50, nativeWindowSettings.Size.Y / 2), new Vector2(10, 60), 0.0f);
		_shaders = new Shaders();
		_glWrapper = new GLWrapper();
	}
	protected override void OnLoad()
	{
		base.OnLoad();

		// Compile and link shaders into a shader program
		int shaderProgram = _glWrapper.CreateProgram(_shaders.Default.VertexShader.Source, _shaders.Default.FragmentShader.Source);

		// Set up projection matrix
		Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, -1, 1);
		_glWrapper.UseProgram(shaderProgram);
		int projectionLocation = GL.GetUniformLocation(shaderProgram, "projection");
		GL.UniformMatrix4(projectionLocation, false, ref projection);
	}

	protected override void OnUpdateFrame(FrameEventArgs e)
	{
		base.OnUpdateFrame(e);

		// Check for collisions and update ball position
		_ball.CheckCollision((int)Width, (int)Height, _leftPaddle, _rightPaddle);
		_ball.Update((float)e.Time);

		// Update paddle positions based on user input
		_leftPaddle.Update((float)e.Time);
		_rightPaddle.Update((float)e.Time);
	}

	protected override void OnRenderFrame(FrameEventArgs e)
	{
		base.OnRenderFrame(e);

		// Clear screen and draw game objects
		_glWrapper.ClearColor(Color.Black);
		_glWrapper.Clear(ClearBufferMask.ColorBufferBit);
		_ball.Draw(_glWrapper);
		_leftPaddle.Draw(_glWrapper);
		_rightPaddle.Draw(_glWrapper);
		SwapBuffers();
	}
}

public class Ball : GameObject
{
	private readonly float _radius;
	Vector2 Velocity;

	public Ball(Vector2 position, Vector2 velocity, float radius)
		: base(position, new Vector2(radius * 2))
	{
		Velocity = velocity;
		_radius = radius;
	}

	public override void Update(float elapsedTime)
	{
		// Update position based on velocity
		Position += Velocity * elapsedTime;
	}

	public override void Draw(GLWrapper gl)
	{
		// Draw ball using shaders

		// Draw ball as a triangle fan
		const int numTriangles = 32;
		float[] vertices = new float[numTriangles * 3];
		for (int i = 0; i < numTriangles; i++)
		{
			float angle = i / (float)numTriangles * 2 * (float)Math.PI;
			vertices[i * 3 + 0] = (float)Math.Cos(angle);
			vertices[i * 3 + 1] = (float)Math.Sin(angle);
			vertices[i * 3 + 2] = 0;
		}

		gl.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

		gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
		gl.EnableVertexAttribArray(0);

		gl.DrawArrays(BeginMode.TriangleFan, 0, numTriangles);
	}

	public void CheckCollision(int screenWidth, int screenHeight, Paddle leftPaddle, Paddle rightPaddle)
	{
		// Check for collision with left or right edge of screen
		if (Position.X - _radius < 0 || Position.X + _radius > screenWidth)
		{
			Velocity.X *= -1;
		}

		// Check for collision with top or bottom edge of screen
		if (Position.Y - _radius < 0 || Position.Y + _radius > screenHeight)
		{
			Velocity.Y *= -1;
		}

		// Check for collision with paddles
		if (leftPaddle.BoundingBox.IntersectsWith(BoundingBox))
		{
			Velocity.X *= -1;
		}
		if (rightPaddle.BoundingBox.IntersectsWith(BoundingBox))
		{
			Velocity.X *= -1;
		}
	}
}



public class Paddle : GameObject
{
	private readonly float _speed;
	public Paddle(Vector2 position, Vector2 size, float speed) : base(position, size)
	{
		_speed = speed;
	}

	Vector2 Velocity;

	public void MoveUp()
	{
		Velocity = new Vector2(0, -_speed);
	}

	public void MoveDown()
	{
		Velocity = new Vector2(0, _speed);
	}

	public void Stop()
	{
		Velocity = Vector2.Zero;
	}

	public override void Update(float elapsedTime)
	{
		Position += Velocity * elapsedTime;
	}
	public override void Draw(GLWrapper gl)
	{
		gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
	}

}

public abstract class GameObject
{
	public Vector2 Position { get; set; }
	public Vector2 Size { get; set; }
	public Vector2 Velocity { get; set; }
	public float Rotation { get; set; }
	public float AngularVelocity { get; set; }

	protected GameObject(Vector2 position, Vector2 size)
	{
		Position = position;
		Size = size;
		Rotation = 0;
	}

	public abstract void Update(float elapsedTime);

	public abstract void Draw(GLWrapper gl);

	public RectangleF BoundingBox => new RectangleF(
				GetBoundingBoxMinX(),
				GetBoundingBoxMinY(),
				GetBoundingBoxMaxX() - GetBoundingBoxMinX(),
				GetBoundingBoxMaxY() - GetBoundingBoxMinY()
			);

	public float GetBoundingBoxMinX()
	{
		return Math.Min(Math.Min(Math.Min(GetTopLeft().X, GetTopRight().X), GetBottomRight().X), GetBottomLeft().X);
	}

	public float GetBoundingBoxMinY()
	{
		return Math.Min(Math.Min(Math.Min(GetTopLeft().Y, GetTopRight().Y), GetBottomRight().Y), GetBottomLeft().Y);
	}

	public float GetBoundingBoxMaxX()
	{
		return Math.Max(Math.Max(Math.Max(GetTopLeft().X, GetTopRight().X), GetBottomRight().X), GetBottomLeft().X);
	}

	public float GetBoundingBoxMaxY()
	{
		return Math.Max(Math.Max(Math.Max(GetTopLeft().Y, GetTopRight().Y), GetBottomRight().Y), GetBottomLeft().Y);
	}

	public Vector2 GetTopLeft()
	{
		Vector2 halfSize = GetHalfSize();
		Quaternion rot = Quaternion.FromAxisAngle(Vector3.UnitZ, Rotation);
		return Vector2.Transform(-halfSize, rot) + Position;
	}

	public Vector2 GetTopRight()
	{
		Vector2 halfSize = GetHalfSize();
		Quaternion rot = Quaternion.FromAxisAngle(Vector3.UnitZ, Rotation);
		return Vector2.Transform(new Vector2(halfSize.X, -halfSize.Y), rot) + Position;
	}

	public Vector2 GetBottomRight()
	{
		Vector2 halfSize = GetHalfSize();
		Quaternion rot = Quaternion.FromAxisAngle(Vector3.UnitZ, Rotation);
		return Vector2.Transform(halfSize, rot) + Position;
	}

	public Vector2 GetBottomLeft()
	{
		Vector2 halfSize = GetHalfSize();
		Quaternion rot = Quaternion.FromAxisAngle(Vector3.UnitZ, Rotation);
		return Vector2.Transform(new Vector2(-halfSize.X, halfSize.Y), rot) + Position;
	}

	public Vector2 GetHalfSize()
	{
		return Size / 2.0f;
	}

}

public class GLWrapper
{
	public void VertexAttribPointer(int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset) =>
		GL.VertexAttribPointer(index, size, type, normalized, stride, offset);
		
	public void BufferData(BufferTarget target, int size, float[] data, BufferUsageHint usage) =>
	GL.BufferData(target, size, data, usage);

	public void EnableVertexAttribArray(int index) => GL.EnableVertexAttribArray(index);

	public void DrawArrays(BeginMode mode, int first, int count) => GL.DrawArrays(mode, first, count);
	public void DrawArrays(PrimitiveType type, int first, int count) => GL.DrawArrays(type, first, count);

	public void ClearColor(Color color) => GL.ClearColor(color);

	public void Clear(ClearBufferMask mask) => GL.Clear(mask);

	public void UseProgram(int program) => GL.UseProgram(program);

	public void DrawElements(BeginMode mode, int count, DrawElementsType type, IntPtr indices) => GL.DrawElements(mode, count, type, indices);

	public int CreateProgram(string vertexShaderSource, string fragmentShaderSource)
	{
		// Compile shaders
		int vertexShader = GL.CreateShader(ShaderType.VertexShader);
		GL.ShaderSource(vertexShader, vertexShaderSource);
		GL.CompileShader(vertexShader);

		int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
		GL.ShaderSource(fragmentShader, fragmentShaderSource);
		GL.CompileShader(fragmentShader);

		// Link shaders into a shader program
		int program = GL.CreateProgram();
		GL.AttachShader(program, vertexShader);
		GL.AttachShader(program, fragmentShader);
		GL.LinkProgram(program);

		// Clean up
		GL.DeleteShader(vertexShader);
		GL.DeleteShader(fragmentShader);

		return program;
	}
}


public class Shaders
{
	public readonly ShaderProgram Default = new ShaderProgram(
		new VertexShader(@"
                #version 330

                in vec3 vertexPosition;
                in vec3 vertexColor;

                uniform mat4 modelMatrix;
                uniform mat4 viewMatrix;
                uniform mat4 projectionMatrix;

                out vec3 fragmentColor;

                void main()
                {
                    fragmentColor = vertexColor;
                    gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(vertexPosition, 1.0);
                }
            "),
		new FragmentShader(@"
                #version 330

                in vec3 fragmentColor;

                out vec4 fragColor;

                void main()
                {
                    fragColor = vec4(fragmentColor, 1.0);
                }
            ")
	);
}

public class ShaderProgram
{
	public VertexShader VertexShader { get; }
	public FragmentShader FragmentShader { get; }

	public ShaderProgram(VertexShader vertexShader, FragmentShader fragmentShader)
	{
		VertexShader = vertexShader;
		FragmentShader = fragmentShader;
	}
}

public class VertexShader
{
	public string Source { get; }

	public VertexShader(string source)
	{
		Source = source;
	}
}

public class FragmentShader
{
	public string Source { get; }

	public FragmentShader(string source)
	{
		Source = source;
	}
}