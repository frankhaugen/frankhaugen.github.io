<Query Kind="Program">
  <NuGetReference>OpenTK</NuGetReference>
  <Namespace>OpenTK</Namespace>
  <Namespace>OpenTK.Audio.OpenAL</Namespace>
  <Namespace>OpenTK.Core</Namespace>
  <Namespace>OpenTK.Graphics</Namespace>
  <Namespace>OpenTK.Graphics.GL</Namespace>
  <Namespace>OpenTK.Graphics.OpenGL</Namespace>
  <Namespace>OpenTK.Input</Namespace>
  <Namespace>OpenTK.Input.Hid</Namespace>
  <Namespace>OpenTK.Mathematics</Namespace>
  <Namespace>OpenTK.Platform.Windows</Namespace>
  <Namespace>OpenTK.Windowing.Common</Namespace>
  <Namespace>OpenTK.Windowing.Common.Input</Namespace>
  <Namespace>OpenTK.Windowing.Desktop</Namespace>
  <Namespace>OpenTK.Windowing.GraphicsLibraryFramework</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Windows.Input</Namespace>
</Query>

void Main()
{
    using (var game = new Game(800, 600, "MyGame"))
    {
        game.Run();
    }
}

public class Game : GameWindow
{

    private PlayerController playerController;
    private World world;
    private IWorldGenerator generator;
    private IRenderer renderer;
    private PlayerCamera camera;
    private WorldOptions options;

    public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
    {
        // Initialize game components
        options = new WorldOptions(666, 0.5f, 20, 0.5f,0.5f);
        generator = new PerlinNoiseGenerator();
        world = new World(10, 10);
        camera = new PlayerCamera(new Vector3(0, 0, 0), Vector3.UnitZ);
        renderer = new OpenTkRenderer(camera);
        playerController = new PlayerController(camera);
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        
        //generator.GenerateWorld(options, ref world);
   
        // Setup OpenGL
        GL.ClearColor(Color4.SkyBlue);
        GL.Enable(EnableCap.DepthTest);

        // Load shaders
        renderer.LoadShaders();
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        // Handle mouse movement
        playerController.ProcessMouseMovement(e);
    }
    
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        // Process player input and update the game state
        playerController.Update();
        

        // Update the world or perform other game logic

        // Redraw the frame
        //Invalidate();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        // Clear the screen
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Setup the view matrix based on the player camera
        Matrix4 viewMatrix = camera.GetViewMatrix();

        // Render the world
        renderer.RenderWorld(world);

        // Swap buffers
        SwapBuffers();
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key == Keys.Space)
        {
            var air = world.GetCount(BlockType.Air);
            var grass = world.GetCount(BlockType.Grass);
            var stone = world.GetCount(BlockType.Stone);
            var blockCounts = $"Air: {air}, Grass: {grass}, Stone: {stone}";
            Console.WriteLine(blockCounts);
        }
        if (e.Key == Keys.Escape)
        {
            Close();
        }
        
        playerController.ProcessKeyboardInput(e);
    }


    protected override void Dispose(bool disposing)
    {
        Close();
        base.Dispose(disposing);
    }
}



public class PerlinNoiseGenerator : IWorldGenerator
{
    public void GenerateWorld(WorldOptions options, ref World world)
    {
        int worldSizeX = options.GetWorldSizeX();
        int worldSizeZ = options.GetWorldSizeZ();

        float[,] heightMap = GenerateHeightMap(worldSizeX, worldSizeZ, options);

        heightMap.Dump();

        for (int x = 0; x < worldSizeX; x++)
        {
            for (int z = 0; z < worldSizeZ; z++)
            {
                int height = (int)MathF.Floor(heightMap[x, z] * 256);

                for (int y = 0; y < height; y++)
                {
                    Vector3 blockPos = new Vector3(x, y, z);
                    Block? block = world.GetBlock(blockPos);

                    if (y < height)
                    {
                        if (y >= height - 1)
                        {
                            block = new Block { Type = BlockType.Grass, Position = blockPos };
                        }
                        else if (y < height - 1)
                        {
                            block = new Block { Type = BlockType.Stone, Position = blockPos };
                        }
                    }
                    else
                    {
                        block = new Block { Type = BlockType.Air, Position = blockPos };
                    }

                    world.SetBlock(blockPos, block.Value);
                }
            }
        }
    }

