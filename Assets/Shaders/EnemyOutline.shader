Shader "Unlit/EnemyOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth("Outline width", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _OutlineColor;
            float _OutlineWidth;

            static float D = 0.7;
            static float2 _dirs[8] = {float2(1, 0), float2(-1, 0), 
                float2(0, 1), float2(0, -1), float2(D, D), float2(-D, D),
                float2(D, -D), float2(-D, -D)};

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.color = v.color;
                return o;
            }

            float GetMaxAlpha(float2 uv)
            {
                float result = 0;
                for (uint i = 0; i < 8; i++)
                {
                    float2 sUV = uv + _dirs[i] * _OutlineWidth;
                    result = max(result, tex2D(_MainTex, sUV).a);
                }
                return result;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= i.color;

                col.rgb = lerp(_OutlineColor, col.rgb, col.a);
                col.a = GetMaxAlpha(i.uv);
                return col;
            }
            ENDCG
        }
    }
}
