// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PlanetShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}

        _OffsetX("Offset X", Float) = 0
        _OffsetY("Offset Y", Float) = 0

        _ScaleX("Scale Y", Float) = 1
        _ScaleY("Scale Y", Float) = 1
    }

    SubShader
    {
        Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
    
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

            float3 offset(float3 vec, float x, float y)
            {
                return float3(vec.xyz) + float3(x, y, 0);
            };


            float3 stretch(float3 vec, float x, float y)
            {
                float2x2 stretchMatrix = float2x2(x, 0, 0, y);
                return float3(mul(stretchMatrix, vec.xy), vec.z).xyz;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _OffsetX;
            float _OffsetY;

            float _ScaleX;
            float _ScaleY;

            v2f vert(appdata v)
            {
                v2f o;

                float3 offset_vertex = offset(v.vertex, _OffsetX, _OffsetY);
                float3 stretched_offset_vertex = stretch(offset_vertex, _ScaleX, _ScaleY);

                o.vertex = UnityObjectToClipPos(stretched_offset_vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col = col * col.a;
                return col;
            }
            ENDCG
        }
    }
}
