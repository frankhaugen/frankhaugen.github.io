<Query Kind="Program">
  <NuGetReference Prerelease="true">TerraFX.ApplicationModel</NuGetReference>
  <NuGetReference Prerelease="true">TerraFX.Interop.Vulkan</NuGetReference>
  <NuGetReference Prerelease="true">TerraFX.WinForms</NuGetReference>
  <Namespace>GC = System.GC</Namespace>
  <Namespace>static TerraFX.Interop.DirectX.D3D</Namespace>
  <Namespace>static TerraFX.Interop.DirectX.D3DCOMPILE</Namespace>
  <Namespace>static TerraFX.Interop.DirectX.DirectX</Namespace>
  <Namespace>static TerraFX.Utilities.AssertionUtilities</Namespace>
  <Namespace>static TerraFX.Utilities.ExceptionUtilities</Namespace>
  <Namespace>static TerraFX.Utilities.MarshalUtilities</Namespace>
  <Namespace>static TerraFX.Utilities.UnsafeUtilities</Namespace>
  <Namespace>TerraFX.ApplicationModel</Namespace>
  <Namespace>TerraFX.Graphics</Namespace>
  <Namespace>TerraFX.Interop.DirectX</Namespace>
  <Namespace>TerraFX.Numerics</Namespace>
  <Namespace>TerraFX.UI</Namespace>
  <Namespace>TerraFX.Utilities</Namespace>
  <Namespace>TerraFX</Namespace>
</Query>



void Main()
{
	using (var window = new HelloQuad("Hi"))
	{
		window.Initialize(new Application(), TimeSpan.FromSeconds(30));
		window.Window.Show();
	}
}

// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.


public sealed class HelloQuad : HelloWindow
{
	private GraphicsBuffer _indexBuffer = null!;
	private GraphicsPrimitive _quadPrimitive = null!;
	private GraphicsBuffer _uploadBuffer = null!;
	private GraphicsBuffer _vertexBuffer = null!;

	public HelloQuad(string name) : base(name)
	{
	}

	public override void Cleanup()
	{
		_quadPrimitive?.Dispose();

		_indexBuffer?.Dispose();
		_uploadBuffer?.Dispose();
		_vertexBuffer?.Dispose();

		base.Cleanup();
	}

	/// <summary>Initializes the GUI for this sample.</summary>
	/// <param name="application">The hosting <see cref="Application" />.</param>
	/// <param name="timeout">The <see cref="TimeSpan" /> after which this sample should stop running.</param>
	/// <param name="windowLocation">The <see cref="Vector2" /> that defines the initial window location.</param>
	/// <param name="windowSize">The <see cref="Vector2" /> that defines the initial window client rectangle size.</param>
	public override void Initialize(Application application, TimeSpan timeout, Vector2? windowLocation, Vector2? windowSize)
	{
		base.Initialize(application, timeout, windowLocation, windowSize);

		var graphicsDevice = GraphicsDevice;

		_indexBuffer = graphicsDevice.CreateIndexBuffer(64 * 1024);
		_uploadBuffer = graphicsDevice.CreateUploadBuffer(64 * 1024);
		_vertexBuffer = graphicsDevice.CreateVertexBuffer(64 * 1024);

		var copyCommandQueue = graphicsDevice.CopyCommandQueue;
		var copyContext = copyCommandQueue.RentContext();
		{
			copyContext.Reset();
			{
				_quadPrimitive = CreateQuadPrimitive(copyContext);
			}
			copyContext.Close();
			copyContext.Execute();
		}
		copyCommandQueue.ReturnContext(copyContext);

		_uploadBuffer.DisposeAllViews();
	}

	protected override void Draw(GraphicsRenderContext renderContext)
	{
		_quadPrimitive.Draw(renderContext);
		base.Draw(renderContext);
	}

