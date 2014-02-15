sampler TextureSampler : register(s0);

float4 Invert(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 color = tex2D(TextureSampler,float2(1-texCoord.x,texCoord.y));
	 
   return color;
}

technique both
{

	pass Invert
	{
		PixelShader = compile ps_2_0 Invert();
	}
	
}
