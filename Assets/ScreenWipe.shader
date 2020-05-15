Shader "Hidden/ScreenWipe"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TransitionTex ("Texture", 2D) = "white" {}
        _Cutoff("Color cutoff", Range (0.0,1.0)) = 0.0
        _Color("Color for cutoff pixels", Color) = (0.0,0.0,0.0,1.0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed4 _Color;
            float _Cutoff;
            sampler2D _TransitionTex;
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 transit = tex2D(_TransitionTex,i.uv);
                if (transit.b <= _Cutoff)
                    return _Color;
                return tex2D(_MainTex,i.uv);
            }
            ENDCG
        }
    }
}
