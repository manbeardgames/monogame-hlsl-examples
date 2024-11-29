#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//	Variables can have storage class modifies given to them at the start
//	These are things like extern, uniform, and static.  There are many more
//	and you can read about them at https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-variable-syntax
//
//	For now, we're going to define a variable called "TintColor".  This will
//	be a float4 variable.  We're going to explicitly add the 'extern' modifer
//	to it.  'extern' means that the input value for this variable comes from
//	somewhere external to the shader.  This means it is a variable we can pass
//	the value of from within our C# code.  
//
//	It should also be noted that by default, all global variables are 'extern'.
//	So if no modifier is given, it is understood that it is extern.  However,
//	just like how in C# variables are private by default, even though the 
//	modifier is implicit, we should explicity state it so it clearly shows
//	the meaning.
extern float4 TintColor;

//	The texture that we'll be sampling
Texture2D SpriteTexture;

//	The sampler object we'll use to sample the texture
sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

//	This is the "output" from the vertex shader that we'll use as the
//	"input" to the pixel shader.  We don't have a vertex shader function
//	defined, so these values will be the default values where 
//
//	Position = The pixel location in screen space.
//	Color = color value given in the SpriteBatch.Draw() command
//	TextureCoordinates = UV position of the pixel in the texture.
//
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

//	This is our Pixel Shader function.  It takes in the output from the vertex shader
//	and returns back a float4 containing the color data for the pixel.
float4 MainPS(VertexShaderOutput input) : COLOR
{
	//	First we use the tex2D function to get the color of the pixel
	float4 pixelColor = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

	//	Next we'll multiply the pixelColor by the tint color that was passed int
	float4 tintedPixelColor = pixelColor * TintColor;

	//	And we return the value
	return tintedPixelColor;
}

//	Here, I've renamed the technique to match what it is actually doing
technique TintedSpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};