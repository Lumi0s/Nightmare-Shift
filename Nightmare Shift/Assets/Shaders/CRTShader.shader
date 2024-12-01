Shader "Custom/CRTShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0, 1)) = 0.5
        _Distortion ("Distortion Amount", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScanlineIntensity;
            float _Distortion;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Add barrel distortion
                uv -= 0.5;
                uv += uv * _Distortion * (uv.x * uv.x + uv.y * uv.y);
                uv += 0.5;

                // Sample the texture
                float4 color = tex2D(_MainTex, uv);

                // Add scanlines
                float scanline = sin(i.uv.y * 800) * _ScanlineIntensity;
                color.rgb -= scanline;

                // Slightly offset RGB for chromatic aberration
                color.r = tex2D(_MainTex, uv + float2(0.002, 0)).r;
                color.b = tex2D(_MainTex, uv + float2(-0.002, 0)).b;

                return color;
            }
            ENDCG
        }
    }
}