	private unsafe GraphicsPrimitive CreateQuadPrimitive(GraphicsCopyContext copyContext)
	{
		var renderPass = RenderPass;
		var surface = renderPass.Surface;

		var graphicsPipeline = CreateGraphicsPipeline(renderPass, "Identity", "main", "main");
		var uploadBuffer = _uploadBuffer;

		return new GraphicsPrimitive(
			graphicsPipeline,
			CreateVertexBufferView(copyContext, _vertexBuffer, uploadBuffer, aspectRatio: surface.PixelWidth / surface.PixelHeight),
			CreateIndexBufferView(copyContext, _indexBuffer, uploadBuffer)
		);

		static GraphicsBufferView CreateIndexBufferView(GraphicsCopyContext copyContext, GraphicsBuffer indexBuffer, GraphicsBuffer uploadBuffer)
		{
			var uploadBufferView = uploadBuffer.CreateBufferView<ushort>(6);
			var indexBufferSpan = uploadBufferView.Map<ushort>();
			{
				// clockwise when looking at the triangle from the outside

				indexBufferSpan[0] = 0;
				indexBufferSpan[1] = 1;
				indexBufferSpan[2] = 2;

				indexBufferSpan[3] = 0;
				indexBufferSpan[4] = 2;
				indexBufferSpan[5] = 3;
			}
			uploadBufferView.UnmapAndWrite();

			var indexBufferView = indexBuffer.CreateBufferView<ushort>(6);
			copyContext.Copy(indexBufferView, uploadBufferView);
			return indexBufferView;
		}

		static GraphicsBufferView CreateVertexBufferView(GraphicsCopyContext copyContext, GraphicsBuffer vertexBuffer, GraphicsBuffer uploadBuffer, float aspectRatio)
		{
			var uploadBufferView = uploadBuffer.CreateBufferView<IdentityVertex>(4);
			var vertexBufferSpan = uploadBufferView.Map<IdentityVertex>();
			{
				vertexBufferSpan[0] = new IdentityVertex
				{                          //
					Color = Colors.Red,                                             //   y          in this setup
					Position = Vector3.Create(-0.25f, 0.25f * aspectRatio, 0.0f),   //   ^     z    the origin o
				};                                                                  //   |   /      is in the middle
																					//   | /        of the rendered scene
				vertexBufferSpan[1] = new IdentityVertex
				{                          //   o------>x
					Color = Colors.Blue,                                            //
					Position = Vector3.Create(0.25f, 0.25f * aspectRatio, 0.0f),    //   0 ----- 1
				};                                                                  //   | \     |
																					//   |   \   |
				vertexBufferSpan[2] = new IdentityVertex
				{                          //   |     \ |
					Color = Colors.Lime,                                            //   3-------2
					Position = Vector3.Create(0.25f, -0.25f * aspectRatio, 0.0f),   //
				};

				vertexBufferSpan[3] = new IdentityVertex
				{
					Color = Colors.Blue,
					Position = Vector3.Create(-0.25f, -0.25f * aspectRatio, 0.0f),
				};
			}
			uploadBufferView.UnmapAndWrite();

			var vertexBufferView = vertexBuffer.CreateBufferView<IdentityVertex>(4);
			copyContext.Copy(vertexBufferView, uploadBufferView);
			return vertexBufferView;
		}

		GraphicsPipeline CreateGraphicsPipeline(GraphicsRenderPass renderPass, string shaderName, string vertexShaderEntryPoint, string pixelShaderEntryPoint)
		{
			var graphicsDevice = renderPass.Device;

			var pipelineCreateOptions = new GraphicsPipelineCreateOptions
			{
				Signature = CreateGraphicsPipelineSignature(graphicsDevice),
				PixelShader = CompileShader(graphicsDevice, GraphicsShaderKind.Pixel, shaderName, pixelShaderEntryPoint),
				VertexShader = CompileShader(graphicsDevice, GraphicsShaderKind.Vertex, shaderName, vertexShaderEntryPoint),
			};

			return renderPass.CreatePipeline(in pipelineCreateOptions);
		}

		GraphicsPipelineSignature CreateGraphicsPipelineSignature(GraphicsDevice graphicsDevice)
		{
			var inputs = new UnmanagedArray<GraphicsPipelineInput>(2)
			{
				[0] = new GraphicsPipelineInput
				{
					BindingIndex = 0,
					ByteAlignment = 16,
					ByteLength = 16,
					Format = GraphicsFormat.R32G32B32A32_SFLOAT,
					Kind = GraphicsPipelineInputKind.Color,
					ShaderVisibility = GraphicsShaderVisibility.Vertex,
				},
				[1] = new GraphicsPipelineInput
				{
					BindingIndex = 1,
					ByteAlignment = 4,
					ByteLength = 12,
					Format = GraphicsFormat.R32G32B32_SFLOAT,
					Kind = GraphicsPipelineInputKind.Position,
					ShaderVisibility = GraphicsShaderVisibility.Vertex,
				},
			};

			return graphicsDevice.CreatePipelineSignature(inputs);
		}
	}
}

// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.


public class HelloWindow : Sample
{
	private GraphicsDevice _graphicsDevice = null!;
	private GraphicsRenderPass _renderPass = null!;
	private UIWindow _window = null!;
	private TimeSpan _elapsedTime;
	private uint _secondsOfLastFpsUpdate;