    private float[,] GenerateHeightMap(int sizeX, int sizeZ, WorldOptions options)
    {
        float[,] heightMap = new float[sizeX, sizeZ];
        float halfSizeX = sizeX / 2f;
        float halfSizeZ = sizeZ / 2f;

        Noise noise = new Noise(options.Seed);

        var perlinValues = new float[sizeX, sizeZ];
        var noiseHeights = new float[sizeX, sizeZ];

        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                float sampleX = (x - halfSizeX) / options.NoiseScale;
                float sampleZ = (z - halfSizeZ) / options.NoiseScale;

                float amplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0f;

                for (int octave = 0; octave < options.Octaves; octave++)
                {
                    float perlinValue = noise.Evaluate(sampleX * frequency, sampleZ * frequency);

                    perlinValues[x, z] = perlinValue;

                    noiseHeight += perlinValue * amplitude;
                    noiseHeights[x, z] = noiseHeight;
                    amplitude *= options.Persistence;
                    frequency *= options.Lacunarity;

                }

                heightMap[x, z] = noiseHeight;
            }
        }

        noiseHeights.Dump();

        perlinValues.Dump();

        return heightMap;
    }
}

public class PlayerController
{
    private readonly PlayerCamera camera;
    private readonly float movementSpeed = 10.1f;

    public PlayerController(PlayerCamera camera)
    {
        this.camera = camera;
    }

    public void Update()
    {
        Util.ClearResults();
        camera.GetViewMatrix().Dump("Camera Matrix");
        camera.GetPosition().Dump("Camera Position");
        camera.Forward.Dump("Camera Forward");
        camera.Right.Dump("Camera Right");
    }

    public void Move(Vector3 moveDirection)
    {
        camera.Move(moveDirection * movementSpeed);
    }

    public void ProcessKeyboardInput(KeyboardKeyEventArgs e)
    {
        Vector3 moveDirection = Vector3.Zero;

        switch (e.Key)
        {
            case Keys.W:
                moveDirection += camera.Forward;
                break;
            case Keys.S:
                moveDirection -= camera.Forward;
                break;
            case Keys.A:
                moveDirection -= camera.Right;
                break;
            case Keys.D:
                moveDirection += camera.Right;
                break;
            case Keys.LeftShift:
                moveDirection += Vector3.UnitY;
                break;
            case Keys.LeftControl:
                moveDirection -= Vector3.UnitY;
                break;
        }

        Move(moveDirection);
    }

    private void ProcessMouseInput()
    {
        //        MouseState mouseState = Mouse.GetState();
        //
        //        float mouseDeltaX = mouseState.XDelta;
        //        float mouseDeltaY = mouseState.YDelta;
        //
        //        camera.Rotate(mouseDeltaX, mouseDeltaY);
    }

    internal void ProcessMouseMovement(MouseMoveEventArgs e)
    {
        camera.Rotate(e.DeltaX, e.DeltaY);
        //e.Delta.Dump("Mouse Delta");
    }
}

public class PlayerCamera
{
    private Vector3 position;
    private Vector3 rotation;

    public Vector3 Forward { get; private set; }
    public Vector3 Right { get; private set; }

    public PlayerCamera(Vector3 position, Vector3 rotation)
    {
        this.position = position;
        this.rotation = rotation;

        UpdateVectors();
    }

    public void Move(Vector3 offset)
    {
        position += offset;
    }

