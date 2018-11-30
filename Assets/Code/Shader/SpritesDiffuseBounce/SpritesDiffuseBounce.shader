Shader "Sprites/DiffuseBounce"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" { }
        _Color("Tint", Color) = (1, 1, 1, 1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor("RendererColor", Color) = (1, 1, 1, 1)
        [HideInInspector] _Flip("Flip", Vector) = (1, 1, 1, 1)
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" { }
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		_ImpactPosition("Impact Position", Vector) = (0, 0, 0, 0)
		_ImpactDirection("Impact Direction", Vector) = (0, 0, 0, 0)
		_DamageRadius("Damage Radius", float) = 1
		_AnimationValue("Animation Value", float) = 0
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

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
        #pragma multi_compile _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #include "UnitySprites.cginc"

		float4 _ImpactPosition;
		float4 _ImpactDirection;
		float _DamageRadius;
		float _AnimationValue;

        struct Input
        {
            float2 uv_MainTex;
            fixed4 color;
        };

        void vert( inout appdata_full v, out Input o )
        {
			float4 offset = (_ImpactDirection * _AnimationValue) * (1 - clamp(distance(_ImpactPosition, v.vertex) / _DamageRadius, 0, 1));

            v.vertex = UnityFlipSprite(v.vertex, _Flip);

            #if defined(PIXELSNAP_ON)
            v.vertex = UnityPixelSnap(v.vertex);
            #endif

			v.vertex += offset;

            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color * _RendererColor;
        }

        void surf( Input IN, inout SurfaceOutput o )
        {
            fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;
            o.Albedo = c.rgb * c.a;
            o.Alpha = c.a;
        }
        ENDCG
    }

Fallback "Transparent/VertexLit"
}