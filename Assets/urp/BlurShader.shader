Shader "Custom/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlurSize;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 offset = _BlurSize / _ScreenParams.xy;
                fixed4 color = fixed4(0, 0, 0, 0);

                color += tex2D(_MainTex, i.uv + offset * float2(-1, -1)) * 0.0625;
                color += tex2D(_MainTex, i.uv + offset * float2(-1,  0)) * 0.125;
                color += tex2D(_MainTex, i.uv + offset * float2(-1,  1)) * 0.0625;

                color += tex2D(_MainTex, i.uv + offset * float2( 0, -1)) * 0.125;
                color += tex2D(_MainTex, i.uv + offset * float2( 0,  0)) * 0.25;
                color += tex2D(_MainTex, i.uv + offset * float2( 0,  1)) * 0.125;

                color += tex2D(_MainTex, i.uv + offset * float2( 1, -1)) * 0.0625;
                color += tex2D(_MainTex, i.uv + offset * float2( 1,  0)) * 0.125;
                color += tex2D(_MainTex, i.uv + offset * float2( 1,  1)) * 0.0625;

                return color;
            }
            ENDCG
        }
    }
}