    public void Rotate(float deltaX, float deltaY)
    {
        rotation.X -= deltaY;
        rotation.Y -= deltaX;

        // Clamp vertical rotation to prevent camera flipping
        rotation.X = MathHelper.Clamp(rotation.X, -90f, 90f);

        UpdateVectors();
    }

    public Vector3 GetPosition() => position;

    private void UpdateVectors()
    {
        Matrix4 rotationMatrix = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                                 Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));

        Vector3 direction = -Vector3.UnitZ;
        direction = Vector3.Transform(direction, Quaternion.FromMatrix(new Matrix3(rotationMatrix)));

        Vector3 up = Vector3.UnitY;
        up = Vector3.Transform(up, Quaternion.FromMatrix(new Matrix3(rotationMatrix)));

        Forward = direction;
        Right = Vector3.Cross(Forward, Vector3.UnitY).Normalized();
    }

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(position, position + Forward, Vector3.UnitY);
    }
}

public class World
{
    private Chunk[,] chunks;
    private int worldSizeX;
    private int worldSizeZ;

    public World(int sizeX, int sizeZ)
    {
        worldSizeX = sizeX;
        worldSizeZ = sizeZ;
        chunks = new Chunk[sizeX, sizeZ];
        InitializeChunks();
    }
    
    public int GetCount(BlockType blockType)
    {
        return chunks.Cast<Chunk>().SelectMany(chunk => chunk.GetBlocks()).Count(block => block.Type == blockType);
    }

    private void InitializeChunks()
    {
        for (int x = 0; x < worldSizeX; x++)
        {
            for (int z = 0; z < worldSizeZ; z++)
            {
                var chunk = new Chunk()
                {
                    Position = new Vector3(x, 0, z)
                };
                
                chunks[x, z] = chunk;
            }
        }
    }

    public IEnumerable<Chunk> GetChunks() => chunks.Cast<Chunk>();

    public int GetWorldSizeX() => worldSizeX;
    public int GetWorldSizeZ() => worldSizeZ;

    public Chunk GetChunk(Vector3 position)
    {
        int chunkX = (int)position.X / 16;
        int chunkZ = (int)position.Z / 16;
        return chunks[chunkX, chunkZ];
    }

    public Block? GetBlock(Vector3 position)
    {
        Chunk chunk = GetChunk(position);
        Vector3 blockPos = new Vector3(position.X % 16, position.Y, position.Z % 16);
        return chunk.GetBlock(blockPos);
    }

    public void SetBlock(Vector3 position, Block block)
    {
        Chunk chunk = GetChunk(position);
        Vector3 blockPos = new Vector3(position.X % 16, position.Y, position.Z % 16);
        chunk.SetBlock(blockPos, block);
    }
}

public class WorldOptions
{
    public int Seed { get; set; }
    public float NoiseScale { get; set; }
    public int Octaves { get; set; }
    public float Persistence { get; set; }
    public float Lacunarity { get; set; }

    public WorldOptions(int seed, float noiseScale, int octaves, float persistence, float lacunarity)
    {
        Seed = seed;
        NoiseScale = noiseScale;
        Octaves = octaves;
        Persistence = persistence;
        Lacunarity = lacunarity;
    }

    internal int GetWorldSizeX()
    {
        return 10 * Chunk.ChunkSizeX;
    }

    internal int GetWorldSizeZ()
    {
        return Chunk.ChunkSizeZ;
    }
}


public class Noise
{
    private readonly Random _random;

    public Noise(int seed)
    {
        _random = new Random(seed);
    }

    public float Evaluate(float x, float z)
    {
        return PerlinNoise(x, z);
    }

