Shader "PostProcessing/SobelOutline"
{
	Properties 
	{
		_MainText ("Main Texture", 2D) = "white" {}

		_OutlineThickness ("Outline Thickness", float) = 1
		_OutlineDepthMultiplier ("Outline Depth Multiplier", float) = 1
		_OutlineDepthBias ("Outline Depth Bias", float) = 1
		_OutlineNormalBias ("Outline Normal Bias", float) = 1

		_OutlineColor ("Outline Colour", Color) = (1, 1, 1, 1)
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" 
			   "RenderPipeline" = "UniversalPipeline" 
		       "Queue" = "Overlay"}
		
        Cull off
        ZWrite off
        ZTest Always

		Pass
		{
            HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag

            #include "UnityCG.cginc"
            
			sampler2D _MainTex;

			texture2D _CameraDepthTexture;
			SamplerState sampler_CameraDepthTexture;

			sampler2D _CameraGBufferTexture2;
			SamplerState sampler_CameraGBufferTexture2;

			uniform float _OutlineThickness;
			uniform float _OutlineDepthMultiplier;
			uniform float _OutlineDepthBias;
			uniform float _OutlineNormalBias;

			uniform float4 _OutlineColor;

			float SobelSampleDepth(Texture2D t, SamplerState s, float2 uv, float3 offset)
			{
				float pixelCenter = LinearEyeDepth(t.Sample(s, uv).r);
				float pixelLeft   = LinearEyeDepth(t.Sample(s, uv - offset.xz).r);
				float pixelRight  = LinearEyeDepth(t.Sample(s, uv + offset.xz).r);
				float pixelUp     = LinearEyeDepth(t.Sample(s, uv + offset.zy).r);
				float pixelDown   = LinearEyeDepth(t.Sample(s, uv - offset.zy).r);

				return length(sqrt(pow(pixelRight - pixelLeft, 2) + pow(pixelUp - pixelDown, 2)));
			}

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
            };

			v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target 
            {
				float3 offset = float3((1.0 / _ScreenParams.x), (1.0 / _ScreenParams.y), 0.0) * _OutlineThickness;

				float sobelDepth = SobelSampleDepth(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv.xy, offset);
				sobelDepth = pow(saturate(sobelDepth) * _OutlineDepthMultiplier, _OutlineDepthBias);


				float3 sobelNormalVec = SobelSample(_CameraGBufferTexture2, sampler_CameraGBufferTexture2, i.uv.xy, offset).rgb;
		        float sobelNormal = sobelNormalVec.x + sobelNormalVec.y + sobelNormalVec.z;

				float sobelOutline = saturate(max(sobelDepth, sobelNormal));

				float3 outlineColor = lerp(sceneColor, _OutlineColor.rgb, _OutlineColor.a);
				color = lerp(sceneColor, outlineColor, sobelOutline);

				return float4(color, 1.0);
            }
            
			ENDHLSL
		}
	} 
	FallBack "Diffuse"
}