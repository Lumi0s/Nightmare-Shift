Shader "Custom/CCTVNoiseEffect"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}        // The main camera texture (e.g., the camera feed)
        _NoiseTex ("Noise Texture", 2D) = "black" {}      // The texture for noise (grayscale)
        _NoiseAmount ("Noise Amount", Range(0, 1)) = 0.5  // Amount of noise to add (0 is no noise, 1 is maximum noise)
        _NoiseOffset ("Noise Offset", Vector) = (0, 0, 0, 0) // Offset for the noise texture
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
            sampler2D _NoiseTex;
            float _NoiseAmount;
            float4 _NoiseOffset;

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

                // Apply the noise offset
                float2 noiseUV = i.uv + _NoiseOffset.xy;

                // Sample the noise texture (grayscale noise)
                fixed4 noiseColor = tex2D(_NoiseTex, noiseUV);

                // Blend the noise with the main texture
                fixed4 finalColor = lerp(mainColor, noiseColor, _NoiseAmount);

                // Return the final color, ensuring it doesn't go out of bounds
                return saturate(finalColor);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}