    private float PerlinNoise(float x, float z)
    {
        int ix = (int)x;
        int iz = (int)z;

        float fx = x - ix;
        float fz = z - iz;

        float u = Fade(fx);
        float v = Fade(fz);

        int a = _random.Next();
        int b = _random.Next();
        int aa = _random.Next();
        int ab = _random.Next();
        int ba = _random.Next();
        int bb = _random.Next();

        float x1 = Lerp(Grad(a, fx, fz), Grad(b, fx - 1, fz), u);
        float x2 = Lerp(Grad(aa, fx, fz - 1), Grad(ab, fx - 1, fz - 1), u);
        float y1 = Lerp(x1, x2, v);

        x1 = Lerp(Grad(ba, fx, fz), Grad(bb, fx - 1, fz), u);
        x2 = Lerp(Grad(ba, fx, fz - 1), Grad(bb, fx - 1, fz - 1), u);
        float y2 = Lerp(x1, x2, v);

        return (y1 + y2) / 2f;
    }

    private float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }

    private float Grad(int hash, float x, float z)
    {
        int h = hash & 15;
        float u = h < 8 ? x : z;
        float v = h < 4 ? z : (h == 12 || h == 14 ? x : 0);
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
}

public class Chunk
{
    private Block[,,] blocks;

    public Chunk()
    {
        blocks = new Block[ChunkSizeX, ChunkSizeY, ChunkSizeZ];
        InitializeBlocks();
    }

    private void InitializeBlocks()
    {
        for (int i = 0; i < Chunk.ChunkSizeX; i++)
        {
            for (int j = 0; j < Chunk.ChunkSizeY; j++)
            {
                for (int k = 0; k < Chunk.ChunkSizeZ; k++)
                {
                    blocks[i, j, k] = new Block();
                }
            }
        }
    }

    public Vector3 Position { get; set; }

    public Block? GetBlock(Vector3 position)
    {
        try
        {
            return blocks[(int)position.X, (int)position.Y, (int)position.Z];
        }
        catch (Exception ex)
        {
            //ex.Dump();
            return default;
        }
    }

    public void SetBlock(Vector3 position, Block block)
    {
        try
        {
            blocks[(int)position.X, (int)position.Y, (int)position.Z] = block;
        }
        catch (Exception ex)
        {
            //ex.Dump();
        }
    }

    internal Block GetBlock(int x, int y, int z)
    {
        return blocks[x, y, z];
    }

    public int GetSizeX() => blocks.GetLength(0);
    public int GetSizeY() => blocks.GetLength(1);
    public int GetSizeZ() => blocks.GetLength(2);

    internal IEnumerable<Block> GetBlocks()
    {
        foreach (Block block in blocks)
        {
            yield return block;
        }
    }

    public static int ChunkSizeX = 16;
    public static int ChunkSizeY = 256;
    public static int ChunkSizeZ = 16;
}

// Enum to represent different types of blocks
public enum BlockType
{
    Air,
    Dirt,
    Stone,
    Grass,
    Wood,
    // Add more block types as needed
}

public interface IWorldGenerator
{
    void GenerateWorld(WorldOptions options, ref World world);
}

// Struct to represent a block in the world
public struct Block
{
    public BlockType Type { get; set; }
    public Vector3 Position { get; set; }

    public Block(BlockType type, Vector3 position)
    {
        Type = type;
        Position = position;
    }
}

public interface IRenderer
{
    void RenderWorld(World world);
    public void LoadShaders();
}

public class OpenTkRenderer : IRenderer
{
    private ShaderManager shaderManager;
    private PlayerCamera _camera;

    public OpenTkRenderer(PlayerCamera camera)
    {
        _camera = camera;
        shaderManager = new ShaderManager();
    }

    public void RenderWorld(World world)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        shaderManager.UseShaderProgram();

