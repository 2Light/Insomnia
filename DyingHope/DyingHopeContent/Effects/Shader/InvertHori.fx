sampler TextureSampler : register(s0);

float4 InvertHori(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 color = tex2D(TextureSampler,float2(texCoord.x,1-texCoord.y));
	 
   return color;
}

technique both
{

	pass Invert
	{
		PixelShader = compile ps_2_0 InvertHori();
	}
	
}
