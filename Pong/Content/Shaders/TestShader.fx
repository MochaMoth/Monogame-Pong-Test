float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 LightView;
float4x4 LightProjection;
float3 ViewPosition;
texture Base;
texture Normal;
texture Height;
texture Roughness;
texture ShadowMap;

float3 LightPosition = float3(10, 20, 3);
float LightPower = 1;
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 1;
float NormalIntensity = 1;
float HeightIntensity = 1;
float SpecularIntensity = 1;
float RoughnessIntensity = 1;
float RoughnessLift = 0;

sampler BaseTextureSampler = sampler_state {
    texture = <Base>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = mirror;
    AddressV = mirror;
};

sampler NormalTextureSampler = sampler_state {
    texture = <Normal>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = mirror;
    AddressV = mirror;
};

sampler HeightTextureSampler = sampler_state {
    texture = <Height>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = mirror;
    AddressV = mirror;
};

sampler RoughnessTextureSampler = sampler_state {
    texture = <Roughness>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = mirror;
    AddressV = mirror;
};

sampler ShadowTextureSampler = sampler_state {
    texture = <ShadowMap>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = clamp;
    AddressV = clamp;
};

// Utilities
float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);
}
// End Utilities

// Color Shader
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
    float3 Normal : NORMAL0;
    float4 VertexColor : COLOR0;
};

struct VertexToPixel
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float3 Position3D : TEXCOORD2;
    float4 VertexColor : COLOR0;
    float4 LightViewPosition : TEXCOORD3;
};

struct PixelToFrame
{
    float4 Color : COLOR0;
};

VertexToPixel VertexShaderFunction(VertexShaderInput input)
{
    VertexToPixel output = (VertexToPixel)0;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.Normal = normalize(mul(input.Normal, (float3x3)World));
    output.Position3D = worldPosition;
    output.TexCoord = input.TexCoord;
    output.VertexColor = input.VertexColor;
    output.LightViewPosition = mul(input.Position, mul(World, mul(LightView, LightProjection)));

    return output;
}

PixelToFrame PixelShaderFunction(VertexToPixel input)
{
    PixelToFrame output = (PixelToFrame)0;

    // Apply Shadow
    float2 ShadowTexCoords;
    ShadowTexCoords[0] = input.LightViewPosition.x / input.LightViewPosition.w / 2.0f + 0.5f;
    ShadowTexCoords[1] = -input.LightViewPosition.y / input.LightViewPosition.w / 2.0f + 0.5f;
    float4 shadowColor = tex2D(ShadowTextureSampler, ShadowTexCoords);

    // Apply Lighting
    float texHeight = tex2D(HeightTextureSampler, input.TexCoord) * HeightIntensity;
    input.Position3D += input.Normal * texHeight;

    float3 texNormal = tex2D(NormalTextureSampler, input.TexCoord) * NormalIntensity * input.Normal;
    float3 texRoughness = tex2D(RoughnessTextureSampler, input.TexCoord) * RoughnessIntensity + RoughnessLift;
    float3 viewVector = normalize(ViewPosition - input.Position3D);
    float3 reflectionVector = reflect(normalize(input.Position3D - LightPosition), normalize(texNormal));
    float specular = texRoughness * pow(saturate(dot(reflectionVector, viewVector)), SpecularIntensity);

    float diffuseLightingFactor = 0;
    if ((saturate(ShadowTexCoords).x == ShadowTexCoords.x) && (saturate(ShadowTexCoords).y == ShadowTexCoords.y))
    {
        float shadowDepth = tex2D(ShadowTextureSampler, ShadowTexCoords).r;
        float actualDepth = input.LightViewPosition.z / input.LightViewPosition.w;

        if ((actualDepth - 0.01f) <= shadowDepth)
        {
            diffuseLightingFactor = 1;
        }
    }

    float diffuseLighting = DotProduct(LightPosition, input.Position3D, texNormal);
    diffuseLighting = saturate(diffuseLighting);
    diffuseLighting *= LightPower;
    diffuseLighting += specular;
    diffuseLighting *= diffuseLightingFactor;
    diffuseLighting += AmbientColor * AmbientIntensity;

    output.Color = saturate(tex2D(BaseTextureSampler, input.TexCoord) * diffuseLighting);
    output.Color.w = 1;
    return output;
}
// End Color Shader

// Shadow Shader
struct ShadowVertexInput
{
    float4 Position : POSITION0;
};

struct ShadowVertexToPixel
{
    float4 Position : POSITION;
    float4 Position2D : TEXCOORD0;
};

struct ShadowPixelToFrame
{
    float4 Color : COLOR0;
};

ShadowVertexToPixel ShadowVertexShaderFunction(ShadowVertexInput input)
{
    ShadowVertexToPixel output = (ShadowVertexToPixel)0;

    output.Position = mul(input.Position, mul(World, mul(LightView, LightProjection)));
    output.Position2D = output.Position;

    return output;
}

ShadowPixelToFrame ShadowPixelShaderFunction(ShadowVertexToPixel input)
{
    ShadowPixelToFrame output = (ShadowPixelToFrame)0;

    output.Color = input.Position2D.z / input.Position2D.w;
    output.Color.w = 1;

    return output;
}
// End Shadow Shader

technique Ambient
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique ShadowMap
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 ShadowVertexShaderFunction();
        PixelShader = compile ps_2_0 ShadowPixelShaderFunction();
    }
}
