Shader "UI/DirtCutout"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _MaskTex ("Mask Texture (RenderTexture)", 2D) = "black" {}
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
        ZTest [UNITY_GUI_ZTEST_MODE] 
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t 
            { 
                float4 vertex : POSITION; 
                float4 color : COLOR; 
                float2 texcoord : TEXCOORD0; 
            };
            
            struct v2f 
            { 
                float4 vertex : SV_POSITION; 
                fixed4 color : COLOR; 
                float2 texcoord : TEXCOORD0; 
            };

            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _MaskTex;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                fixed4 mask = tex2D(_MaskTex, i.texcoord);
                
                // Erases the texture wherever your white brush strokes are painted
                col.a *= (1.0 - mask.r);
                return col;
            }
            ENDCG
        }
    }
}