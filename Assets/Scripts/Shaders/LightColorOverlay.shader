Shader "Custom/2D/LightColorOverlay"
{
    Properties
    {
        _Color ("Light Color", Color) = (1,1,1,1)
        _LightThreshold("Light Threshold", Range(0, 1)) = 0.1
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend Source", Float) = 5 // SrcAlpha
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend Destination", Float) = 10 // OneMinusSrcAlpha
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent+1" // Render after other transparent objects
            "RenderPipeline" = "UniversalPipeline"
        }

        Blend [_BlendSrc] [_BlendDst]
        ZWrite Off
        Cull Off

        Pass
        {
            Name "Light Color Overlay"
            Tags
            {
                "LightMode" = "Universal2D"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                float4 screenPosition : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_ShapeLightTexture0);
            SAMPLER(sampler_ShapeLightTexture0);
            float4x4 _ShapeLightMatrix0;

            TEXTURE2D(_ShapeLightTexture1);
            SAMPLER(sampler_ShapeLightTexture1);
            float4x4 _ShapeLightMatrix1;

            TEXTURE2D(_ShapeLightTexture2);
            SAMPLER(sampler_ShapeLightTexture2);
            float4x4 _ShapeLightMatrix2;

            TEXTURE2D(_ShapeLightTexture3);
            SAMPLER(sampler_ShapeLightTexture3);
            float4x4 _ShapeLightMatrix3;

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _LightThreshold;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.screenPosition = ComputeScreenPos(output.positionCS);
                output.color = input.color * _Color;

                return output;
            }

            float SampleLight(float4 screenPos, Texture2D lightTex, SamplerState lightSampler, float4x4 lightMatrix)
            {
                float4 lightCoord = mul(lightMatrix, screenPos);
                float2 lightUV = lightCoord.xy / lightCoord.w;
                return SAMPLE_TEXTURE2D(lightTex, lightSampler, lightUV).r;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float4 screenPos = input.screenPosition;
                float totalLight = 0;

                #if USE_SHAPE_LIGHT_TYPE_0
                totalLight += SampleLight(screenPos, _ShapeLightTexture0, sampler_ShapeLightTexture0, _ShapeLightMatrix0);
                #endif

                #if USE_SHAPE_LIGHT_TYPE_1
                totalLight += SampleLight(screenPos, _ShapeLightTexture1, sampler_ShapeLightTexture1, _ShapeLightMatrix1);
                #endif

                #if USE_SHAPE_LIGHT_TYPE_2
                totalLight += SampleLight(screenPos, _ShapeLightTexture2, sampler_ShapeLightTexture2, _ShapeLightMatrix2);
                #endif

                #if USE_SHAPE_LIGHT_TYPE_3
                totalLight += SampleLight(screenPos, _ShapeLightTexture3, sampler_ShapeLightTexture3, _ShapeLightMatrix3);
                #endif

                // Only show color where there is light
                float4 finalColor = input.color;
                finalColor.a *= smoothstep(_LightThreshold, _LightThreshold + 0.01, totalLight);

                return finalColor;
            }
            ENDHLSL
        }
    }
}