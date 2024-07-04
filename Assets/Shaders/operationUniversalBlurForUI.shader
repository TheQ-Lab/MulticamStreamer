Shader "Unify/operationUniversalBlurForUI"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _Start1 ("Start of input position", Float) = 0
        _Stop1 ("Stop of input position", Float) = 0
        _Start2 ("Start of output opacity", Float) = 0
        _Stop2 ("Stop of output opacity", Float) = 0

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _GlobalFullScreenBlurTexture;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color;
                OUT.screenPos = ComputeScreenPos(OUT.vertex);
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 mainTexColor = tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd;
                
                half4 color = (tex2D(_GlobalFullScreenBlurTexture, IN.screenPos) + _TextureSampleAdd) *  (IN.color); // "1/" is new, inverts color and lets me make white opaque instead of black

                #ifdef UNITY_UI_CLIP_RECT
                mainTexColor.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
                
                #ifdef UNITY_UI_ALPHACLIP
                clip (mainTexColor.a - 0.001);
                #endif
                
                //color *= mainTexColor;
    //float gradient = (1 / IN.texcoord.x);
    float gradient = IN.texcoord.x;
    
    gradient = clamp(gradient, 0.0, 1.0);
    
    
    gradient = sqrt(gradient);
    
    //half4 remapBounds = half4(1.2, 2.0, 0., 2.0);
    //half4 remapBounds = half4(0.4, 1, 4.9, 0);
    //half4 remapBounds = half4(0.5, 0.99, 0.45, 0);
    //half4 remapBounds = half4(0.90, 0.99, 0.17, 0);
    half4 remapBounds = half4(0.58, 0.85, 0.17, 0.01);

    gradient = remapBounds.z + (remapBounds.w - remapBounds.z) * ((gradient - remapBounds.x) / (remapBounds.y - remapBounds.x));
    //gradient = clamp(gradient, 1., 300.);
    //color *= IN.texcoord.x; // new!
    //color *= (mainTexColor * gradient);
    //color *= gradient;
    //color = 0;
    
    gradient = clamp(gradient, 0.0, 1.0);
    gradient = clamp(gradient, remapBounds.w, remapBounds.z);
    
    //color += gradient;
    color += ((1 - color) * gradient);
    color = clamp(color, .0, 1.0);
                return color;
            }
        ENDCG
        }
    }
}