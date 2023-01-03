<Query Kind="Program">
  <NuGetReference>MonoGame.Framework.DesktopGL</NuGetReference>
  <Namespace>Microsoft.Xna.Framework</Namespace>
  <Namespace>Microsoft.Xna.Framework.Audio</Namespace>
  <Namespace>Microsoft.Xna.Framework.Content</Namespace>
  <Namespace>Microsoft.Xna.Framework.Design</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics.PackedVector</Namespace>
  <Namespace>Microsoft.Xna.Framework.Input</Namespace>
  <Namespace>Microsoft.Xna.Framework.Input.Touch</Namespace>
  <Namespace>Microsoft.Xna.Framework.Media</Namespace>
  <Namespace>MonoGame.Framework.Utilities</Namespace>
  <Namespace>MonoGame.Framework.Utilities.Deflate</Namespace>
  <Namespace>MonoGame.OpenGL</Namespace>
  <Namespace>NVorbis</Namespace>
  <Namespace>NVorbis.Ogg</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>


























void Main()
{
}

    public class Block
    {
        // The texture for the block
        public Texture2D Texture { get; set; }

        // The color of the block
        public Color Color { get; set; }

        // The block's position in the world
        public Vector3 Position { get; set; }

        // The block's bounding box (used for collision detection)
        public BoundingBox BoundingBox { get; set; }

        // The block's durability (how many hits it takes to destroy the block)
        public int Durability { get; set; }

        // The block's current hit points (decreases as the block is hit)
        public int HitPoints { get; set; }

        // A flag indicating whether the block is solid (can be collided with)
        public bool IsSolid { get; set; }

        // A flag indicating whether the block is visible (should be drawn)
        public bool IsVisible { get; set; }

        // A flag indicating whether the block is currently being destroyed
        public bool IsDestroying { get; set; }

        // Constructor for the block class
        public Block(Texture2D texture, Color color, Vector3 position, int durability, bool isSolid)
        {
            Texture = texture;
            Color = color;
            Position = position;
            Durability = durability;
            HitPoints = durability;
            IsSolid = isSolid;
            IsVisible = true;
            IsDestroying = false;
            BoundingBox = new BoundingBox(position - Vector3.One * 0.5f, position + Vector3.One * 0.5f);
        }

        // Method for updating the block (called each frame)
        public void Update(GameTime gameTime)
        {
            // Update the block's bounding box based on its position
            BoundingBox = new BoundingBox(Position - Vector3.One * 0.5f, Position + Vector3.One * 0.5f);
        }

        // Method for drawing the block (called each frame)
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(Texture, Position, Color);
            }
        }
    }
