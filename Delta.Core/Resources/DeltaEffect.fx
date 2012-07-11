sampler2D sample: register(s0);
sampler2D sample2: register(s1);

float4 Color = float4(1,1,1,1);

float4 tint(float4 tintColor : COLOR0, float2 texCoord : TEXCOORD0) : SV_Target0
{
    return tex2D(sample, texCoord) * tintColor;
}

float4 fill(float4 tintColor : COLOR0, float2 texCoord : TEXCOORD0) : SV_Target0
{
	float4 color = tex2D(sample, texCoord);
	color.rgb = Color.rgb;
	return color * tintColor;
}

float4 grayscale(float4 tintColor : COLOR0, float2 texCoord : TEXCOORD0) : SV_Target0
{
	float4 color = tex2D(sample, texCoord);
	color.rgb = 0.2989 * color.r + 0.5870 * color.g + 0.1140 * color.b;
	return color * tintColor;
}

float4 negative(float4 tintColor : COLOR0, float2 texCoord : TEXCOORD0) : SV_Target0
{
	float4 color = tex2D(sample, texCoord);
	color.rgb =  1 - color.rgb;
	return color * tintColor;
}

float4 overlay(float4 tintColor : COLOR0, float2 texCoord : TEXCOORD0) : SV_Target0
{
	float4 baseColor = tex2D(sample, texCoord);
	float4 blendColor = tex2D(sample2, texCoord);
	float4 finalColor = baseColor.a;

	// un-premultiply the blendColor alpha out from blendColor
	blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);
	
	//automatically apply one set of conditionals,.My wishful thinking is that it will speed up this routine
	//finalColor.rgb = 1 - (1 - 2 * (baseColor.rgb - 0.5)) * (1 - blendColor.rgb);
	finalColor.rgb = 1 - 2 * (1 - baseColor.rgb) * (1 - blendColor.rgb);

	if (baseColor.r < 0.5)
		finalColor.r = 2 * baseColor.r * blendColor.r;
	if (baseColor.r < 0.5)
		finalColor.g = 2 * baseColor.g * blendColor.g;
	if (baseColor.r < 0.5)
		finalColor.b = 2 * baseColor.b * blendColor.b;

	// re-multiply the blendColor alpha in to blendColor, weight baseColor according to blendColor.a
	finalColor.rgb = (1 - blendColor.a) * baseColor.rgb + finalColor.rgb * blendColor.a;
	
	return finalColor * tintColor;
}

technique Tint
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 tint();
    }
}

technique Grayscale
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 grayscale();
    }
}

technique Negative
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 negative();
    }
}

technique Overlay
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 overlay();
    }
}
