Shader "Custom/GodRays"
{
    Properties
    {
        _RayColor ("Ray Color", Color) = (1, 0.95, 0.8, 1)
        _RayIntensity ("Ray Intensity", Range(0, 4)) = 2
        _RaySpeed ("Ray Speed", Range(0, 2)) = 0.5
        _RayWidth ("Ray Width", Range(1, 20)) = 10
        _RayAngle ("Ray Angle", Range(-1, 1)) = 0.3
        _BottomBrightness ("Bottom Brightness", Range(1, 10)) = 3
        _AlphaCutoff ("Alpha Cutoff", Range(0, 1)) = 0
    }
    
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Blend One One
        ZWrite Off
        Cull Off

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
            };

            float4 _RayColor;
            float _RayIntensity;
            float _RaySpeed;
            float _RayWidth;
            float _RayAngle;
            float _BottomBrightness;
            float _AlphaCutoff;

            float hash(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * float3(.1031, .1030, .0973));
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }

            float rays(float2 uv)
            {
                // Invert UV for bottom-up effect
                float2 flipped_uv = float2(uv.x, 1.0 - uv.y);
                
                // Apply angle to UV coordinates
                float2 angleOffset = float2(flipped_uv.y * _RayAngle, 0);
                float2 angled_uv = flipped_uv + angleOffset;
                
                // Animate UV space (inverted width calculation for thinner rays at lower values)
                float timeOffset = _Time.y * _RaySpeed;
                float adjustedWidth = 50.0 / _RayWidth; // Inverse relationship
                float2 moved = float2(angled_uv.x * adjustedWidth + timeOffset, angled_uv.y);
                
                // Create base ray pattern
                float ray = hash(floor(moved));
                
                // Bottom-up gradient
                float bottomGradient = pow(uv.y, 2.0);
                float brightnessGradient = lerp(0.5, _BottomBrightness, bottomGradient);
                ray *= brightnessGradient;
                
                // Strengthen rays at the bottom
                ray *= uv.y * 2.0;
                
                return ray;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Get base ray value
                float ray = rays(i.uv);
                
                // Add variation
                float variation = hash(i.uv * 10.0 + _Time.y * _RaySpeed * 0.5);
                ray *= 0.8 + 0.2 * variation;
                
                // Enhanced bottom brightness
                float bottomBoost = pow(i.uv.y, 3.0) * 2.0;
                ray *= 1.0 + bottomBoost;
                
                // Fade out at screen edges
                float edgeFade = 1.0 - pow(abs(i.uv.x - 0.5) * 2.0, 2.0);
                ray *= edgeFade;
                
                // Apply intensity and create final color
                float4 finalColor = _RayColor * ray * _RayIntensity;

                if (finalColor.w < _AlphaCutoff * edgeFade) return float4(0.0, 0.0, 0.0, 0.0);
                
                return finalColor;
            }
            ENDCG
        }
    }
}