	public HelloWindow(string name) : base(name)
	{
	}

	public GraphicsDevice GraphicsDevice => _graphicsDevice;

	public GraphicsRenderPass RenderPass => _renderPass;

	public UIWindow Window => _window;

	public override void Cleanup()
	{
		_renderPass?.Dispose();
		_graphicsDevice?.Dispose();
		_window?.Dispose();

		base.Cleanup();
	}

	/// <summary>Initializes the GUI for this sample.</summary>
	/// <param name="application">The hosting <see cref="Application" />.</param>
	/// <param name="timeout">The <see cref="TimeSpan" /> after which this sample should stop running.</param>
	public override void Initialize(Application application, TimeSpan timeout) => Initialize(application, timeout, null, null);

	/// <summary>Initializes the GUI for this sample.</summary>
	/// <param name="application">The hosting <see cref="Application" />.</param>
	/// <param name="timeout">The <see cref="TimeSpan" /> after which this sample should stop running.</param>
	/// <param name="windowLocation">The <see cref="Vector2" /> that defines the initial window location.</param>
	/// <param name="windowSize">The <see cref="Vector2" /> that defines the initial window client rectangle size.</param>
	public virtual void Initialize(Application application, TimeSpan timeout, Vector2? windowLocation, Vector2? windowSize)
	{
		ExceptionUtilities.ThrowIfNull(application);

		var uiService = application.UIService;

		_window = uiService.DispatcherForCurrentThread.CreateWindow();
		_window.SetTitle(Name);

		if (windowLocation.HasValue)
		{
			_window.Relocate(windowLocation.GetValueOrDefault());
		}

		if (windowSize.HasValue)
		{
			_window.ResizeClient(windowSize.GetValueOrDefault());
		}

		_window.Show();

		var graphicsService = application.GraphicsService;
		var graphicsAdapter = graphicsService.Adapters.First();

		var graphicsDevice = graphicsAdapter.CreateDevice();
		_graphicsDevice = graphicsDevice;

		_renderPass = graphicsDevice.CreateRenderPass(_window, GraphicsFormat.B8G8R8A8_UNORM);
		base.Initialize(application, timeout);
	}

	protected virtual void Draw(GraphicsRenderContext renderContext) { }

	protected virtual void Update(TimeSpan delta) { }

	protected override void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs)
	{
		ExceptionUtilities.ThrowIfNull(sender);

		_elapsedTime += eventArgs.Delta;

		if (_elapsedTime >= Timeout)
		{
			var application = (Application)sender;
			application.RequestExit();
		}

		if (_window.IsVisible)
		{
			// add current fps to the end of the Window Title
			var seconds = (uint)_elapsedTime.TotalSeconds;
			if (_secondsOfLastFpsUpdate < seconds)
			{
				var newTitle = $"{Name} ({eventArgs.FramesPerSecond} fps)";
				Window.SetTitle(newTitle);
				_secondsOfLastFpsUpdate = seconds;
			}

			Update(eventArgs.Delta);
			Render();
			Present();
		}
	}

	protected void Present() => RenderPass.Swapchain.Present();

	protected void Render()
	{
		var renderCommandQueue = GraphicsDevice.RenderCommandQueue;
		var renderContext = renderCommandQueue.RentContext();
		{
			renderContext.Reset();
			{
				renderContext.BeginRenderPass(RenderPass, Colors.CornflowerBlue);
				{
					var surfaceSize = RenderPass.Surface.Size;

					var viewport = BoundingBox.CreateFromSize(Vector3.Zero, Vector3.Create(surfaceSize, 1.0f));
					renderContext.SetViewport(viewport);

					var scissor = BoundingRectangle.CreateFromSize(Vector2.Zero, surfaceSize);
					renderContext.SetScissor(scissor);

					Draw(renderContext);
				}
				renderContext.EndRenderPass();
			}
			renderContext.Close();
			renderContext.Execute();
		}
		renderCommandQueue.ReturnContext(renderContext);
	}
}

// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.


public abstract unsafe class Sample : IDisposable
{
	private readonly string _assemblyPath;
	private readonly string _name;
	private TimeSpan _timeout;

	protected Sample(string name)
	{
		var entryAssembly = Assembly.GetEntryAssembly()!;
		_assemblyPath = Path.GetDirectoryName(entryAssembly.Location)!;

		_name = name;
	}

	~Sample() => Dispose(isDisposing: false);

