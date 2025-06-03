Shader "UI/GradientHorizontalWithAlphaTint"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color1 ("Color 1 (25%)", Color) = (1, 0, 0.286, 1)
        _Color2 ("Color 2 (70%)", Color) = (0.631, 0.259, 0.604, 1)
        _Stop1 ("Stop 1 (0–1)", Range(0,1)) = 0.25
        _Stop2 ("Stop 2 (0–1)", Range(0,1)) = 0.7
        _Tint ("Tint Color", Color) = (1,1,1,1)
        _OverrideColor ("Override Color", Color) = (1,1,1,1)
        _UseOverrideColor ("Use Override Color", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color1;
            fixed4 _Color2;
            float _Stop1;
            float _Stop2;
            fixed4 _Tint;
            fixed4 _OverrideColor;
            float _UseOverrideColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                float alpha = texColor.a;

                fixed4 finalColor;

                if (_UseOverrideColor > 0.5)
                {
                    finalColor = _OverrideColor;
                }
                else
                {
                    float t = saturate((i.uv.x - _Stop1) / max(_Stop2 - _Stop1, 0.0001));
                    fixed4 gradient = lerp(_Color1, _Color2, t);
                    finalColor = gradient * _Tint;
                }

                finalColor.a *= alpha;
                return finalColor;
            }
            ENDCG
        }
    }
}