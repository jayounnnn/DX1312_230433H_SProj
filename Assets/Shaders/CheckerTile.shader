Shader "Custom/CheckerTile"
{
    Properties
    {
        _MainTex ("Checker Texture", 2D) = "white" {}
        _Scale ("Tile Size", Float) = 1
        _BorderWidth ("Tile Border Width", Range(0, 0.1)) = 0.02
        _BlockBorderWidth ("Block Border Width", Range(0, 0.5)) = 0.05
        _BorderColor ("Border Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
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
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Scale;
            float _BorderWidth;
            float _BlockBorderWidth;
            fixed4 _BorderColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 gridUV = i.worldPos.xz / _Scale;
                float2 cell = floor(gridUV);
                float2 localUV = frac(gridUV);

                // 8x8 block positioning
                float2 blockUV = i.worldPos.xz / (_Scale * 8.0);
                float2 blockFrac = frac(blockUV);

                // Check for block border
                if (blockFrac.x < _BlockBorderWidth || blockFrac.y < _BlockBorderWidth)
                {
                    return _BorderColor;
                }

                // Tile border inside each 1x1 cell
                if (localUV.x < _BorderWidth || localUV.y < _BorderWidth)
                {
                    return _BorderColor;
                }

                float2 atlasUV = (cell.xy / 8.0) + (localUV / 8.0);
                return tex2D(_MainTex, atlasUV);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
