sampler TextureSampler : register(s0);
float time ;
float glitchIntensity;


float4 Fov(float2 texCoord: TEXCOORD0) : COLOR
{

  //GLITCH FX
  float mult = sin(texCoord.y * time * 100.0) * 220.0;
  float2 texCoord2 = floor(texCoord * mult) / mult;
  float x = (texCoord2.x) * (texCoord2.y) * time;
	
  x = fmod ((fmod (x, 13.0) + 1.0) * (fmod (x, 123.0) + 1.0), 0.01);
  
  texCoord.x += cos(x * 2200.0) * glitchIntensity * x * 200.0;
  texCoord.y += sin(x * 2200.0) * glitchIntensity * x * 200.0;

  //COLOR
  float4 color = tex2D(TextureSampler,texCoord);


  
  //GLITCH FX
  texCoord.x += glitchIntensity * x * 800.0;
  color.r = tex2D(TextureSampler,texCoord).r;
  texCoord.x -= glitchIntensity * x * 1600.0;
  color.b = tex2D(TextureSampler,texCoord).b;
  
  //NOISE
 return  float4(color.r, color.g,color.b,1.0);
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 Fov();
    }
}
