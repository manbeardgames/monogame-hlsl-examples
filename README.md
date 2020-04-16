# MonoGame HLSL Shader Examples
This repository is a collection of MonoGame projects that show examples on using HLSL shaders with MonoGame.  Each example contains comment information in the source code to explain how the example works and what is happening as a way of showing how to use shaders.

## Using This Repository
To use this repository, first clone the repo locally to your machine.

```sh
git clone https://github.com/manbeardgames/monogame-hlsl-examples.git
```

Once cloned, open the **source\ShaderExamples.sln** solution file in Visual Studio. Each example that is written is a seperate project in the solution.  Each project was created using the MonoGame Cross Platform Desktop Project template.  

Inside each project, locate the Game1.cs file to look at the code from the C#/MonoGame side.  To view the HLSL
shader code, locate the appropriate .fx file inside the **Content** folder for each project.  

I recommend using Visual Studio Code to view and edit any HLSL files since Visual Studio does not do formatting and syntax highlight properly. 

## Explination Of The Examples
The following provides an explination of each example project included.

### Example 01: Apply Shader
In the **Example01ApplyShader** project, we do nothing special other than just appling a shader.  The whole purpose of this example is to show how to use a shader in MonoGame by supplying it as a paramater in `spriteBatch.Begin()`.  The actual shader file that was created for this example does nothing special, it literally just draws the texture/sprite.  The shader file is HEAVILY commented though to the point of ugly commenting to explain some basics and structure of a shader file.

### Example 02: Passing Values To Shader
In the **Example02PassingValuesToShader** project, an example is given to show how to pass a value from the game code to the shader code.  This is useful to set things like which texture to use, timing values, etc.  Be sure to read the information in both the Game1.cs file and the shader .fx file

### Example 03: Using Multiple Textures
In the **Example03UsingMultipleTextures** project, an example is given on passing multiple textures to a shader to use.  In this example in particular, we pass two textures to the shader, and using a a timing property with a sine wave, we blend back and forth between the two textures over time.  Be sure to read the information in both the Game1.cs file and the shader.fx file.

### Example 04: Simple 2D Lighting
In the **Example04Simple2DLighting** project, an example is given on how to create 2D lighting.  This is a super
simple method that uses a trick to create the illusion of lighting. We make use of multiple render targets, one for where the lighting is and one to render the game to, then we blend them together using the shader. This is not useful for creating realtime 2D shadow efects, but is a useful cheap trick for givin the illuision of a light source for things like torches and fires.

# License
All code and images written and used for this project are free and open sourced under the MIT License, unless otherwise stated below.  See the LICENSE file for more information.

The character, character01, and character02 images used in Example project 01, 02, and 03 are from http://www.kenney.nl and are licensed seperatly under CC0 license.  See the ARTWORKLICENSE file for more information.
