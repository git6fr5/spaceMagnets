Shader "Custom/VertexColorUnlit"
{
    Properties{
    }

    Category{
        Tags { 
            "Queue" = "Geometry"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
        }
        Lighting Off
        BindChannels {
        Bind "Color", color
        Bind "Vertex", vertex
        //Bind "TexCoord", texcoord
    }

    SubShader {
            Pass {

                //CGPROGRAM
                //#pragma vertex vert
                //#pragma fragment frag

                //#include "UnityCG.cginc"

                //struct appdata
                //{
                //    float4 vertex : POSITION;
                //    float2 uv : TEXCOORD0;
                //};

                //struct v2f
                //{
                //    float2 uv : TEXCOORD0;
                //    float4 vertex : SV_POSITION;
                //};

                //sampler2D _MainTex;
                //float4 _MainTex_ST;

                //v2f vert(appdata v)
                //{
                //    v2f o;
                //    o.vertex = UnityObjectToClipPos(v.vertex);
                //    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //    return o;
                //}

                //fixed4 frag(v2f i) : SV_Target
                //{
                //    fixed4 col = float4(1, 0, 0, 0);
                //    // col.a = 0;
                //    return col;
                //}

                //ENDCG
                
            }
        }
    }

}
