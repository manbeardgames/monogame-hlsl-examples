//  ---------------------------------------------------------------------------
//  Example 01: Appling a Shader
//
//  In this example, we look at how to apply a shader to a spritebatch.
//  The shader we apply is a super simple basic shader that does nothing actually.
//  It just renders the texture normally.  
//
//  Be sure to look at the shader effect file in Content\BasicShader.fx.  I've gon
//  through and ugly commented the file to explain a few things that are part of 
//  a basic shader.
//
//  In this example, and all examples that are created, the texture is rendered
//  twice.  The first rendering uses the shader effect.  The second render does not
//  use the effect, so you can see them side by side for comparison when running the
//  game.
//  ---------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Example01ApplyShader
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

        //  This is the Texture2D we'll use to render our character.
        private Texture2D _characterTexture;

        //  This is the basic shader effect we'll use for this example.
        private Effect _basicShader;

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
            _characterTexture = Content.Load<Texture2D>(@"character");

            //  Load our shader effect in here
            _basicShader = Content.Load<Effect>(@"BasicShader");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //  To use the shader effect, all we have to do is load it into the spriteBatch.Begin()'s parameters
            //  Since this shader doesn't do anything special and jsut renders the sprite normaly, when you 
            //  run the game, it will look no diferent than the one rendered below without the effect.
            //
            spriteBatch.Begin(effect: _basicShader);
            spriteBatch.Draw(texture: _characterTexture,
                             position: new Vector2(440, 180),
                             sourceRectangle: null,
                             color: Color.White,
                             rotation: 0.0f,
                             origin: _characterTexture.Bounds.Size.ToVector2() * 0.5f,
                             scale: 1.0f,
                             effects: SpriteEffects.None,
                             layerDepth: 0.0f);
            spriteBatch.End();

            //  This will draw the sprite without a the shader so we can see them side by side.
            //  This one will be rendered to the left of the sprite with the shader.
            spriteBatch.Begin();
            spriteBatch.Draw(texture: _characterTexture,
                             position: new Vector2(200, 180),
                             sourceRectangle: null,
                             color: Color.White,
                             rotation: 0.0f,
                             origin: _characterTexture.Bounds.Size.ToVector2() * 0.5f,
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
