Shader "Unlit/OutlineShader"
{
    Properties
    {
        _OutlineColour ("Outline Colour", Color) = (1, 1, 1, 1) 
        _OutlineWidth ("Outline Width", float) = 0.1
        _MinOutlineWidth ("Min Outline Width", float) = 0.1
        _MaxOutlineWidth ("Max Outline Width", float) = 1

        _MinOutlineZ("Min Outline Z", Range(-0.01, 0)) = -.001
		_MaxOutlineZ("Max Outline Z", Range(-0.01, 0)) = -.001
        _MaxWCamDist("Max Width CamDist", float) = 500
        _MaxZCamDist("Max Z CamDist", float) = 500

    }

    SubShader
    {
        Tags { "RenderType"="Opaque"
               "Queue" = "Geometry-1"
               "DisableBatching" = "true"}

        LOD 100

        Pass
        {
            Stencil
            {
                Ref 2
                Comp Always
                Pass Zero
            }

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
            };

            uniform float _OutlineWidth;

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            uniform float _MinOutlineZ;
            uniform float _MaxOutlineZ;

            uniform float _MaxZCamDist;
            uniform float _MaxWCamDist;

            uniform float _MinOutlineWidth;
            uniform float _MaxOutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float2 offset = TransformViewToProjection(norm.xy);

                float camDist = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, o.pos));

                float cameraAdjustedOutlineWidth = _OutlineWidth * clamp(camDist, camDist,  camDist / _MaxWCamDist);

                o.pos.xy += offset * clamp(cameraAdjustedOutlineWidth, _MinOutlineWidth, _MaxOutlineWidth);

                float normCamDistance = camDist/_MaxZCamDist;
                float clampedOutlineZ = clamp(lerp(_MinOutlineZ, _MaxOutlineZ, normCamDistance), _MinOutlineZ, _MaxOutlineZ);

                o.pos.z += clampedOutlineZ;

                return o;
            }

            uniform float4 _OutlineColour;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _OutlineColour;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
