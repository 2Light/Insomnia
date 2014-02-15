sampler TextureSampler : register(s0);
float time ;
float noiseInterpolation;
float4 Noise(float2 texCoord: TEXCOORD0) : COLOR
{
	float InnerVig = 0.00001f;

	float4 color = tex2D(TextureSampler,texCoord);


	float2  uv = texCoord*0.001;
    float x = frac(sin(dot(uv*time, float2( 12.9898, 78.233)))*43758.5453) * 0.85;
	float4 OUT = float4(x, x, x, 1.0);

	float4 c = lerp(lerp(color, color * x * 2.0, 0.084), float4(x, x, x,1.0), noiseInterpolation);

    return c;
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 Noise();
    }
}