	// ps_5_0
	private static ReadOnlySpan<sbyte> D3D12CompileTarget_ps_5_0 => new sbyte[] { 0x70, 0x73, 0x5F, 0x35, 0x5F, 0x30, 0x00 };

	// vs_5_0
	private static ReadOnlySpan<sbyte> D3D12CompileTarget_vs_5_0 => new sbyte[] { 0x76, 0x73, 0x5F, 0x35, 0x5F, 0x30, 0x00 };

	public string Name => _name;

	public TimeSpan Timeout => _timeout;

	public virtual void Cleanup() { }

	public void Dispose()
	{
		Dispose(isDisposing: true);
		GC.SuppressFinalize(this);
	}

	public virtual void Initialize(Application application, TimeSpan timeout)
	{
		_timeout = timeout;
		application.Idle += OnIdle;
	}

	protected GraphicsShader CompileShader(GraphicsDevice graphicsDevice, GraphicsShaderKind kind, string shaderName, string entryPointName)
	{
		GraphicsShader? graphicsShader = null;

		fixed (char* assetPath = GetAssetFullPath("Shaders", shaderName, $"{shaderName}{kind}.hlsl"))
		fixed (sbyte* entryPoint = entryPointName.GetUtf8Span())
		{
			var compileFlags = 0u;

			if (GraphicsService.EnableDebugMode)
			{
				// Enable better shader debugging with the graphics debugging tools.
				compileFlags |= D3DCOMPILE_DEBUG | D3DCOMPILE_SKIP_OPTIMIZATION;
			}
			else
			{
				compileFlags |= D3DCOMPILE_OPTIMIZATION_LEVEL3;
			}

			ID3DBlob* d3dShaderBlob = null;
			ID3DBlob* d3dShaderErrorBlob = null;

			var result = D3DCompileFromFile((ushort*)assetPath, pDefines: null, D3D_COMPILE_STANDARD_FILE_INCLUDE, entryPoint, GetD3D12CompileTarget(kind).GetPointerUnsafe(), compileFlags, Flags2: 0, &d3dShaderBlob, ppErrorMsgs: &d3dShaderErrorBlob);

			if (result.SUCCEEDED)
			{
				var bytecode = new UnmanagedArray<byte>(d3dShaderBlob->GetBufferSize());
				new UnmanagedReadOnlySpan<byte>((byte*)d3dShaderBlob->GetBufferPointer(), bytecode.Length).CopyTo(bytecode);

				switch (kind)
				{
					case GraphicsShaderKind.Pixel:
						{
							graphicsShader = graphicsDevice.CreatePixelShader(bytecode, entryPointName);
							break;
						}

					case GraphicsShaderKind.Vertex:
						{
							graphicsShader = graphicsDevice.CreateVertexShader(bytecode, entryPointName);
							break;
						}

					default:
						{
							ThrowForInvalidKind(kind);
							break;
						}
				}
			}

			if (d3dShaderBlob != null)
			{
				_ = d3dShaderBlob->Release();
			}

			if (d3dShaderErrorBlob != null)
			{
				_ = d3dShaderErrorBlob->Release();
			}

			if (result.FAILED)
			{
				var errorMsg = GetUtf8Span((sbyte*)d3dShaderErrorBlob->GetBufferPointer(), (int)d3dShaderErrorBlob->GetBufferSize()).GetString();
				Console.WriteLine(errorMsg);
				ExceptionUtilities.ThrowExternalException(nameof(D3DCompileFromFile), result);
			}

			AssertNotNull(graphicsShader);
			return graphicsShader;
		}

		static ReadOnlySpan<sbyte> GetD3D12CompileTarget(GraphicsShaderKind graphicsShaderKind)
		{
			ReadOnlySpan<sbyte> d3d12CompileTarget;

			switch (graphicsShaderKind)
			{
				case GraphicsShaderKind.Vertex:
					{
						d3d12CompileTarget = D3D12CompileTarget_vs_5_0;
						break;
					}

				case GraphicsShaderKind.Pixel:
					{
						d3d12CompileTarget = D3D12CompileTarget_ps_5_0;
						break;
					}

				default:
					{
						ThrowNotImplementedException();
						d3d12CompileTarget = default;
						break;
					}
			}

			return d3d12CompileTarget;
		}
	}

	protected virtual void Dispose(bool isDisposing) => Cleanup();

	protected abstract void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs);

	private string GetAssetFullPath(string assetCategory, string assetFolder, string assetName) => Path.Combine(_assemblyPath, "Assets", assetCategory, assetFolder, assetName);
}

// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.


public struct IdentityVertex
{
	public ColorRgba Color;
	public Vector3 Position;
}