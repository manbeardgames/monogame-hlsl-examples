#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//	This is the amount of time that has elapsed in the game in seconds.
extern float ElapsedTime;

//	This is the first character texture that is applied to the shader
extern Texture2D Character01;

//	This is the second character texture that is applied to the shader
extern Texture2D Character02;

//	This is the sampler for the first character texture.
sampler2D Character01Sampler = sampler_state
{
	Texture = <Character01>;
};

//	This is the sampler for the second character texture
sampler2D Character02Sampler = sampler_state
{
	Texture = <Character02>;
};


struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};


float4 MainPS(VertexShaderOutput input) : COLOR
{
	//	First we're going to get the pixel color sample from the first texture
	float4 pixelColor1 = tex2D(Character01Sampler, input.TextureCoordinates);

	//	Next we're going to get the pixel color sample from the second texture at
	//	the same coordinates
	float4 pixelColor2 = tex2D(Character02Sampler, input.TextureCoordinates);

	//	We can then lerp between the two by using a Sien wave.  If you recall,
	//	a Sine wave starts at 0 and goes up to 1, then back to 0, then to -1,
	//	the back to 0 and repeats, like the following
	//	
    //	  1       .      
	//	  |    .     .   
    //	  |  .         . 
    //	  | .           .
    //	  -.-------------.-------------.
    //	  |               .           .
    //	  |                .         .
    //	  |                  .     .
    //	 -1                     .
	//
	//	We don't want it to go negative for the purpose of lerping so we're also
	//	go to be getting the absolute value of it each time.
	//	
	//	We can lerp the two pixel colors together by using the lerp function
	//	https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-lerp
	//
	//	We can get the sine value by using the sin function
	//	https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-sin
	//
	//	And we can get the absolute value by using the abs function
	//	https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-abs
	float4 result = lerp(pixelColor1, pixelColor2, abs(sin(ElapsedTime)));

	//	And we return the result
	return result;
}

technique SpriteBlending
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};