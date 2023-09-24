float4x4 xView;
float4x4 xProjection;

struct VertexToPixel
{
    float4 Position     : POSITION;
    float4 Color        : COLOR0;
};

struct PixelToFrame
{
    float4 Color        : COLOR0;
};

VertexToPixel SimplestVertexShader(float4 inPos : POSITION)
{
    VertexToPixel Output = (VertexToPixel)0;

    Output.Position = mul(inPos, xView);
    Output.Position = mul(Output.Position, xProjection);
    Output.Color = 1.0f;

    return Output;
};

PixelToFrame OurFirstPixelShader(VertexToPixel PSIn)
{
    PixelToFrame Output = (PixelToFrame)0;

    Output.Color = PSIn.Color;

    return Output;
}

technique Simplest
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 SimplestVertexShader();
        PixelShader = compile ps_2_0 OurFirstPixelShader();
    }
};
