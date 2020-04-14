//  ---------------------------------------------------------------------------
//  Example 02: Passing Variables To Shaders
//
//  In this example, we'll look at how to pass values to a shader.  The shader 
//  that we'll be using is a "tint" shader that takes in a color value and 
//  tints the render using that color.  
//
//  Be sure to look at the shader effect file in Content\TintShader.fx. 
//
//  In this example, and all examples that are created, the texture is rendered
//  twice.  The first rendering uses the shader effect.  The second render does not
//  use the effect, so you can see them side by side for comparison when running the
//  game.
//  ---------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Example02PassingValuesToShader
{
    public class Game1 : Game
    {
        //  The manager used to mange the the device that presents the graphics.
        private GraphicsDeviceManager _graphics;

        //  The spritebatch used for rendering.
        private SpriteBatch _spriteBatch;

        //  This is the Texture2D we'll use to render our character.
        private Texture2D _characterTexture;

        //  This is the tint shader effect we'll use for this example.
        private Effect _tintShader;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 360;
            _graphics.IsFullScreen = false;
            IsMouseVisible = true;
            _graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //  Load our character texture in here.
            _characterTexture = Content.Load<Texture2D>(@"character");

            //  Load our shader effect in here
            _tintShader = Content.Load<Effect>(@"TintShader");
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //  Let's pass the shader a color value so it knows what to use for the 
            //  'TintColor' property.  
            //  We can do this by using the "Parameters" property of the shader effect object.
            //  We just need to know tht ename of the property we want to set.
            //  In the shader, the TintColor property is a  float4.  The equivilant 
            //  of this is a Vector4 in C#. Luckily, we can convert a Color value to 
            //  a Vector4 eaisly. 
            //  Feel free to change the color value to play around with different colors.
            _tintShader.Parameters["TintColor"].SetValue(Color.Blue.ToVector4());

            //  To use the shader, load it into the spriteBatch.Begin()
            _spriteBatch.Begin(effect: _tintShader);
            _spriteBatch.Draw(texture: _characterTexture,
                             position: new Vector2(440, 180),
                             sourceRectangle: null,
                             color: Color.White,
                             rotation: 0.0f,
                             origin: _characterTexture.Bounds.Size.ToVector2() * 0.5f,
                             scale: 1.0f,
                             effects: SpriteEffects.None,
                             layerDepth: 0.0f);
            _spriteBatch.End();


            //  This will draw the sprite without a the shader so we can see them side by side.
            //  This one will be rendered to the left of the sprite with the shader.
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture: _characterTexture,
                             position: new Vector2(200, 180),
                             sourceRectangle: null,
                             color: Color.White,
                             rotation: 0.0f,
                             origin: _characterTexture.Bounds.Size.ToVector2() * 0.5f,
                             scale: 1.0f,
                             effects: SpriteEffects.None,
                             layerDepth: 0.0f); _spriteBatch.End();


            base.Draw(gameTime);
        }
    }















    // ------------------------------------------------------------------------
    //
    //
    //
    //
    //
    //
    //  Ignore the below. I've just moved the Program.cs class file into this file
    //  so that there is only one .cs file in the project to be concerned with.
    //  Don't edit below this line.
    // ------------------------------------------------------------------------
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() { using (var game = new Game1()) { game.Run(); } }
    }
}
