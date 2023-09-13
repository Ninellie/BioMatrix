// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/PixelOutline"
{
    Properties
    {
        _OutlineWidth ("Outline", Range(0, 0.1)) = 0.01
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineEnabled ("Outline Enabled", Range(0, 1)) = 1.0
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata_t
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

float _OutlineWidth;
float4 _OutlineColor;
float _OutlineEnabled;
sampler2D _MainTex;

v2f vert(appdata_t v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

half4 frag(v2f i) : SV_Target
{
    if (_OutlineEnabled > 0.5)
    {
        half4 col = tex2D(_MainTex, i.uv);
        half4 outlineCol = _OutlineColor;
        half outline = fwidth(col.a) * _OutlineWidth;

                    // Apply the outline effect if the pixel is near the alpha boundary
        half alpha = smoothstep(0.5 - outline, 0.5 + outline, col.a);
        return lerp(col, outlineCol, alpha);
    }
    else
    {
        return tex2D(_MainTex, i.uv);
    }
}
            ENDCG
        }
    }   
}