#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor = float4(0.5f, 0.5f, 0.5f, 1.0f);
float AmbientIntensity = 1.0f;

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

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	return output;
};

float4 PixelShaderFunction(PixelInput input) : COLOR0
{
	return AmbientColor * AmbientIntensity;
};

technique Ambient
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
};
