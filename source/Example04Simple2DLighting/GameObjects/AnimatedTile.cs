using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Example04Simple2DLighting.GameObjects
{
    public class AnimatedTile : GameObject
    {
        private VirtualTexture2D[] _frames;
        private float _delay;
        private int _currentFrame;
        private float _timer;

        public AnimatedTile(VirtualTexture2D[] frames, float delay, Vector2 position)
        {
            Position = position;
            _frames = frames;
            _delay = delay;
            _currentFrame = 0;
        }

        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_timer >= _delay)
            {
                _timer = 0.0f;
                _currentFrame++;
                if(_currentFrame >= _frames.Length)
                {
                    _currentFrame = 0;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _frames[_currentFrame].Render(spriteBatch, Position);
        }
    }
}
