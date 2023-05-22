<Query Kind="Program">
  <NuGetReference>SharpDX.Direct3D11</NuGetReference>
  <NuGetReference>SharpDX.XInput</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>SharpDX.Direct3D</Namespace>
  <Namespace>SharpDX.Direct3D11</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>SharpDX.DXGI</Namespace>
  <Namespace>SharpDX</Namespace>
</Query>

void Main()
{
    // Create a window
    var form = new RenderForm("SharpDX Sample");

    // Create a Direct3D device and swap chain
    var swapChainDesc = new SwapChainDescription()
    {
        BufferCount = 1,
        ModeDescription = new ModeDescription(form.ClientSize.Width, form.ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
        IsWindowed = true,
        OutputHandle = form.Handle,
        SampleDescription = new SampleDescription(1, 0),
        SwapEffect = SwapEffect.Discard,
        Usage = Usage.RenderTargetOutput
    };

    Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapChainDesc, out var device, out var swapChain);

    // Create a vertex buffer
    var vertices = new[]
    {
        new Vector3(0.0f, 0.5f, 0.5f),
        new Vector3(0.5f, -0.5f, 0.5f),
        new Vector3(-0.5f, -0.5f, 0.5f),
    };

    var vertexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, vertices);

    // Create a vertex shader
    var vertexShaderByteCode = ShaderBytecode.CompileFromFile("Triangle.hlsl", "VS", "vs_4_0");
    var vertexShader = new VertexShader(device, vertexShaderByteCode);

    // Create a pixel shader
    var pixelShaderByteCode = ShaderBytecode.CompileFromFile("Triangle.hlsl", "PS", "ps_4_0");
    var pixelShader = new PixelShader(device, pixelShaderByteCode);

    // Set the vertex and pixel shaders
    device.ImmediateContext.VertexShader.Set(vertexShader);
    device.ImmediateContext.PixelShader.Set(pixelShader);

    // Set the input layout
    var inputLayout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), new[]
    {
        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
    });
    device.ImmediateContext.InputAssembler.InputLayout = inputLayout;

    // Set the primitive topology
    device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

    // Set the vertex buffer
    device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<Vector3>(), 0));

    // Render the triangle
    device.ImmediateContext.Draw(3, 0);

    // Present the rendered image to the window
    swapChain.Present(0, PresentFlags.None);

    // Dispose of resources
    vertexShader.Dispose();
    pixelShader.Dispose();
    inputLayout.Dispose();
    vertexBuffer.Dispose();
    device.Dispose();
    swapChain.Dispose();
    form.Dispose();
}
