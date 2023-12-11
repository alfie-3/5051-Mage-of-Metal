Shader "PostProcessing/SobelOutline"
{
	Properties 
	{
		_OutlineThickness ("Outline Thickness", float) = 1
		_OutlineDepthMultiplier ("Outline Depth Multiplier", float) = 1
		_OutlineNormalMultiplier ("Outline Normal Multiplier", float) = 1
		_OutlineDepthBias ("Outline Depth Bias", float) = 1
		_OutlineNormalBias ("Outline Normal Bias", float) = 1

		_OutlineColor ("Outline Colour", Color) = (1, 1, 1, 1)
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
		
		LOD 100
        Cull off
        ZWrite off
        ZTest Always

		Pass
		{
            HLSLPROGRAM

			#pragma vertex Vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
			#include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            
            TEXTURE2D(_MainTex);
			SamplerState sampler_MainTex;

			texture2D _CameraDepthTexture;
			SamplerState sampler_CameraDepthTexture;

			texture2D _CameraDepthNormalsTexture;
			SamplerState sampler_CameraDepthNormalsTexture;

			uniform float _OutlineThickness;
			uniform float _OutlineDepthMultiplier;
			uniform float _OutlineDepthBias;
			uniform float _OutlineNormalBias;
			uniform float _OutlineNormalMultiplier;

			uniform float4 _OutlineColor;

			TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

			inline float LinearEyeDepthDEPTH( float z )
			{
				return 1.0 / (_ZBufferParams.z * z + _ZBufferParams.w);
			}

			float SobelSampleDepth(Texture2D t, SamplerState s, float2 uv, float3 offset)
			{
				float pixelCenter = LinearEyeDepthDEPTH(t.Sample(s, uv).r);
				float pixelLeft   = LinearEyeDepthDEPTH(t.Sample(s, uv - offset.xz).r);
				float pixelRight  = LinearEyeDepthDEPTH(t.Sample(s, uv + offset.xz).r);
				float pixelUp     = LinearEyeDepthDEPTH(t.Sample(s, uv + offset.zy).r);
				float pixelDown   = LinearEyeDepthDEPTH(t.Sample(s, uv - offset.zy).r);

				return length(sqrt(pow(pixelRight - pixelLeft, 2) + pow(pixelUp - pixelDown, 2)));
			}

			float4 SobelSample(Texture2D t, SamplerState s, float2 uv, float3 offset)
			{
				float4 pixelCenter = t.Sample(s, uv);
				float4 pixelLeft   = t.Sample(s, uv - offset.xz);
				float4 pixelRight  = t.Sample(s, uv + offset.xz);
				float4 pixelUp     = t.Sample(s, uv + offset.zy);
				float4 pixelDown   = t.Sample(s, uv - offset.zy);
    
				return abs(pixelLeft  - pixelCenter) +
					   abs(pixelRight - pixelCenter) +
					   abs(pixelUp    - pixelCenter) +
					   abs(pixelDown  - pixelCenter);
			}

            float4 frag (Varyings i) : SV_Target 
            {
				// Sample the scene and our depth buffer
				float3 offset     = float3((1.0 / _ScreenParams.x), (1.0 / _ScreenParams.y), 0.0) * _OutlineThickness;
				float3 sceneColor = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, i.texcoord).rgb;
				float  sobelDepth = SobelSampleDepth(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord.xy, offset);

				sobelDepth = pow(saturate(sobelDepth) * _OutlineDepthMultiplier, _OutlineDepthBias);

				// Sample the normal buffer and build a composite scalar value
				float3 sobelNormalVec = SobelSample(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.texcoord.xy, offset).rgb;
				float sobelNormal = sobelNormalVec.x + sobelNormalVec.y + sobelNormalVec.z;

				sobelNormal = pow(sobelNormal * _OutlineNormalMultiplier, _OutlineNormalBias);

				float sobelOutline = saturate(max(sobelDepth, sobelNormal));

				float3 outlineColor = lerp(sceneColor, _OutlineColor.rgb, _OutlineColor.a);
			    const float3 color = lerp(sceneColor, outlineColor, sobelOutline);

				return float4(color, 1.0);
            }
            
			ENDHLSL
		}
	} 
	FallBack "Diffuse"
}