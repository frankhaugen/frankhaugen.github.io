<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
  <NuGetReference>OpenTK</NuGetReference>
  <Namespace>OpenTK</Namespace>
  <Namespace>OpenTK.Graphics</Namespace>
  <Namespace>OpenTK.Graphics.OpenGL</Namespace>
  <Namespace>OpenTK.Input</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>OpenTK.Windowing.Desktop</Namespace>
  <Namespace>OpenTK.Windowing.Common</Namespace>
  <Namespace>OpenTK.Mathematics</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>


void Main()
{
    // Create a new OpenGL window
    using (var window = new GameWindow(new() {}, new() { Title = "My Game", Size = new Vector2i(800, 600) }))
    {
        // Set the update and render frames
        window.UpdateFrame += (FrameEventArgs e) => Update(window, e);
        window.RenderFrame += (FrameEventArgs e) => Render(window, e);

        // Run the game loop
        window.Run();
    }
}

void Update(GameWindow sender, FrameEventArgs e)
{
    // Update the game state
}

void Render(GameWindow sender, FrameEventArgs e)
{
    // Clear the screen
    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

    // Set the viewport
    GL.Viewport(0, 0, 800, 600);

    // Set the projection matrix
    Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 800f / 600f, 0.1f, 1000f);
    GL.MatrixMode(MatrixMode.Projection);
    GL.LoadMatrix(ref projection);

    // Set the modelview matrix
    Matrix4 modelview = Matrix4.LookAt(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY);
    GL.MatrixMode(MatrixMode.Modelview);
    GL.LoadMatrix(ref modelview);

    // Draw a triangle
    GL.Begin(PrimitiveType.Triangles);
    GL.Color3(Color.Red);
    GL.Vertex3(-1.0f, -1.0f, 0.0f);
    GL.Color3(Color.Green);
    GL.Vertex3(1.0f, -1.0f, 0.0f);
    GL.Color3(Color.Blue);
    GL.Vertex3(0.0f, 1.0f, 0.0f);
    GL.End();
    
    sender.SwapBuffers();
}
