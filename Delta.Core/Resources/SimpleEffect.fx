sampler samplerState;
float Time;
float FlickerRate;
float4 Color;
float4 ColorsSrc[4];
float4 ColorsDst[4];

struct PS_INPUT
{
    float2 TexCoord : TEXCOORD0;
};

float4 fill_color(PS_INPUT Input) : COLOR0
{
    float4 col = tex2D(samplerState, Input.TexCoord.xy);

    //if (col.a == 1) // exclude transparency
    if (col.a > 0) // include transparency
    {
        col.rgb = Color.rgb;
    }

    return col;
}

float4 flicker(PS_INPUT Input) : COLOR0
{
    float4 col = tex2D(samplerState, Input.TexCoord.xy);

    if (col.a == 1) {
            col.a = (sin(Time/FlickerRate) * 0.5) + 0.2;
            col.rgb = col.rgb * col.a;
    }

    return col;
}

float4 replace_colors(PS_INPUT Input) : COLOR0
{
    float4 col = tex2D(samplerState, Input.TexCoord.xy);

    if (col.a == 1) {
        float num = col.r + col.b + col.g;
        float num1 = ColorsSrc[0].r + ColorsSrc[0].b + ColorsSrc[0].g;
        float num2 = ColorsSrc[1].r + ColorsSrc[1].b + ColorsSrc[1].g;
        float num3 = ColorsSrc[2].r + ColorsSrc[2].b + ColorsSrc[2].g;
        float num4 = ColorsSrc[3].r + ColorsSrc[3].b + ColorsSrc[3].g;

        if (num == num1) {
            col.rgb = ColorsDst[0].rgb;
        }
        if (num == num2) {
            col.rgb = ColorsDst[1].rgb;
        }
        if (num == num3) {
            col.rgb = ColorsDst[2].rgb;
        }
        if (num == num4) {
            col.rgb = ColorsDst[3].rgb;
        }
    }

    return col;
}

technique FillColor
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 fill_color();
    }
}

technique Flicker
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 flicker();
    }
}

technique ReplaceColors
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 replace_colors();
    }
}
