Shader "Knots/PointMassShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _StretchX("Stretch X", Float) = 1
        _StretchY("Stretch Y", Float) = 1
        _Color("Color", Color) = (1, 1, 1, 1)
    }

        SubShader
        {
            // No culling or depth
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "False"
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

                float3 stretch(float3 vec, float x, float y)
                {
                    float2x2 stretchMatrix = float2x2(x, 0, 0, y);
                    return float3(mul(stretchMatrix, vec.xy), vec.z).xyz;
                };

                float _StretchX;
                float _StretchY;

                v2f vert(appdata v)
                {
                    v2f o;

                    float3 stretched_vertex = stretch(v.vertex,  _StretchX, _StretchY);

                    o.vertex = UnityObjectToClipPos(stretched_vertex);
                    o.uv = v.uv;
                    return o;
                }

                float4 _Color;

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = _Color;
                    col.a = 0.5f;
                    return col;
                }
                ENDCG
            }
        }
}