        // Perform rendering operations here
        RenderBlocks(world);
    }

    public void LoadShaders()
    {
        string vertexShaderSource = Shaders.VertexShader;
        string fragmentShaderSource = Shaders.FragmentShader;

        shaderManager.LoadShaders(vertexShaderSource, fragmentShaderSource);
    }

    private void RenderBlocks(World world)
    {
        // Get the block size and iterate over the world's chunks
        int blockSize = 1;
        foreach (Chunk chunk in world.GetChunks())
        {
            // Iterate over the blocks in the chunk
            for (int x = 0; x < Chunk.ChunkSizeX; x++)
            {
                for (int y = 0; y < Chunk.ChunkSizeZ; y++)
                {
                    for (int z = 0; z < chunk.GetSizeZ(); z++)
                    {
                        Block block = chunk.GetBlock(x, y, z);

                        // Skip rendering if the block is air
                        if (block.Type == BlockType.Air)
                            continue;

                        // Calculate the world position of the block
                        Vector3 blockPosition = new Vector3(
                            chunk.Position.X * Chunk.ChunkSizeX + x,
                            chunk.Position.Y * Chunk.ChunkSizeY + y,
                            chunk.Position.Z * Chunk.ChunkSizeZ + z
                        );

                        // Perform rendering operations for the block
                        RenderBlock(block, blockPosition, blockSize);
                    }
                }
            }
        }
    }

    private void RenderBlock(Block block, Vector3 position, int blockSize)
    {
        // Calculate the scaling factor based on the block size
        float scale = blockSize / 2f;

        // Calculate the model matrix for the block
        Matrix4 modelMatrix = Matrix4.CreateScale(scale) *
                              Matrix4.CreateTranslation(position);

        // Pass the model matrix to the shader program
        int modelMatrixLocation = GL.GetUniformLocation(shaderManager.GetShaderProgram(), "modelMatrix");
        GL.UniformMatrix4(modelMatrixLocation, false, ref modelMatrix);

        // Pass camera to GL
        //int cameraLocation = GL.GetUniformLocation(shaderManager.GetShaderProgram(), "camera");
        //var viewMatrix = _camera.GetViewMatrix();
        //GL.UniformMatrix4(cameraLocation, false, ref viewMatrix);

        // Set the color based on the block type
        Vector3 color = GetBlockColor(block.Type);
        int colorLocation = GL.GetUniformLocation(shaderManager.GetShaderProgram(), "blockColor");
        GL.Uniform3(colorLocation, color);

        // Perform the actual drawing
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }

    Vector3 GetBlockColor(BlockType type)
    {
        switch (type)
        {
            case BlockType.Dirt:
                return new Vector3(0.6f, 0.4f, 0.2f);
            case BlockType.Wood:
                return new Vector3(0.2f, 0.8f, 0.2f);
            case BlockType.Stone:
                return new Vector3(0.5f, 0.5f, 0.5f);
            default:
                return Vector3.Zero;
        }
    }
}

public class ShaderCompilationException : Exception
{
    public ShaderCompilationException(string message) : base(message)
    {

    }
}

public class ShaderLinkingException : Exception
{
    public ShaderLinkingException(string message) : base(message)
    {

    }
}

public class ShaderManager
{
    private int shaderProgram;

    public void LoadShaders(string vertexShaderSource, string fragmentShaderSource)
    {
        int vertexShader = CompileShader(vertexShaderSource, ShaderType.VertexShader);
        int fragmentShader = CompileShader(fragmentShaderSource, ShaderType.FragmentShader);

        shaderProgram = LinkShaders(vertexShader, fragmentShader);

        // Cleanup: Delete the individual shaders
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void UseShaderProgram()
    {
        GL.UseProgram(shaderProgram);
    }

    public int GetShaderProgram()
    {
        return shaderProgram;
    }

    private int CompileShader(string shaderSource, ShaderType shaderType)
    {
        int shader = GL.CreateShader(shaderType);
        GL.ShaderSource(shader, shaderSource);
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int compileStatus);
        if (compileStatus != 1)
        {
            string log = GL.GetShaderInfoLog(shader);
            throw new ShaderCompilationException($"Shader compilation failed: {log}");
        }

        return shader;
    }

