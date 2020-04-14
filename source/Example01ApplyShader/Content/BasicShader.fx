// ----------------------------------------------------------------------------
//	BasicShader.fx
//
//
//	This is an example of the default shader effect that is created by the 
//	MonoGame Content Pipeline Tool when you do Add > New Item > Effect
//
//	All shaders for MonoGame are written in HLSL (High Level Shader Language).
//	This is the shader langauge uses for DirectX applications.  If your MonoGame
//	project is using OpenGL (i.e. MonoGame Cross Platform Desktop Project), then
//	the Content Pipeline Tool converts the HLSL code you write here into GLSL.
//	Howveer, when writing the shader effect files, keep in mind we'll be doing it
//	in HLSL.
//
//
//	No changes have been made to this file except to add comments explaining
//	the different parts of the shader.
// ----------------------------------------------------------------------------




//	By appending the #if OPENGL directive here, when the shader is complied, if
//	you are using the MonoGame Cross Platform project type (OpenGL), then it will
//	define the SV_POSITION as a POSITIOn type, and it will also defin the vertex
//	shader model and pixel shader model as version 3.0. 
//
//	If not OPENGL (so DirectX) is being used, then vertex and shader model is 
//	defined as version 4.0 level 9.1  
//
//	The different model vesion used affect various things for the shader like
//	how many instructions the shader can process. For a comparison of the 
//	models you can view the wikipedia page here
// 	https://en.wikipedia.org/wiki/High-Level_Shading_Language#Shader_model_comparison
#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//	---------------------------------------------------------------------------
//	After defining the compiler constants above, the next thing you'll want to
//	define in the shader are the global object/variables.  These will be in scope
//	(accessable) throughout the entierty of the shader.  Just like how C# has
//	modifiers like public/private/protected, HLSL also has modifies like 
//	extern/uniform/static.  These will not be covered in this shader example, but
//	keep them in mind as we'll cover them in others.
//	---------------------------------------------------------------------------

//	This is the texture object of the sprite that is being passed 
//	through the shader.
//	https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/sm5-object-texture2d
Texture2D SpriteTexture;

//	This is the sampler object that that will be used to sample the texture and return
//	back the color information about a particular pixel.  This is a sampler2D object,
//	and we created it by setting it equal to a 'sampler_state' struct and set the
//	Texture property equal to the Texture2D from above. 
sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

//	---------------------------------------------------------------------------
//	After defining the global scoped objects/variables, we'll next define any
//	structs that are going to be used throughout the shader.  Structs here are 
//	similar in idea s they are in C#, they are a way of grouping data properties
//	together.
//	---------------------------------------------------------------------------

//	By default, you'll get this struct called VertexShaderOutput.  This will be 
//	the data struct that is returned back by the Vertex Shader function.  This struct
//	by default contains the data for the Position, Color, and UV Texture Coordinates
//	of the pixel that is going to be processed by the Fragment shader function.  
//	The default name of this struct as 'VertextShaderOutput' can be confusing, as 
//	I'll point out in a moment.  I normally change the name of this to something
//	more useful and less confusing, but will leave it as default for now.
struct VertexShaderOutput
{
	//	This can look a little confusing so I'll describe variable syntax here.
	//	This is a float4 type variable, called 'Position'.  The ': SV_POSITION' part
	//	is the semantic. 
	//	A way to think of "semantics" is like this.  This struct is the return type
	//	of the Vertex Shader Function.  One of the values the vertex shader function
	//	returns is the SV_POSITION as a float4 type.  By using a semantic, it tells
	//	the vertex shader function that the SV_POSITION value is 'linked' to this
	//	float4 variable called Position.
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

//	---------------------------------------------------------------------------
//	After your data structs are defined, the next thing to do is define the 
//	actual functions of the shader.  Typically, you would define them in this order
//
//	-	Custom functions that you'll need to call from other functions
//	-	Next define the Vertex Shader Function
//	-	Next define the Fragment Shader Function
//
//	In this basic shader that is created by default, we will not need to define
//	any custom functions.  This basic shader also does not have a Vertext Function
//	defined.  Since we are only focused on Fragment Shaders, we'll only be seeing,
//	the Fragment Shader Function here.  Just know that the typicaly order of 
//	functions is the order of the list above.
//	---------------------------------------------------------------------------

//	This is the Fragment Shader function.  By default, this function is called
//	MainPS.  It can actually be called anything you want, and I usually change this
//	to be called "FragmentShader".  If you want to change the name of this function,
//	be sure to also change the call to it in the Pass at the end.  There will be a commennt
//	below on where/how to do this.  For now, just know that MainPS by default is the
//	fragment shader
//
//	Lets cover the signature of a function here by looking at this function's signature
//	The typical signature is
//
//	return_type function_name(paramater_type parameter_name) : semantic_type
//	
//	So by that definition, we can read this function's signature as
//	This is a function called MainPS that takes a VertexShaderOutput parameter. It
//	returns back a float4 value, which should semantically be linked as the COLOR value
//	for the return.
float4 MainPS(VertexShaderOutput input) : COLOR
{
	//	So lets talk for a moment about what this function is actually doing.  
	//	This is the Fragment Shader Function.  It takes in the data that is output
	//	from the Vertex Shader Function, then this function is run once for every 
	//	pixel in the sprite.  So, if you have a 32x32 pixel sprite, that means
	//	this function will run 1,024 times, once for every pixel in the sprite.
	//	That's crazy right lol.
	//	So how do we know which pixel we are currently working on?  We can
	//	use the values in the input.TextureCoordinates property.  We'll go
	//	over this in another shader, but keep in mind now that's how we'll determine
	//	which pixel we're working on.

	//	For now, this basic effect shader just uses the tex2D function.
	//	the tex2D funciton takes in two parameters.  The first parameter is the
	//	sampler_state, which we have defined as a global variable.  The second 
	//	parameter is the UV coordinate within the texture that we want to sample.
	//	It returns back the color of the pixel at that coordinate.
	//	Basically it's saying "In this texture, at this coordinate, return back a
	//	float4 value that contains the RGBA color of the pixel at that coordinate
	//	in the texture."
	//
	//	It's then multiplied by input.Color.  This value "input.Color" is the Color
	//	value that you gave when you do SpriteBatch.Draw();.  It takes color of the pixel
	//	from the texture and multiplies it by the color you gave in SpriteBatch.Draw and
	//	returns back the new color.  The new color that is return back is the color the
	//	pixel will be when it is rendered.
	return tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
}

//	---------------------------------------------------------------------------
//	After all functions are defined above, the last thing we'll define in the
//	shader file are the "Techniques" and their "Passes".  Typically, for a
//	fragment shader, we'll almost always have only 1 technique, with only
//	1 pass.  
//
//	Think of a "Technique" as the "What" for a shader.  For example, in this
//	default shader, the technique is called "SpriteDrawing".  So this technique 
//	should only be concerned with drawing a sprite. 
//
//	Next, inside of a technique, we defined "Passes".  Think of a Pass as the
//	"How".  Each pass will set the property of VertexShader and/or PixelShader.
//	This tells that for that pass which function to call for the VertexShader and
//	which function to call for the PixelShader.  
//	As mentioned before, since we are only convering Pixel Shaders, we only have
//	to include the Pixel Shader in the pass below.
//	---------------------------------------------------------------------------
technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};