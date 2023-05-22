<Query Kind="Program">
  <NuGetReference>Silk.NET.OpenGL</NuGetReference>
  <NuGetReference>Silk.NET.Windowing</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Silk.NET.Windowing</Namespace>
  <Namespace>Silk.NET.OpenGL</Namespace>
</Query>

    void Main()
    {
        // Create a window
        var windowOptions = new WindowOptions(){
            Title = "My 3D Scene",
            Size = new Silk.NET.Maths.Vector2D<int>(800,600)
        };
        
        IWindow window = Window.Create(windowOptions);
    
        // Create an OpenGL context
        var openGL = GL.GetApi(window);
    
        // Set the background color to blue
        openGL.ClearColor(0.0f, 0.0f, 1.0f, 1.0f);
    
        // Run the main loop
        while (window.IsVisible)
        {
            // Clear the screen
            openGL.Clear(ClearBufferMask.ColorBufferBit);
    
            // Draw your 3D scene here
            
    
            // Swap the front and back buffers
            window.SwapBuffers();
    
            // Process events
            window.DoRender();
        }
    }