    private int LinkShaders(int vertexShader, int fragmentShader)
    {
        int program = GL.CreateProgram();
        GL.AttachShader(program, vertexShader);
        GL.AttachShader(program, fragmentShader);
        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int linkStatus);
        if (linkStatus != 1)
        {
            string log = GL.GetProgramInfoLog(program);
            throw new ShaderLinkingException($"Shader program linking failed: {log}");
        }

        return program;
    }
}

public static class PerlinNoise
{
    private static readonly int[] PermutationTable =
    {
    151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225,
    140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148,
    247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32,
    57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175,
    74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122,
    60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54,
    65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169,
    200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3,
    64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85,
    212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170,
    213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43,
    172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185,
    112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191,
    179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31,
    181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150,
    254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195,
    78, 66, 215, 61, 156, 180
};

    private static readonly int[] Permutation = new int[512];
    private static readonly int[] P = new int[256];

    static PerlinNoise()
    {
        for (int i = 0; i < 256; i++)
        {
            P[i] = PermutationTable[i % 256];
            Permutation[i] = P[i];
            Permutation[i + 256] = P[i];
        }
    }

    public static float Generate(float x, float y, float z)
    {
        int xi = (int)x & 255;
        int yi = (int)y & 255;
        int zi = (int)z & 255;

        float xf = x - (int)x;
        float yf = y - (int)y;
        float zf = z - (int)z;

        float u = Fade(xf);
        float v = Fade(yf);
        float w = Fade(zf);

        int aaa = Permutation[Permutation[Permutation[xi] + yi] + zi];
        int aba = Permutation[Permutation[Permutation[xi] + Increment(yi)] + zi];
        int aab = Permutation[Permutation[Permutation[xi] + yi] + Increment(zi)];
        int abb = Permutation[Permutation[Permutation[xi] + Increment(yi)] + Increment(zi)];
        int baa = Permutation[Permutation[Permutation[Increment(xi)] + yi] + zi];
        int bba = Permutation[Permutation[Permutation[Increment(xi)] + Increment(yi)] + zi];
        int bab = Permutation[Permutation[Permutation[Increment(xi)] + yi] + Increment(zi)];
        int bbb = Permutation[Permutation[Permutation[Increment(xi)] + Increment(yi)] + Increment(zi)];

        float x1 = Lerp(Grad(aaa, xf, yf, zf), Grad(baa, xf - 1, yf, zf), u);
        float x2 = Lerp(Grad(aba, xf, yf - 1, zf), Grad(bba, xf - 1, yf - 1, zf), u);
        float y1 = Lerp(x1, x2, v);

        x1 = Lerp(Grad(aab, xf, yf, zf - 1), Grad(bab, xf - 1, yf, zf - 1), u);
        x2 = Lerp(Grad(abb, xf, yf - 1, zf - 1), Grad(bbb, xf - 1, yf - 1, zf - 1), u);
        float y2 = Lerp(x1, x2, v);

        return (Lerp(y1, y2, w) + 1) * 0.5f;
    }

    private static int Increment(int num)
    {
        num++;
        return num & 255;
    }

    private static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }

    private static float Grad(int hash, float x, float y, float z)
    {
        int h = hash & 15;
        float u = h < 8 ? x : y;
        float v = h < 4 ? y : h == 12 || h == 14 ? x : z;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
}
public static class Shaders
{
    public static readonly string VertexShader = @"
    #version 330 core

    layout(location = 0) in vec3 vertexPosition;
    layout(location = 1) in vec3 vertexColor;

    uniform mat4 modelMatrix;
    uniform mat4 viewMatrix;
    uniform mat4 projectionMatrix;

    out vec3 color;

    void main()
    {
        gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(vertexPosition, 1.0);
        color = vertexColor;
    }
";

    public static readonly string FragmentShader = @"
    #version 330 core

    in vec3 color;
    out vec4 fragColor;

    void main()
    {
        fragColor = vec4(color, 1.0);
    }
";
}
