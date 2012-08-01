static float pi =  3.14159265;
static float HueStepRadians = (pi * 2) / 6;
static float ChordLength = (2 * 0.7 * sin( (pi * 2 / 3) / 2 )) - 0.2;
static float3 Hues[7] = 
	{ 
		float3(1,0,0), 
		float3(1,1,0),
		float3(0,1,0),
		float3(0,1,1),
		float3(0,0,1),
		float3(1,0,1),
		float3(1,0,0)
	};

float2 HuePoint;
float2 WhitePoint;
float2 BlackPoint;
float Hue;

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 Texcoord : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	output.Position = input.Position;
	output.Texcoord = input.Position;
   return output;
}

struct PixelShaderOutput
{
    float4 Color : COLOR0;
};
	
	 float CrossZ(float2 A, float2 B)
		{
			return (B.y * A.x) - (B.x * A.y);
		}

		float AngleBetweenVectors(float2 A, float2 B)
		{
		   A = normalize(A);
		   B = normalize(B);
		   float Dot = dot(A, B);
		   Dot = clamp(Dot, -1, 1);
		   float Angle = acos(Dot);
		   if (CrossZ(A,B) < 0) return (2 * pi) - Angle;
		   else return Angle;
		}
		
float GetHue(float2 Pos)
{
	return AngleBetweenVectors(Pos, float2(1,0)) / HueStepRadians;
}

float3 GetColorFromHue(float Hue)
{
	int BaseHue = floor(Hue);
	float P = Hue - BaseHue;
	return Hues[BaseHue] * (1 - P) + Hues[BaseHue + 1] * P;
}
		
PixelShaderOutput HueWheelPS(VertexShaderOutput input) : COLOR0
{
    PixelShaderOutput output;
    
    float Length = length(input.Texcoord);
    if (Length > 0.69 && Length <= 0.91)
    {
		output.Color.rgb = GetColorFromHue(GetHue(input.Texcoord));
		output.Color.a = 1;
		
		if (Length <= 0.7) 
			output.Color *= 1 - ((0.7 - Length) * 100);
		else if (Length > 0.9)
			output.Color *= (0.91 - Length) * 100;
		
	}
	else
		output.Color = float4(0,0,0,0);
	
    return output;
}

PixelShaderOutput ShadeTrianglePS(VertexShaderOutput input) : COLOR0
{
	PixelShaderOutput output;
	
	float3 Color = GetColorFromHue(Hue);
	
	float HueDistance = length(HuePoint - input.Texcoord) / ChordLength;
	HueDistance -= 0.1f;
	HueDistance = clamp(HueDistance, 0, 1);
	
	output.Color.rgb = Color * (1 - HueDistance);
	
	float WhiteDistance = length(WhitePoint - input.Texcoord) / ChordLength;
	WhiteDistance -= 0.1f;
	WhiteDistance = clamp(WhiteDistance, 0, 1);
	
    output.Color.rgb += float3(1,1,1) * (1 - WhiteDistance);

	output.Color.a = 1;
	
	return output;
}

technique DrawHueWheel
{
    pass Pass1
    {
		AlphaBlendEnable = true;
		BlendOp = Add;
		SrcBlend = One;
		DestBlend = Zero;
		//AlphaFunc = Greater;
		//AlphaRef = 0;
		ZEnable = false;
		ZWriteEnable = false;
		ZFunc = LessEqual;
		
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 HueWheelPS();
    }
}

technique DrawShadeTriangle
{
	pass Pass1
    {
		AlphaBlendEnable = true;
		BlendOp = Add;
		SrcBlend = One;
		DestBlend = Zero;
		//AlphaFunc = Greater;
		//AlphaRef = 0;
		ZEnable = false;
		ZWriteEnable = false;
		ZFunc = LessEqual;
		
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 ShadeTrianglePS();
    }
}



