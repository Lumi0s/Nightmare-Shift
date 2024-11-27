Shader "Custom/ScrollingLinesEffect"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}       // The main camera texture
        _LineTex ("Line Texture", 2D) = "white" {}       // The texture for scrolling lines
        _ScrollSpeed ("Scroll Speed", Float) = 0.2        // Speed of the scrolling lines
        _BlendAmount ("Blend Amount", Range(0,1)) = 0.5   // Blend intensity of lines
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

            sampler2D _MainTex;
            sampler2D _LineTex;
            float _ScrollSpeed;
            float _BlendAmount;

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
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the main texture (camera feed)
                fixed4 mainColor = tex2D(_MainTex, i.uv);

                // Calculate the scrolling offset for the line texture
                float2 lineUV = i.uv;
                lineUV.y += _Time.y * _ScrollSpeed;

                // Sample the line texture with the scrolling offset
                fixed4 lineColor = tex2D(_LineTex, lineUV);

                // Blend the lines with the main texture without changing its color
                fixed4 finalColor = lerp(mainColor, lineColor, _BlendAmount);
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}
