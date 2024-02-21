Shader "Unlit/OutlineShader"
{
    Properties
    {
        _OutlineColour ("Outline Colour", Color) = (1, 1, 1, 1) 
        _OutlineWidth ("Outline Width", float) = 0.1

        _outlineZ("OutlineZ", Range(-0.01, 0)) = -.001
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
                Pass Replace
            }

            Cull Off
            ZWrite On

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
            float4 _MainTex_ST;

            uniform float _outlineZ;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float2 offset = TransformViewToProjection(norm.xy);

                float camDist = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, o.pos));

                o.pos.xy += offset * _OutlineWidth;

                o.pos.z += _outlineZ;

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
