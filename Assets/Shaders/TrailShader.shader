Shader "Knots/ShootingPixelShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _DirectionX("Direction X", Float) = 1
        _DirectionY("Direction Y", Float) = 1
        _Speed("Speed", Float) = 0
        _Transparency("Transparency", Float) = 0.25
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
            "CanUseSpriteAtlas" = "True"
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

            float _DirectionX;
            float _DirectionY;
            float _Speed;

            v2f vert(appdata v)
            {
                v2f o;

                float3 stretched_vertex = stretch(v.vertex, 2 * _DirectionX * _Speed + sign(_DirectionX + 0.5) * 1, 2 * _DirectionY * _Speed + sign(_DirectionY + 0.5) * 1);

                o.vertex = UnityObjectToClipPos(stretched_vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Transparency;

            fixed4 frag(v2f i) : SV_Target
            {

                float s_x = abs(_DirectionX) * _Speed + 1;
                float s_y = abs(_DirectionY) * _Speed + 1;
                float x = abs((i.uv.x * s_x) % _MainTex_ST.x);
                float y = abs((i.uv.y * s_y) % _MainTex_ST.y);
                float2 pos = float2(x, y);

                float indexA = floor(4 * (i.uv.x * s_x) / (_MainTex_ST.x * _Speed)) / 4;
                float indexB = floor(4 * (i.uv.y * s_y) / (_MainTex_ST.y * _Speed)) / 4;
                float index = indexA + indexB;

                fixed4 col = tex2D(_MainTex, pos);

                col.a = col.a * index * _Transparency;
                col.rgb = col.rgb * col.a;

                return col;
            }
            ENDCG
        }
    }
}
