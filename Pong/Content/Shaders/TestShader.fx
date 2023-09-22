#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#endif

float4x4 WorldMatrix;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;

float4 AmbientColor = float4(0.5f, 0.5f, 0.5f, 1.0f);

struct VertexInput
{
	float4 Position: POSITION0;
};

struct PixelInput
{
	float4 Position: POSITION0;
};

PixelInput VertexShaderFunction(VertexInput input)
{
	PixelInput output = (PixelInput)0;

	float4 worldPosition = mul(input.Position, WorldMatrix);
	float4 viewPosition = mul(worldPosition, ViewMatrix);
	output.Position = mul(viewPosition, ProjectionMatrix);

	return output;
};

float4 PixelShaderFunction(PixelInput input) : COLOR0
{
	return AmbientColor;
};

technique Ambient
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
};
