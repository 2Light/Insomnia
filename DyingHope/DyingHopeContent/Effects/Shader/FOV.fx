sampler TextureSampler : register(s0);
float OuterVig ;

float4 Fov(float2 texCoord: TEXCOORD0) : COLOR
{
	float InnerVig = 0.00001f;

	float4 color = tex2D(TextureSampler,texCoord);


	float2 center = float2(0.5,0.5);

	float dist = distance(center,texCoord)*1.414213;

	float vig = clamp((OuterVig-dist) / (OuterVig-InnerVig),0.00,1.0);

	color = lerp(color, color * vig , 0.6);
	//float4 c = lerp(lerp(color, color * x * 2.0, 0.084), float4(x, x, x,1.0), noiseInterpolation);

    return color;
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 Fov();
    }
}
