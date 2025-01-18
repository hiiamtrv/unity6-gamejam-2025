Shader "Custom/UnderwaterCaustics"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.0, 0.3, 0.5, 1.0)
        _CausticColor ("Caustic Color", Color) = (1.0, 1.0, 0.8, 1.0)
        _Scale ("Scale", Range(1, 50)) = 10
        _Speed ("Speed", Range(0.1, 10)) = 1
        _Sharpness ("Sharpness", Range(1, 10)) = 3
        _Intensity ("Intensity", Range(0, 2)) = 1
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            float4 _BaseColor;
            float4 _CausticColor;
            float _Scale;
            float _Speed;
            float _Sharpness;
            float _Intensity;

            // Improved wave function for more realistic water surface simulation
            float wave(float2 p, float2 dir, float frequency, float timeShift)
            {
                float wave = dot(dir, p) * frequency + _Time.y * _Speed * timeShift;
                return sin(wave);
            }

            // Creates sharper peaks characteristic of caustic patterns
            float causticPattern(float x)
            {
                return pow(max(0, x), _Sharpness);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 p = i.worldPos.xz * _Scale * 0.1;
                
                // Combine multiple wave patterns at different angles and frequencies
                float pattern = 0;
                pattern += wave(p, float2(1, 1), 3.0, 1.0);
                pattern += wave(p, float2(-1, 0.8), 2.7, 1.1);
                pattern += wave(p, float2(0.8, -1), 2.4, 0.9);
                pattern += wave(p, float2(-0.8, -0.9), 2.9, 1.2);
                
                // Normalize and sharpen the pattern
                pattern = pattern * 0.25 + 0.5;
                pattern = causticPattern(pattern);
                
                // Add some variation to break up uniformity
                float2 offset = float2(_Time.y * 0.5, _Time.y * 0.4);
                float variation = wave(p + offset, float2(1, -1), 5.0, 1.5);
                pattern *= 1.0 + 0.2 * variation;
                
                // Apply intensity and blend colors
                float caustics = pattern * _Intensity;
                float3 finalColor = lerp(_BaseColor.rgb, _CausticColor.rgb, caustics);
                
                return float4(finalColor, 1.0);
            }
            ENDCG
        }
    }
}