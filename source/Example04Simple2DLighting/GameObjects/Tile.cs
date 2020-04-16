using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Example04Simple2DLighting.GameObjects
{
    public class Tile : GameObject
    {
        private VirtualTexture2D _texture;

        public Tile(Vector2 position, VirtualTexture2D texture)
        {
            Position = position;
            _texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _texture.Render(spriteBatch, Position, Color.White);
        }
    }
}
