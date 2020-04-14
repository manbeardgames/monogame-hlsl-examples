//  ---------------------------------------------------------------------------
//  Example 03: Using Multiple Textures
//
//  In this example, we'll look at using multiple textures in a shader when
//  rendering a single texture.  Why would you use multiple textures?  There
//  are lots of reasons, one of the most common is what's called using a normal map.
//
//  However, to keep things simple, in this example, we're just going to have the 
//  shader blend transition from one texture to another and back.  
//
//  Be sure to look at the shader effect file in Content\BlendShader.fx.
//
//  In this example, and all examples that are created, the texture is rendered
//  twice.  The first rendering uses the shader effect.  The second render does not
//  use the effect, so you can see them side by side for comparison when running the
//  game.
//  ---------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Example03UsingMultipleTextures
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //  The manager used to mange the the device that presents the graphics.
        private GraphicsDeviceManager graphics;

        //  The spritebatch used for rendering.
        private SpriteBatch spriteBatch;

        //  This is the first Texture2D we'll use when blending the render.
        private Texture2D _characterTexture01;

        //  This is the second Texture2D that we'll use when blending the render.
        private Texture2D _characterTexture02;

        //  This is the basic shader effect we'll use for this example.
        private Effect _blendShader;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 360;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //  Load our character texture in here.
            _characterTexture01 = Content.Load<Texture2D>(@"character01");

            //  Load the second character texture here.
            _characterTexture02 = Content.Load<Texture2D>(@"character02");

            //  Load our shader effect in here
            _blendShader = Content.Load<Effect>(@"BlendShader");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //  First let set the paramaters of our shader.  We need to give it two 
            //  tetures, one for Character01 and one for Character02.  We also need
            //  to give it a value for the ElapsedTime paramater, which is the amount
            //  of time that has passed in the game, in seconds.
            _blendShader.Parameters["Character01"].SetValue(_characterTexture01);
            _blendShader.Parameters["Character02"].SetValue(_characterTexture02);
            _blendShader.Parameters["ElapsedTime"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);

            //  Next lets load our shader in using the effect paramater in SpriteBatch.Begin().
            spriteBatch.Begin(effect: _blendShader);

            //  Then we draw the first character as normal.  As time goes on, the shader will
            //  lerp back and forth between the two textures, blending them as it goes.s
            spriteBatch.Draw(texture: _characterTexture01,
                             position: new Vector2(440, 180),
                             sourceRectangle: null,
                             color: Color.White,
                             rotation: 0.0f,
                             origin: _characterTexture01.Bounds.Size.ToVector2() * 0.5f,
                             scale: 1.0f,
                             effects: SpriteEffects.None,
                             layerDepth: 0.0f);
            spriteBatch.End();

            //  This will draw the sprite without a the shader so we can see them side by side.
            //  This one will be rendered to the left of the sprite with the shader.
            spriteBatch.Begin();
            spriteBatch.Draw(texture: _characterTexture01,
                             position: new Vector2(200, 180),
                             sourceRectangle: null,
                             color: Color.White,
                             rotation: 0.0f,
                             origin: _characterTexture01.Bounds.Size.ToVector2() * 0.5f,
                             scale: 1.0f,
                             effects: SpriteEffects.None,
                             layerDepth: 0.0f); spriteBatch.End();


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