using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Example04Simple2DLighting.GameObjects
{
    public abstract class GameObject
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public float X
        {
            get => _position.X;
            set => _position.X = value;
        }

        public float Y
        {
            get => _position.Y;
            set => _position.Y = value;
        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
