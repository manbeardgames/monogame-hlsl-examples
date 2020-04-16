using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Example04Simple2DLighting
{
    public class VirtualTexture2D
    {
        public Texture2D Texture { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public VirtualTexture2D(Texture2D texture)
        {
            Texture = texture;
            SourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Width = SourceRectangle.Width;
            Height = SourceRectangle.Height;
        }

        public VirtualTexture2D(VirtualTexture2D existingInstance, int x, int y, int width, int height)
        {
            Texture = existingInstance.Texture;
            SourceRectangle = existingInstance.GetRelativeSourceRectangle(x, y, width, height);
            Width = width;
            Height = height;
        }

        public Rectangle GetRelativeSourceRectangle(int x, int y, int width, int height)
        {
            int baseX = SourceRectangle.X + x;
            int baseY = SourceRectangle.Y + y;
            int relativeX = MathHelper.Clamp(baseX, SourceRectangle.Left, SourceRectangle.Right);
            int relativeY = MathHelper.Clamp(baseY, SourceRectangle.Top, SourceRectangle.Bottom);
            int relativeWidth = Math.Max(0, Math.Min(baseX + width, SourceRectangle.Right) - baseX);
            int relativeHeight = Math.Max(0, Math.Min(baseY + height, SourceRectangle.Bottom) - baseY);
            return new Rectangle(relativeX, relativeY, relativeWidth, relativeHeight);
        }

        public void Render(SpriteBatch spriteBatch, Vector2 position)
        {
            Render(spriteBatch, position, Color.White);
        }

        public void Render(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(Texture, position, SourceRectangle, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
