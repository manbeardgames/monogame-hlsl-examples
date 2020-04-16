// ----------------------------------------------------------------------------
//  Example 04: Simple 2D Lighting
//
//  In this example, we use a really simple trick to create the illusion of
//  lighting for a top down 2D game.  In the game screen that will be rendered,
//  it is a cave with four fires placed in the top-left, bottom-left, top-right,
//  and bottom-right of the room.
//
//  To create the "light" effect, we make use of a texture is a gradient circle
//  that is white in the middle, and gets darker towards it's edges.  We'll render
//  this texture at each location in the game screen where a light source would be,
//  but we'll be rendering it to it's own render target
//
//  Next, we'll render our game screen like normal to it's own render target.
//
//  Then we'll render the game's render target to the screen using our shader. 
//  We'll pass the shader the light's render target.  The shader will use the
//  pixel color data from the light render target to determine the color of the
//  pixel to use when rendering the game's render target.
//
//  A couple of things to note here.
//  1)  When rendering the light textures, the blend state is set to BlendState.AdditiceBlend
//  2)  When rendering the game screen and the final render target, the blend state is set 
//      to BlendState.AlphaBlend
//  3)  The texture used for the light mask is a 128x128 texture.  This means each "light"
//      source is only 128x128 in size in our game screen.  This is rather limiting, but thats
//      one of the downsides to using this lighting trick. You could create different sized
//      light mask textures to use for various light source sizes, or you could scale the
//      render of the light mask texture for light sources that are brighter.  
//  4)  The important bits for the shader example is in the Draw method. There is a lot of
//      code in LoadContent() that just sets up the tile map for rendering the tiles.  It's
//      not important for the shader example at all.
//
//  As always, be sure to check out the shader effect file at "Content\SimpleLightShader.fx"
// ----------------------------------------------------------------------------
using Example04Simple2DLighting.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Example04Simple2DLighting
{
    public class Game1 : Game
    {
        //  The manager used to mange the the device that presents the graphics.
        private GraphicsDeviceManager graphics;

        //  The spritebatch used for rendering.
        private SpriteBatch spriteBatch;

        //  This is the basic shader effect we'll use for this example.
        private Effect _simpleLightShader;

        //  This is the texture that will be rendered where light sources are placed.
        private Texture2D _lightMask;

        //  We'll render the light mask texture to this target 
        private RenderTarget2D _lightTarget;

        //  We'll render everything else to this target
        private RenderTarget2D _mainTarget;

        //  These are the tiles that will be rendered for the map
        private GameObject[] _tiles;

        //  These are the tiles that are the fire tiles that provide the light source.
        private GameObject[] _fireTiles;

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

        //  A lot of code is written here, but it's mostly not important at all
        //  for the shader relevant parts of this example.  This just loads the 
        //  textures and the effect file in from disk and setups the tiles to
        //  be rendered for the game screen.  
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //  First load the light mask texture
            _lightMask = Content.Load<Texture2D>(@"lightmask");

            //  Next, load the simple light shader effect
            _simpleLightShader = Content.Load<Effect>(@"SimpleLightShader");

            //  Next, create a new VirtualTexture2D for the texture atlas.
            VirtualTexture2D atlas = new VirtualTexture2D(Content.Load<Texture2D>(@"atlas"));

            //  The first three columns of the top row of the atlas are the left and right wall tile sprites
            int numWallTextures = 3;
            VirtualTexture2D[] wallTextures = new VirtualTexture2D[numWallTextures];
            for (int i = 0; i < numWallTextures; i++)
            {
                wallTextures[i] = new VirtualTexture2D(atlas, i * 32, 0, 32, 32);
            }

            //  The first three columns of the middle row of the atlas are the grass tiles sprites
            int numGroundTextures = 4;
            VirtualTexture2D[] groundTextures = new VirtualTexture2D[numGroundTextures];
            for (int i = 0; i < numGroundTextures; i++)
            {
                groundTextures[i] = new VirtualTexture2D(atlas, i * 32, 32, 32, 32);
            }

            //  The first three columns of the third row of the atlas are the top wall tile sprites
            int numTopWallTextures = 3;
            VirtualTexture2D[] topWallTextures = new VirtualTexture2D[numTopWallTextures];
            for (int i = 0; i < numTopWallTextures; i++)
            {
                topWallTextures[i] = new VirtualTexture2D(atlas, i * 32, 64, 32, 32);
            }

            //  The columns in the bottom row of the atals are the frames of the fire tile sprite
            int numFireFrames = 4;
            VirtualTexture2D[] fireFrames = new VirtualTexture2D[numFireFrames];
            for (int i = 0; i < numFireFrames; i++)
            {
                fireFrames[i] = new VirtualTexture2D(atlas, i * 32, 96, 32, 32);
            }

            //  Now that we've got the textures sorted for the actual tiles, let's create
            //  the tilemap
            Random random = new Random();
            int columnCount = 640 / 32;
            int rowCount = 360 / 32;
            _tiles = new GameObject[columnCount * rowCount];
            int tileIndex = 0;
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    //  Calculate the position of the tile
                    Vector2 tilePos = new Vector2(column * 32, row * 32);

                    //  If this is the first column, or the last column, or if this
                    //  is the last row, then we use the normal wall tile sprites.
                    if (column == 0 || column == columnCount - 1 || row == rowCount - 1)
                    {
                        _tiles[tileIndex] = new Tile(tilePos, wallTextures[random.Next(0, numWallTextures)]);
                    }
                    else
                    {
                        //  if this is the top row, then we use the top row wall tile sprites
                        if (row == 0)
                        {
                            _tiles[tileIndex] = new Tile(tilePos, topWallTextures[random.Next(0, numTopWallTextures)]);
                        }
                        else
                        {
                            //  Otherwise, this tile is one of the ones inside the wall, so we 
                            //  add a grass tile 
                            _tiles[tileIndex] = new Tile(tilePos, groundTextures[random.Next(0, numGroundTextures)]);
                        }

                    }

                    //  Increase the tileindex
                    tileIndex++;
                }
            }

            //  Create the four tourch animated tiles
            _fireTiles = new GameObject[4];
            float delay = 0.15f;
            _fireTiles[0] = _tiles[42] = new AnimatedTile(fireFrames, delay, new Vector2(64, 64));
            _fireTiles[1] = _tiles[57] = new AnimatedTile(fireFrames, delay, new Vector2(544, 64));
            _fireTiles[2] = _tiles[162] = new AnimatedTile(fireFrames, delay, new Vector2(64, 256));
            _fireTiles[3] = _tiles[177] = new AnimatedTile(fireFrames, delay, new Vector2(544, 256));

            //  Create our two render targets
            int width = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int height = GraphicsDevice.PresentationParameters.BackBufferHeight;
            _lightTarget = new RenderTarget2D(GraphicsDevice, width, height);
            _mainTarget = new RenderTarget2D(GraphicsDevice, width, height);

        }

        //  Nothing here is important for the example. This is just used to
        //  update the fire tiles so they animate.
        protected override void Update(GameTime gameTime)
        {
            //  Call update for each of the fire tiles so their animations play properly
            for (int i = 0; i < _fireTiles.Length; i++)
            {
                _fireTiles[i].Update(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // ----------------------------------------------------------------
            //  The first thing we want to do is render the light mask texture 
            //  at all positions that there is a light source in the game. In 
            //  your game, this could be where there are tourches on the walls, 
            //  fires on the ground, etc. 
            // ----------------------------------------------------------------

            //  We want to render these to the light source render target, so first
            //  we set that as the render target.
            GraphicsDevice.SetRenderTarget(_lightTarget);
            GraphicsDevice.Clear(Color.Black);


            spriteBatch.Begin(blendState: BlendState.Additive);
            //  Go through each of our fire tiles and render a copy of the light mask texture
            //  centered on the fire tile.
            for (int i = 0; i < _fireTiles.Length; i++)
            {
                //  The fire tile is 32x32, so to get the center position of the tile,
                //  we add the tile position with half it's width and height.
                Vector2 fireCenter = _fireTiles[i].Position + new Vector2(16, 16);

                //  Next we need to calcualte the position to render the light mask for this fire tile
                //  Since we're not using the "origin" parameter, we just offset the position by
                //  half the width and height of the light mask texture from the fire's center position.
                Vector2 lightMaskPos = fireCenter - (new Vector2(_lightMask.Width, _lightMask.Height) * 0.5f);

                //  And now draw the light mask texture. Remember, this is being draw to the lightTarget 
                //  render target.
                spriteBatch.Draw(_lightMask, lightMaskPos, Color.White);
            }

            //  Now that all of the light masks are rendered for the fires, let's do the same thing
            //  but based on the mouse's position on the screen. 
            Vector2 mosPosition = Mouse.GetState().Position.ToVector2();
            Vector2 mouseLightPos = mosPosition - (new Vector2(_lightMask.Width, _lightMask.Height) * 0.5f);
            spriteBatch.Draw(_lightMask, mouseLightPos, Color.White);
            spriteBatch.End();



            // ----------------------------------------------------------------
            //  Next, we want to render the game's screen to the "mainTarget" 
            //  render target.  Rendering this does not apply any of the lighting
            //  to the render.  
            // ----------------------------------------------------------------
            GraphicsDevice.SetRenderTarget(_mainTarget);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            //  Draws each of the tiles
            for (int i = 0; i < _tiles.Length; i++)
            {
                _tiles[i].Draw(spriteBatch);
            }
            spriteBatch.End();

            // ----------------------------------------------------------------
            //  Now that we've got the light render and the game render, we'll
            //  make use of the shader.  We're going to set render target to null,
            //  so that's we can render to the screen.  For the shader, we're
            //  going to tell it that the "MaskTexture" to use is the "lightTarget"
            //  render texture.  
            //
            //  Inside the shader, for each pixel, it's going to get the color of
            //  the pixel from the "mainTarget" texture, then get the color of the
            //  pixel at that position in the "lightTarget" texture, and blend
            //  them together.  Thus creating the effect of a lighting.
            // ----------------------------------------------------------------
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            _simpleLightShader.Parameters["MaskTexture"].SetValue(_lightTarget);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, effect: _simpleLightShader);
            spriteBatch.Draw(_mainTarget, Vector2.Zero, Color.Red);
            spriteBatch.End();
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
