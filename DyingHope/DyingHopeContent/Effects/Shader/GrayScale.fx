sampler TextureSampler : register(s0);

float4 GrayScale(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 color = tex2D(TextureSampler,texCoord);
	color.rb = color.g;
    return color;
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 GrayScale();
    }
}
